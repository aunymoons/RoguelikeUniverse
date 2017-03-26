using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    public byte spriteType;
    public enum property { walkable = 0, solid = 1, destructible = 2,}
    public property propertyType;
    public bool visible;
    public GameObject associatedTile;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
