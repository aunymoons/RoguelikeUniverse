using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using ProceduralToolkit.Examples;

public class TileManager : Manager
{
    //Setup
    Tile[,,] tileArray;

    //Runtime generation
    GameObject tilePrefab;
    GameObject currentSpawnTileGameObject, currentDespawnTileGameObject;
    Tile currentSpawnTile, currentDespawnTile;

    //Pooling
    List<GameObject> enabledTiles = new List<GameObject>();
    List<GameObject> disabledTiles = new List<GameObject>();

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
        tilePrefab = Resources.Load("Prefabs/Tile") as GameObject;

        //Initializes arrays and variables
        tileArray = new Tile[WorldController.worldsize, WorldController.worldsize, WorldController.numberOfLevels];

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
    }

    public override void SpawnObject(int targetX, int targetY, int targetZ)
    {
        base.SpawnObject(targetX, targetY, targetZ);

        //Grabs the reference to the tile object
        currentSpawnTile = tileArray[targetX, targetY, targetZ];

        //Gets a tile from the pool
        currentSpawnTileGameObject = Lean.LeanPool.Spawn(tilePrefab, new Vector3(targetX, targetY, targetZ), Quaternion.identity);

        //Assigns the properties to the tile

        //Associated tile
        currentSpawnTile.associatedTile = currentSpawnTileGameObject;

        //Collider
        if (currentSpawnTile.propertyType == Tile.Property.walkable)
        {
            currentSpawnTileGameObject.GetComponent<BoxCollider2D>().enabled = false; //This can be encapsulated as a function inside the tile class
        }
        else
        {
            currentSpawnTileGameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        //Sprite type
        currentSpawnTileGameObject.GetComponent<SpriteRenderer>().sprite = allSprites[currentSpawnTile.spriteType];

    }

    public override void DeSpawnObject(int targetX, int targetY, int targetZ)
    {
        base.DeSpawnObject(targetX, targetY, targetZ);

        //Grabs the reference to the tile object
        currentDespawnTile = tileArray[targetX, targetY, targetZ];

        currentDespawnTileGameObject = currentDespawnTile.associatedTile;

        Lean.LeanPool.Despawn(currentDespawnTileGameObject);
    }


    IEnumerator GenerateCelAuto()
    {
        celAuto = new CellularAutomaton(WorldController.worldsize, WorldController.worldsize, Ruleset.majority, 0.5f, true);
        generate = true;
        //yield return new WaitForSeconds(1f);
        generate = false;
        StartCoroutine(GenerateWorld());
        //yield return new WaitForSeconds(1f);
        yield return null;
    }
    
    IEnumerator GenerateWorld()
    {

        for (int x = 0; x < WorldController.worldsize; x++)
        {
            for (int y = 0; y < WorldController.worldsize; y++)
            {
                if (celAuto.cells[x, y] == CellState.Alive)
                {
                    tileArray[x, y, 0] = new Tile(15, Tile.Property.walkable, new Vector3(x,y,0)); ;
                }
                else
                {
                    tileArray[x, y, 0] = tileArray[x, y, 0] = new Tile(16, Tile.Property.solid, new Vector3(x, y, 0)); ;
                }
            }
            //yield return delay;
        }
        yield return null;
    }


    /*
void GenerateVisibletiles()
{
    for (int x = -poolRadius; x <= poolRadius; x++)
    {
        for (int y = -poolRadius; y <= poolRadius; y++)
        {

            //If exist inside world compared to player position
            if ((playerPosition.x + x) >= 0 && (playerPosition.x + x) < WorldController.worldsize && (playerPosition.y + y) >= 0 && (playerPosition.y + y) < WorldController.worldsize)
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

                    }
                }

            }
        }
    }

    //Debug.Log(enabledTiles.Count);
}
*/

}
