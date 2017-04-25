using System.Collections;
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
                //Passes the GameObject to the next place
                targetWeapon.associatedWeapon = weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z].associatedWeapon;

                //Updates the weapon array
                weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z] = targetWeapon;

                //updates the associated GameObject values
                weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z].associatedWeapon.UpdateSprite(allSprites[weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z].spriteType]);
                weaponArray[(int)targetPosition.x, (int)targetPosition.y, (int)targetPosition.z].associatedWeapon.UpdateName(targetWeapon.weaponName);
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
