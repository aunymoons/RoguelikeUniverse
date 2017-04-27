using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerWeaponController : PlayerComponentController
{
    //Events
    //public delegate void UpdateWeaponAnswerCallback(Vector3 targetPosition, Weapon targetWeapon);
    //public static event UpdateWeaponAnswerCallback OnUpdateWeapon;

    //References
    WeaponManager weaponManager;
    public GameObject weaponHolder, bulletPrefab;
    public Transform bulletSpawnTransform;

    //Pickup
    Weapon currentWeapon;
    byte tempWeaponID;
    List<WeaponObject> weaponsInRange;

    //UI
    public SpriteRenderer weaponSprite;

    //BASE

    protected override void Start()
    {
        base.Start();

        weaponManager = FindObjectOfType<WeaponManager>();

        if (!isLocalPlayer) return;

        weaponsInRange = new List<WeaponObject>();
    }

    protected override void Update()
    {
        base.Update();

        if (!isLocalPlayer) return;
        
        InputManager();
    }

    //INPUT

    void InputManager()
    {
        RotationMovementHandler();

        if (CrossPlatformInputManager.GetButtonDown("Shoot"))
        {
            CmdShoot();
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            PickupWeapon();
        }
    }

    //MOVEMENT

    void RotationMovementHandler()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetDirection = new Vector3(transform.position.x - mousePos.x, transform.position.y - mousePos.y, transform.position.z);

        targetDirection.Normalize();

        if (targetDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(-targetDirection.y, -targetDirection.x) * Mathf.Rad2Deg;
            weaponHolder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        //If joysticks are available
        /*
        if (Input.GetJoystickNames().Length > 0)
        {
            // Get a Directional Vector from the Joystick input / offset from center
            Vector3 targetDirection = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));

            if (targetDirection != Vector3.zero)
            {
                float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                weaponHolder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        //if not
        else
        {
            
        }
        */
    }

    //SHOOTING

    [Command]
    void CmdShoot()
    {
        if (currentWeapon == null) return;
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * 20;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

    }

    //WEAPON PICKUP

    void PickupWeapon()
    {

        //Checks if there is any weapon around
        if (weaponsInRange.Count > 0)
        {
            //Gets the closest weapon
            WeaponObject closestWeaponObject = GetClosestWeaponObject();
            //Clones the weapon he has found
            tempWeaponID = closestWeaponObject.weaponReference.weaponID;

            //If we dont have a weapon
            if (currentWeapon == null)
            {
                //Update it to null
                CmdUpdateWeapon(closestWeaponObject.weaponReference.indexPosition, 0, false);
            }
            else
            {
                //Update it to our current weapon
                CmdUpdateWeapon(closestWeaponObject.weaponReference.indexPosition, currentWeapon.weaponID, true);
            }

            //Equips the cloned weapon
            CmdEquipWeapon(tempWeaponID);

        }
    }

    WeaponObject GetClosestWeaponObject()
    {
        //Get the closest weapon
        float shortestDistance = Mathf.Infinity;
        float currentDistance;
        WeaponObject closestWeaponObject = null;
        for (int i = 0; i < weaponsInRange.Count; i++)
        {
            currentDistance = Vector3.Distance(weaponsInRange[i].transform.position, transform.position);
            if (currentDistance < shortestDistance)
            {
                shortestDistance = currentDistance;
                closestWeaponObject = weaponsInRange[i];
            }
        }
        return closestWeaponObject;
    }

    [Command]
    public void CmdUpdateWeapon(Vector3 targetPosition, byte targetWeaponID, bool weaponExists)
    {
        //OnUpdateWeapon(targetPosition, targetWeapon);
        weaponManager.RpcUpdateWeapon(targetPosition, targetWeaponID, weaponExists);
    }

    [Command]
    void CmdEquipWeapon(byte weaponToEquip)
    {
        RpcEquipWeaponServer(weaponToEquip);
    }

    [ClientRpc]
    void RpcEquipWeaponServer(byte weaponToEquip)
    {
        //Update current weapon reference
        currentWeapon = WeaponManager.weaponList[weaponToEquip].Clone();

        //Update spriteRenderer
        weaponSprite.sprite = WeaponManager.weaponSprites[currentWeapon.spriteType];

        //TODO: Update other weapon variables 
    }


    //TRIGGERS

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocalPlayer) return;
        WeaponObject triggerEnterWeapon = collision.GetComponent<WeaponObject>();
        if (triggerEnterWeapon)
        {
            if (!weaponsInRange.Contains(triggerEnterWeapon))
            {
                weaponsInRange.Add(triggerEnterWeapon);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isLocalPlayer) return;
        WeaponObject triggerExitWeapon = collision.GetComponent<WeaponObject>();
        if (triggerExitWeapon)
        {
            if (weaponsInRange.Contains(triggerExitWeapon))
            {
                weaponsInRange.Remove(triggerExitWeapon);
            }
        }
    }

}
