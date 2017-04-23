using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    
    public byte spriteType;
    public enum Property { walkable = 0, solid = 1, destructible = 2}
    Vector3 indexPosition;
    public Property propertyType;
    public bool visible;
    public GameObject associatedTile;


    //Constructors
    public Tile()
    {
        spriteType = 0;
        propertyType = Property.walkable;
        visible = false;
        indexPosition = new Vector3();
    }

    public Tile(byte spriteNum, Property property, Vector3 indexPos = new Vector3(), bool visibility = false,  GameObject tileGameObject = null)
    {
        spriteType = spriteNum;
        propertyType = property;
        visible = visibility;
        associatedTile = tileGameObject;
        indexPosition = indexPos;
    }
}
