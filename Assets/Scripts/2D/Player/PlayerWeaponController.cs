using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerWeaponController : PlayerComponentController
{

    //Events

    //Events
    public delegate void UpdateWeaponAnswerCallback(Vector3 targetPosition, Weapon targetWeapon);

    public static event UpdateWeaponAnswerCallback OnUpdateWeapon;

    public void UpdateWeapon(Vector3 targetPosition, Weapon targetWeapon)
    {
           OnUpdateWeapon(targetPosition, targetWeapon);
    }
    
    public GameObject weaponHolder, bulletPrefab;
    public Transform bulletSpawnTransform;

    // Use this for initialization
    protected override void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            UpdateWeapon(new Vector3(0,2,0), new Weapon(15, "anotherweapon" , new Vector3(0,2,0)));
        }
        CmdShoot();
        RotationMovementHandler();
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

    void SwitchWeapon()
    {

    }

   

}
