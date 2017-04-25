﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : Manager
{
    //Sprites
    static public List<Sprite> weaponSprites;
    //Setup
    Weapon[,,] weaponArray;

    //Runtime generation
    GameObject weaponPrefab;
    Weapon currentSpawnWeapon, currentDeSpawnWeapon;
    GameObject currentSpawnWeaponGameObject, currentDeSpawnWeaponGameObject;
    WeaponObject currentSpawnWeaponObject, currentDeSpawnWeaponObject;


    //GameObject CurrentWeapon;

    //Enablers/Disablers

    private void OnEnable()
    {
        PlayerWeaponController.OnUpdateWeapon += UpdateWeapon;
    }

    private void OnDisable()
    {
        PlayerWeaponController.OnUpdateWeapon -= UpdateWeapon;
    }

    // Use this for initialization
    void Start()
    {
        //Loads assets from resources
        weaponPrefab = Resources.Load("Prefabs/Weapon") as GameObject;

        //Initializes arrays and variables
        weaponArray = new Weapon[WorldController.worldsize, WorldController.worldsize, WorldController.numberOfLevels];

        weaponArray[0, 2, 0] = new Weapon(4, "weapon1", new Vector3(0, 2, 0));
        weaponArray[0, 8, 0] = new Weapon(18, "weapon2", new Vector3(0, 8, 0));
        weaponArray[0, 9, 0] = new Weapon(32, "weapon3", new Vector3(0, 12, 0));
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Makes sprites static
    protected override void GenerateAllSprites()
    {
        base.GenerateAllSprites();

        //Assigns sprites
        weaponSprites = new List<Sprite>();
        weaponSprites.AddRange(allSprites);
    }

    public override void SpawnObject(int targetX, int targetY, int targetZ)
    {
        base.SpawnObject(targetX, targetY, targetZ);

        //Grabs the reference to the weapon object
        currentSpawnWeapon = weaponArray[targetX, targetY, targetZ];

        //Checks if its not null
        if (currentSpawnWeapon != null)
        {
            //Instantiages the prefab
            currentSpawnWeaponGameObject = Instantiate(weaponPrefab, new Vector3(targetX, targetY, targetZ), Quaternion.identity);

            //Assigns references
            currentSpawnWeaponObject = currentSpawnWeaponGameObject.GetComponent<WeaponObject>();
            currentSpawnWeaponObject.weaponReference = currentSpawnWeapon;
            currentSpawnWeaponObject.UpdateSprite(allSprites[currentSpawnWeapon.spriteType]);
            currentSpawnWeaponObject.UpdateName(currentSpawnWeapon.weaponName);

            currentSpawnWeapon.associatedWeapon = currentSpawnWeaponObject;

        }
    }

    public override void DeSpawnObject(int targetX, int targetY, int targetZ)
    {
        base.DeSpawnObject(targetX, targetY, targetZ);

        currentDeSpawnWeapon = weaponArray[targetX, targetY, targetZ];

        //Checks if its not null
        if (currentDeSpawnWeapon != null)
        {
            //Grabs the reference to the tile object
            currentDeSpawnWeapon = weaponArray[targetX, targetY, targetZ];

            currentDeSpawnWeaponGameObject = currentDeSpawnWeapon.associatedWeapon.gameObject;

            Destroy(currentDeSpawnWeaponGameObject);
        }

    }

    void UpdateWeapon(Vector3 targetPosition, Weapon targetWeapon)
    {
        if (targetWeapon != null)
        {
            if (weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z] != null)
            {
                Weapon newWeapon = targetWeapon.Clone();
                
                newWeapon.indexPosition = targetPosition;
            
                //Passes the GameObject to the next place
                newWeapon.associatedWeapon = weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z].associatedWeapon;
                
                //updates the associated GameObject values
                newWeapon.associatedWeapon.UpdateSprite(allSprites[targetWeapon.spriteType]);
                newWeapon.associatedWeapon.UpdateName(targetWeapon.weaponName);
                newWeapon.associatedWeapon.UpdateWeaponReference(targetWeapon);
                
                //Updates the weapon array
                weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z] = newWeapon;

                
            }
            else
            {
                Debug.LogError("The target weapon in the weapon array is null");
            }
        }
        else
        {
            Destroy(weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z].associatedWeapon);
            weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z] = null;
        }
    }
}
