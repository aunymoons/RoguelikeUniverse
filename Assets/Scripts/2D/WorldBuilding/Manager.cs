using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    //Setup
    protected int squareResolution = 32;
    public string spriteToLoadName;
    protected Texture2D spriteSheet;

    //Sprites
    protected List<Sprite> allSprites = new List<Sprite>();
    protected Material spriteMaterial;
    protected Sprite newSprite;


    // Use this for initialization
    void Awake () {

        //Loads assets from resources
        spriteSheet = Resources.Load(spriteToLoadName) as Texture2D;
        spriteMaterial = Resources.Load("Materials/SpriteMaterial") as Material;

        //Generates the tile sprites
        GenerateAllSprites();

    }

    // Update is called once per frame
    void Update () {
		
	}
    
    /// <summary>
    /// Generates all sprites for tiles based on the loaded spritesheet and the tile resolution
    /// </summary>
    protected void GenerateAllSprites()
    {
        for (int x = 0; x < spriteSheet.width / squareResolution; x++)
        {
            for (int y = 0; y < spriteSheet.width / squareResolution; y++)
            {
                newSprite = Sprite.Create(spriteSheet, new Rect(x * squareResolution, y * squareResolution, squareResolution, squareResolution), new Vector2(0.5f, 0.5f), squareResolution, 1, SpriteMeshType.FullRect);
                newSprite.name = "Tile" + x + "_" + y;
                allSprites.Add(newSprite);
            }
        }
    }

    /// <summary>
    /// Override this to make objects appear
    /// </summary>
    public virtual void SpawnObject(int targetX, int targetY, int targetZ)
    {

    }

    /// <summary>
    /// Override this to make objects disappear
    /// </summary>
    public virtual void DeSpawnObject(int targetX, int targetY, int targetZ)
    {

    }

}
