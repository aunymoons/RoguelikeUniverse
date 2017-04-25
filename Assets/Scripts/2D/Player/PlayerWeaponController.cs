using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerWeaponController : PlayerComponentController
{
    //Events
    public delegate void UpdateWeaponAnswerCallback(Vector3 targetPosition, Weapon targetWeapon);
    public static event UpdateWeaponAnswerCallback OnUpdateWeapon;

    //References
    public GameObject weaponHolder, bulletPrefab;
    public Transform bulletSpawnTransform;

    //Pickup
    Weapon currentWeapon, tempWeapon;
    List<WeaponObject> weaponsInRange;

    //UI
    public SpriteRenderer weaponSprite;

    // Use this for initialization
    protected override void Start()
    {
        //TODO: remove this, testing only
        EquipWeapon(new Weapon(15, "whatever"));

        weaponsInRange = new List<WeaponObject>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        CmdShoot();
        RotationMovementHandler();
        //SwitchWeapon();
        PickupWeapon();
    }

    void RotationMovementHandler()
    {
        // Get a Directional Vector from the Joystick input / offset from center
        Vector3 targetDirection = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));

        if (targetDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            weaponHolder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    [Command]
    void CmdShoot()
    {
        if (CrossPlatformInputManager.GetButtonDown("Shoot"))
        {
            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * 6;

            // Spawn the bullet on the Clients
            NetworkServer.Spawn(bullet);
        }
    }

    void EquipWeapon(Weapon weaponToEquip)
    {
        currentWeapon = weaponToEquip;

        //Update spriteRenderer
        weaponSprite.sprite = WeaponManager.weaponSprites[currentWeapon.spriteType];

        //TODO: Update other weapon variables 



    }
    
    WeaponObject GetClosestWeaponObject()
    {
        //Get the closest weapon
        float shortestDistance = Mathf.Infinity;
        float currentDistance;
        WeaponObject closestWeaponObject = null;
        for(int i = 0; i < weaponsInRange.Count; i++)
        {
            currentDistance = Vector3.Distance(weaponsInRange[i].transform.position, transform.position);
            if ( currentDistance < shortestDistance)
            {
                shortestDistance = currentDistance;
                closestWeaponObject = weaponsInRange[i];
            }
        }
        return closestWeaponObject;
    }
    
    void PickupWeapon()
    {
        //Checks for button press
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            //Checks if there is any weapon around
            if(weaponsInRange.Count > 0)
            {
                //Gets the closest weapon
                WeaponObject closestWeaponObject = GetClosestWeaponObject();
                //Clones the weapon he has found
                tempWeapon = closestWeaponObject.weaponReference.Clone();
                
                //If we dont have a weapon
                if (currentWeapon == null)
                {
                    //Update it to null
                    //UpdateWeapon(closestWeaponObject.weaponReference.indexPosition, null);
                }
                else
                {
                    //Update it to our current weapon
                    //UpdateWeapon(closestWeaponObject.weaponReference.indexPosition, currentWeapon);
                }
                
                //Equips the cloned weapon
                EquipWeapon(tempWeapon);
                    
                
            
            }
        }
    }
    


    void SwitchWeapon()
    {
        //Checks for button press
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            //Checks if there is any weapon around
            if(weaponsInRange.Count > 0)
            {
                //Get the closest weapon
                float shortestDistance = Mathf.Infinity;
                float currentDistance;
                WeaponObject closestWeapon = null;
                for(int i = 0; i < weaponsInRange.Count; i++)
                {
                    currentDistance = Vector3.Distance(weaponsInRange[i].transform.position, transform.position);
                    if ( currentDistance < shortestDistance)
                    {
                        shortestDistance = currentDistance;
                        closestWeapon = weaponsInRange[i];
                    }
                }

                Weapon ourWeapon = new Weapon(currentWeapon.spriteType, currentWeapon.weaponName);
                Weapon theirWeapon = new Weapon(closestWeapon.weaponReference.spriteType, closestWeapon.weaponReference.weaponName);

                //Get the weapon we are supposed to hold;
                //if (closestWeapon)
                //tempWeapon = new Weapon(closestWeapon.weaponReference.spriteType, closestWeapon.weaponReference.weaponName);



                //If we dont have a weapon
                if (currentWeapon == null)
                {
                    //Update it to null
                    UpdateWeapon(closestWeapon.weaponReference.indexPosition, null);
                }
                else
                {
                    //Update it to our current weapon
                    UpdateWeapon(closestWeapon.weaponReference.indexPosition, ourWeapon);   
                }

                //equip the weapon
                EquipWeapon(theirWeapon);
                
            }
            //UpdateWeapon(new Vector3(0, 2, 0), new Weapon(15, "anotherweapon", new Vector3(0, 2, 0)));
        }
    }

    public void UpdateWeapon(Vector3 targetPosition, Weapon targetWeapon)
    {
        OnUpdateWeapon(targetPosition, targetWeapon);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
