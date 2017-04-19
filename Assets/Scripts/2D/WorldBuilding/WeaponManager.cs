using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Manager
{

    //Setup
    Weapon[,,] weaponArray;

    //Runtime generation
    GameObject weaponPrefab;

    GameObject CurrentWeapon;

    // Use this for initialization
    void Start()
    {
        //Loads assets from resources
        weaponPrefab = Resources.Load("Prefabs/Weapon") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SpawnObject(int targetX, int targetY, int targetZ)
    {
        base.SpawnObject(targetX, targetY, targetZ);
        if (Random.Range(0, 2) == 1)
        {
            CurrentWeapon = Instantiate(weaponPrefab, new Vector3(targetX, targetY, targetZ), Quaternion.identity);

            CurrentWeapon.GetComponent<SpriteRenderer>().sprite = allSprites[(int)Random.Range(0, 256)];

        }
    }
}
