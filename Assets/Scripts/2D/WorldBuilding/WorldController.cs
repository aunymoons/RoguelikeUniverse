using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{

    //Setup
    public static int worldsize = 256, numberOfLevels = 1;
    bool[,,] objectArray;

    //References
    public TileManager tileManager;
    public WeaponManager weaponManager;

    //Generation
    Transform targetPlayer;
    Vector3 playerPosition = new Vector3();
    Vector3 lastPlayerPosition = new Vector3();

    //Pooling
    int poolRadius = 25;

    // Use this for initialization
    void Start()
    {
        objectArray = new bool[worldsize, worldsize, numberOfLevels];
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPlayer)
        {
            playerPosition.x = Mathf.FloorToInt(targetPlayer.transform.position.x);
            playerPosition.y = Mathf.FloorToInt(targetPlayer.transform.position.y);

            //Updates only if player has switched tiles
            if (lastPlayerPosition != playerPosition)
            {
                lastPlayerPosition = playerPosition;
                GenerateVisibleWorld();
            }

        }
    }

    public void SetTarget(Transform target)
    {
        targetPlayer = target;
    }

    void GenerateVisibleWorld()
    {
        for (int x = -poolRadius; x <= poolRadius; x++)
        {
            for (int y = -poolRadius; y <= poolRadius; y++)
            {

                //If exist inside world compared to player position
                if ((playerPosition.x + x) >= 0 && (playerPosition.x + x) < worldsize && (playerPosition.y + y) >= 0 && (playerPosition.y + y) < worldsize)
                {
                    //If is inside range
                    if (Mathf.Abs(x) < poolRadius - (poolRadius / 3) && Mathf.Abs(y) < poolRadius - (poolRadius / 3))
                    {
                        if (!objectArray[(int)playerPosition.x + x, (int)playerPosition.y + y, 0])
                        {
                            objectArray[(int)playerPosition.x + x, (int)playerPosition.y + y, 0] = true;

                            tileManager.SpawnObject((int)playerPosition.x + x, (int)playerPosition.y + y, 0);
                            weaponManager.SpawnObject((int)playerPosition.x + x, (int)playerPosition.y + y, 0);
                        }
                    }
                    //If its outside range
                    else
                    {
                        if (objectArray[(int)playerPosition.x + x, (int)playerPosition.y + y, 0])
                        {
                            objectArray[(int)playerPosition.x + x, (int)playerPosition.y + y, 0] = false;

                            tileManager.DeSpawnObject((int)playerPosition.x + x, (int)playerPosition.y + y, 0);
                            weaponManager.DeSpawnObject((int)playerPosition.x + x, (int)playerPosition.y + y, 0);
                        }
                    }

                }
            }
        }

        //Debug.Log(enabledTiles.Count);
    }
}
