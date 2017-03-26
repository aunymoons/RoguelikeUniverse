using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using ProceduralToolkit.Examples;

public class TileManager : MonoBehaviour
{
    //Setup
    int tileResolution = 32, worldsize = 256, numberOfLevels = 1;
    public string spriteSheetName;
    Texture2D spriteSheet;
    Tile[,,] tileArray;

    //Runtime world generation
    GameObject tilePrefab;
    GameObject currentTileGameObject;
    Tile currentTile;
    public Transform targetPlayer;
    Vector2 playerPosition = new Vector2();

    //Pooling
    int poolAmount = 1000;
    int poolRadius = 50;
    int poolThreshHold;
    List<GameObject> enabledTiles = new List<GameObject>();
    List<GameObject> disabledTiles = new List<GameObject>();
   

    //Sprites
    List<Sprite> allSprites = new List<Sprite>();
    Material spriteMaterial;
    Sprite newSprite;

    //Procedural Generation
    CellularAutomaton celAuto;
    bool generate;
    bool updateTiles;

    //Coroutines
    WaitForEndOfFrame delay = new WaitForEndOfFrame();

    // Use this for initialization
    void Start()
    {
        //Loads assets from resources
        spriteSheet = Resources.Load(spriteSheetName) as Texture2D;
        spriteMaterial = Resources.Load("spriteMaterial") as Material;
        tilePrefab = Resources.Load("Tile") as GameObject;

        //Initializes arrays and variables
        tileArray = new Tile[worldsize, worldsize, numberOfLevels];

        poolThreshHold = poolRadius / 3;

        //Generates the tile sprites
        GenerateTileSprites();

        //Instantiate pooled sprites;
        StartCoroutine(InstantiatePooledTiles());
        
        //Celautomation generation
        StartCoroutine(GenerateCelAuto());
    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
        {
            celAuto.Simulate();
        }
        else if(updateTiles)
        {
            GenerateVisibletiles();
        }
    }

    //Instantiate 
    IEnumerator InstantiatePooledTiles()
    {
        yield return delay;
        for (int i = 0; i <= poolAmount; i++)
        {
            disabledTiles.Add(Instantiate(tilePrefab));
        }
    }

    /// <summary>
    /// Generates all sprites for tiles based on the loaded spritesheet and the tile resolution
    /// </summary>
    void GenerateTileSprites()
    {
        for (int x = 0; x < spriteSheet.width / tileResolution; x++)
        {
            for (int y = 0; y < spriteSheet.width / tileResolution; y++)
            {
                newSprite = Sprite.Create(spriteSheet, new Rect(x * tileResolution, y * tileResolution, tileResolution, tileResolution), new Vector2(0.5f, 0.5f), tileResolution);
                newSprite.name = "Tile" + x + "_" + y;
                allSprites.Add(newSprite);
            }
        }
    }

    void GenerateVisibletiles()
    {
        playerPosition.x = Mathf.FloorToInt(targetPlayer.transform.position.x);
        playerPosition.y = Mathf.FloorToInt(targetPlayer.transform.position.y);


        for (int x = -poolRadius; x <= poolRadius; x++)
        {
            for (int y = -poolRadius; y <= poolRadius; y++)
            {

                //If exist inside world compared to player position
                if((playerPosition.x + x) >= 0 && (playerPosition.x + x) < worldsize && (playerPosition.y + y) >= 0 && (playerPosition.y + y) < worldsize)
                {
                    //Grabs the reference to the tile object
                    currentTile = tileArray[((int)playerPosition.x + (int)x), ((int)playerPosition.y + (int)y), 0];
                    
                    //If is inside range
                    if(Mathf.Abs(x) < poolThreshHold && Mathf.Abs(y) < poolThreshHold)
                    {
                        //if the current tile is not flagged as visible
                        if (!currentTile.visible)
                        {
                            //Marks it as visible
                            currentTile.visible = true;
                        
                            //Gets a tile from the disabled list
                            currentTileGameObject = disabledTiles[0];

                            //Assigns the tile to the appropiate position
                            currentTileGameObject.transform.position = new Vector3(((int)playerPosition.x + (int)x), ((int)playerPosition.y + (int)y), 0);

                            //Assigns the properties to the tile

                            //Associated tile
                            currentTile.associatedTile = currentTileGameObject;

                            //Collider
                            if (currentTile.propertyType == Tile.property.walkable)
                            {
                                currentTileGameObject.GetComponent<BoxCollider2D>().enabled = false;
                            }
                            else{
                                currentTileGameObject.GetComponent<BoxCollider2D>().enabled = true;
                            }
                            //Sprite type
                            currentTileGameObject.GetComponent<SpriteRenderer>().sprite = allSprites[currentTile.spriteType];

                            //Moves it to appropiate list
                            disabledTiles.Remove(currentTileGameObject);
                            enabledTiles.Add(currentTileGameObject);

                            //Enables gameObject
                            currentTileGameObject.SetActive(true);
                        
                        }
                    }
                    
                    //If its outside range
                    else
                    {
                        //if its visible
                        if (currentTile.visible)
                        {
                            //Marks it as visible
                            currentTile.visible = false;

                            currentTileGameObject = currentTile.associatedTile;

                            //Moves it to appropiate list
                            enabledTiles.Remove(currentTileGameObject);
                            disabledTiles.Add(currentTileGameObject);

                            //Disables gameObject
                            currentTileGameObject.SetActive(false);
                        }
                    }
                    
                }
            }
        }
    }

    IEnumerator GenerateCelAuto()
    {
        celAuto = new CellularAutomaton(100 * Chunk.chunkSize, 100 * Chunk.chunkSize, Ruleset.majority, 0.5f, true);
        generate = true;
        yield return new WaitForSeconds(1f);
        generate = false;
        StartCoroutine(GenerateWorld());
        yield return new WaitForSeconds(1f);
        updateTiles = true;
    }


    IEnumerator GenerateWorld()
    {
        Tile tileToAdd;

        for (int x = 0; x < worldsize; x++)
        {
            for (int y = 0; y < worldsize; y++)
            {
                /*
                GameObject n = new GameObject();
                
                SpriteRenderer sr = n.AddComponent<SpriteRenderer>();
                sr.material = mat;
                sr.sortingLayerName = "Level1_below";
                */
                if (celAuto.cells[x, y] == CellState.Alive)
                {
                    tileToAdd = new Tile();

                    tileToAdd.spriteType = 15;

                    tileToAdd.propertyType = Tile.property.walkable;

                    tileArray[x, y, 0] = tileToAdd;
                }
                else
                {
                    tileToAdd = new Tile();

                    tileToAdd.spriteType = 23;

                    tileToAdd.propertyType = Tile.property.solid;

                    tileArray[x, y, 0] = tileToAdd;
                }
                
            }
            yield return delay;
        }
        
    }

}
