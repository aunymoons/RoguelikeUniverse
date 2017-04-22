using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public byte spriteType;
    public enum WeaponType { plasma = 0, nitrogen = 1, electricity = 2, acid = 3, oxygen = 4 }
    public WeaponType weaponType;

    int attackPower, energyCost;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
