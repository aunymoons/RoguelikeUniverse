using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon {
    
    public byte spriteType;
    public Vector3 indexPosition;
    public enum WeaponType { plasma = 0, nitrogen = 1, electricity = 2, acid = 3, water = 4 }
    public WeaponType weaponType;
    public WeaponObject associatedWeapon;
    public string weaponName;

    int attackPower, energyCost;

    //Constructors
    public Weapon()
    {
        spriteType = 0;
        weaponType = WeaponType.plasma;
    }

    public Weapon( byte spriteNum, string targetName, Vector3 indexPos = new Vector3(), WeaponObject weaponNetGameObject = null)
    {
        spriteType = spriteNum;
        associatedWeapon = weaponNetGameObject;
        indexPosition = indexPos;
        weaponName = targetName;
    }
    
    public Weapon Clone()
    {
           return new Weapon(spriteType, weaponName, indexPosition, associatedWeapon);
    }
    
}
