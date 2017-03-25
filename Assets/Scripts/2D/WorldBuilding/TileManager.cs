using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using ProceduralToolkit.Examples;

public class TileManager : MonoBehaviour
{

    public Texture2D spriteSheet;

    public List<Sprite> allSprites = new List<Sprite>();

    int numberOfTiles = 16, tileResolution = 32, worldsize = 256;

    bool generate;

    Sprite newSprite;

    CellularAutomaton celAuto;

    public Material mat;

    // Use this for initialization
    void Start()
    {
        GenerateTileSprites();
        StartCoroutine(Test());

        
    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
        {
            celAuto.Simulate();
        }
    }

    void GenerateTileSprites()
    {
        for (int x = 0; x < numberOfTiles; x++)
        {
            for (int y = 0; y < numberOfTiles; y++)
            {
                newSprite = Sprite.Create(spriteSheet, new Rect(x * tileResolution, y * tileResolution, tileResolution, tileResolution), new Vector2(0.5f, 0.5f), tileResolution);
                newSprite.name = "Tile" + x + "_" + y;
                allSprites.Add(newSprite);
            }
        }
    }

    IEnumerator Test()
    {
        celAuto = new CellularAutomaton(100 * Chunk.chunkSize, 100 * Chunk.chunkSize, Ruleset.majority, 0.5f, true);
        generate = true;
        yield return new WaitForSeconds(1f);
        generate = false;
        StartCoroutine(GenerateWorld());

    }

    IEnumerator GenerateWorld()
    {
        WaitForEndOfFrame delay = new WaitForEndOfFrame();

        for (int x = 0; x <= worldsize; x++)
        {
            for (int y = 0; y <= worldsize; y++)
            {

                GameObject n = new GameObject();
                
                SpriteRenderer sr = n.AddComponent<SpriteRenderer>();
                sr.material = mat;
                sr.sortingLayerName = "Level1_below";

                if (celAuto.cells[x, y] == CellState.Alive)
                {
                    sr.sprite = allSprites[24];
                }
                else
                {
                    sr.sprite = allSprites[22];
                    n.AddComponent<BoxCollider2D>();
                }

                n.transform.position = new Vector3(x, y, 0);

               
            }
            yield return delay;
        }
        
    }

}
