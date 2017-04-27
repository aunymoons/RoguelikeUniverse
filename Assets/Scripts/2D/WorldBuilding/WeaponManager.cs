using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WeaponManager : Manager
{
    //Sprites
    static public List<Sprite> weaponSprites;
    //Setup
    Weapon[,,] weaponArray;
    //Weapons
    static public List<Weapon> weaponList;


    //Runtime generation
    GameObject weaponPrefab;
    Weapon currentSpawnWeapon, currentDeSpawnWeapon;
    GameObject currentSpawnWeaponGameObject, currentDeSpawnWeaponGameObject;
    WeaponObject currentSpawnWeaponObject, currentDeSpawnWeaponObject;


    //GameObject CurrentWeapon;

    //Enablers/Disablers

    private void OnEnable()
    {
        //PlayerWeaponController.OnUpdateWeapon += RpcUpdateWeapon;
    }

    private void OnDisable()
    {
        //PlayerWeaponController.OnUpdateWeapon -= RpcUpdateWeapon;
    }

    // Use this for initialization
    void Start()
    {
        //Loads assets from resources
        weaponPrefab = Resources.Load("Prefabs/Weapon") as GameObject;

        weaponList = new List<Weapon>();
        
        //Later turn into loadweapons();
        for(int i = 0; i < 5; i++)
        {
            weaponList.Add(new Weapon((byte)i, (byte)(i * 2), "w" + i));
        }

        //Initializes arrays and variables
        weaponArray = new Weapon[WorldController.worldsize, WorldController.worldsize, WorldController.numberOfLevels];

        weaponArray[0, 2, 0] = weaponList[1].Clone(new Vector3(0, 2, 0));
        weaponArray[0, 4, 0] = weaponList[25].Clone(new Vector3(0, 4, 0));
        weaponArray[0, 6, 0] = weaponList[35].Clone(new Vector3(0, 6, 0));
        
    }

    void LoadWeapons()
    {

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

    [ClientRpc]
    public void RpcUpdateWeapon(Vector3 targetPosition, byte targetWeaponID, bool weaponExists)
    {
        Weapon targetWeapon = weaponList[targetWeaponID].Clone(targetPosition);
        if (weaponExists && targetWeapon != null)
        {
            if (weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z] != null)
            {
                //Weapon newWeapon = targetWeapon.Clone();

                //newWeapon.indexPosition = targetPosition;

                //Passes the GameObject to the next place
                targetWeapon.associatedWeapon = weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z].associatedWeapon;

                //updates the associated GameObject values
                targetWeapon.associatedWeapon.UpdateSprite(allSprites[targetWeapon.spriteType]);
                targetWeapon.associatedWeapon.UpdateName(targetWeapon.weaponName);
                targetWeapon.associatedWeapon.UpdateWeaponReference(targetWeapon);
                
                //Updates the weapon array
                weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z] = targetWeapon;

                Debug.Log(weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z].weaponName);


            }
            else
            {
                Debug.LogError("The target weapon in the weapon array is null");
            }
        }
        else
        {
            Debug.Log("destroyed object");
            DeSpawnObject((int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z);
            weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z] = null;
        }
    }
}
