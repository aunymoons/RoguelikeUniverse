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
    Vector2 lastPlayerPosition = new Vector2();

    //Pooling
    //int poolAmount = 800;
    int poolRadius = 25;
    //int poolThreshHold;
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
        spriteMaterial = Resources.Load("Materials/SpriteMaterial") as Material;
        tilePrefab = Resources.Load("Prefabs/Tile") as GameObject;

        //Initializes arrays and variables
        tileArray = new Tile[worldsize, worldsize, numberOfLevels];

        //poolThreshHold = poolRadius / 3;

        //Generates the tile sprites
        GenerateTileSprites();

        //Instantiate pooled sprites;
        //StartCoroutine(InstantiatePooledTiles());

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
            else if (updateTiles)
            {
            if (targetPlayer)
            {
                playerPosition.x = Mathf.FloorToInt(targetPlayer.transform.position.x);
                playerPosition.y = Mathf.FloorToInt(targetPlayer.transform.position.y);

                //Updates only if player has switched tiles
                if (lastPlayerPosition != playerPosition)
                {
                    lastPlayerPosition = playerPosition;
                    GenerateVisibletiles();
                }

            }
        }
    }

    //Instantiate 
    /*
    IEnumerator InstantiatePooledTiles()
    {
        for (int i = 0; i <= poolAmount; i++)
        {
            disabledTiles.Add(Instantiate(tilePrefab));
        }
        yield return delay;
    }
    */

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
        //playerPosition.x = Mathf.FloorToInt(targetPlayer.transform.position.x);
        //playerPosition.y = Mathf.FloorToInt(targetPlayer.transform.position.y);
        /*
        if (enabledTiles.Count > 0)
        {
            GameObject[] enabledTilesCopy = new GameObject[enabledTiles.Count];

            enabledTiles.CopyTo(enabledTilesCopy);

            for (int i = 0; i < enabledTilesCopy.Length; i++)
            //for (int i = enabledTilesCount - 1; i >= 0; i--)
            //foreach (GameObject enabledTile in enabledTiles.ToArray())
            {
                //if out of range
                
                if (
                    
                    Mathf.Abs(enabledTilesCopy[i].transform.position.x) > Mathf.Abs(playerPosition.x + poolRadius)
                    ||
                    Mathf.Abs(enabledTilesCopy[i].transform.position.y) > Mathf.Abs(playerPosition.y + poolRadius)
                    )
                {
                
                    currentTile = tileArray[Mathf.FloorToInt(enabledTilesCopy[i].transform.position.x), Mathf.FloorToInt(enabledTilesCopy[i].transform.position.y), 0];

                    if (currentTile.visible)
                    {
                        currentTile.visible = false;

                        //currentTileGameObject = enabledTile;

                        //Moves it to appropiate list
                        enabledTiles.Remove(enabledTilesCopy[i]);
                        disabledTiles.Add(enabledTilesCopy[i]);

                        //Disables gameObject
                        //currentTileGameObject.SetActive(false);

                    }
                }
            }
        }

        Debug.Log(disabledTiles.Count);
        */

        for (int x = -poolRadius; x <= poolRadius; x++)
        {
            for (int y = -poolRadius; y <= poolRadius; y++)
            {

                //If exist inside world compared to player position
                if ((playerPosition.x + x) >= 0 && (playerPosition.x + x) < worldsize && (playerPosition.y + y) >= 0 && (playerPosition.y + y) < worldsize)
                {
                    //Grabs the reference to the tile object
                    currentTile = tileArray[((int)playerPosition.x + (int)x), ((int)playerPosition.y + (int)y), 0];

                    //If is inside range
                    if (Mathf.Abs(x) < poolRadius - (poolRadius / 3) && Mathf.Abs(y) < poolRadius - (poolRadius / 3))
                    {
                        //if the current tile is not flagged as visible
                        if (!currentTile.visible)
                        {
                            //Marks it as visible
                            currentTile.visible = true;

                            //Gets a tile from the pool
                            currentTileGameObject = Lean.LeanPool.Spawn(tilePrefab, new Vector3(((int)playerPosition.x + (int)x), ((int)playerPosition.y + (int)y), 0), Quaternion.identity);//disabledTiles[0];

                            //Assigns the tile to the appropiate position
                            //currentTileGameObject.transform.position = new Vector3(((int)playerPosition.x + (int)x), ((int)playerPosition.y + (int)y), 0);

                            //Assigns the properties to the tile

                            //Associated tile
                            currentTile.associatedTile = currentTileGameObject;

                            //Collider
                            if (currentTile.propertyType == Tile.Property.walkable)
                            {
                                currentTileGameObject.GetComponent<BoxCollider2D>().enabled = false;
                            }
                            else
                            {
                                currentTileGameObject.GetComponent<BoxCollider2D>().enabled = true;
                            }
                            //Sprite type
                            currentTileGameObject.GetComponent<SpriteRenderer>().sprite = allSprites[currentTile.spriteType];

                            //Moves it to appropiate list
                            //disabledTiles.Remove(currentTileGameObject);
                            //enabledTiles.Add(currentTileGameObject);

                            //Enables gameObject
                            //currentTileGameObject.SetActive(true);

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

                            Lean.LeanPool.Despawn(currentTileGameObject);

                            //Moves it to appropiate list
                            //enabledTiles.Remove(currentTileGameObject);
                            //disabledTiles.Add(currentTileGameObject);

                            //Disables gameObject
                            //currentTileGameObject.SetActive(false);
                        }
                    }

                }
            }
        }

        //Debug.Log(enabledTiles.Count);

    }

    IEnumerator GenerateCelAuto()
    {
        celAuto = new CellularAutomaton(worldsize, worldsize, Ruleset.majority, 0.5f, true);
        generate = true;
        yield return new WaitForSeconds(1f);
        generate = false;
        StartCoroutine(GenerateWorld());
        yield return new WaitForSeconds(1f);
        GenerateVisibletiles();
        updateTiles = true;
    }


    IEnumerator GenerateWorld()
    {

        for (int x = 0; x < worldsize; x++)
        {
            for (int y = 0; y < worldsize; y++)
            {
                if (celAuto.cells[x, y] == CellState.Alive)
                {
                    tileArray[x, y, 0] = new Tile(15, Tile.Property.walkable); ;
                }
                else
                {
                    tileArray[x, y, 0] = tileArray[x, y, 0] = new Tile(16, Tile.Property.solid); ;
                }
            }
            yield return delay;
        }

    }

}
