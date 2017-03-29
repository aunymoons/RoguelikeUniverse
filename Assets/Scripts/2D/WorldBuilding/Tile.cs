using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    public byte spriteType;
    public enum Property { walkable = 0, solid = 1, destructible = 2}
    public Property propertyType;
    public bool visible;
    public GameObject associatedTile;


    //Constructors
    public Tile()
    {
        spriteType = 0;
        propertyType = Property.walkable;
        visible = false;
    }

    public Tile(byte spriteNum, Property property, bool visibility = false, GameObject tileGameObject = null)
    {
        spriteType = spriteNum;
        propertyType = property;
        visible = visibility;
        associatedTile = tileGameObject;
    }
}
