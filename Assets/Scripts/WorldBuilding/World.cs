﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Threading;

public class World : MonoBehaviour
{

    //Dictionary of chunks
    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public GameObject chunkPrefab;

    public Vector3 worldSize;

    void Awake()
    {
        EZThread.ExecuteInBackground(PrecalculateRotations);
        
    }

    // Use this for initialization
    void Start()
    {
        GenerateWorld();
        /*
        for (int x = -((int)worldSize.x / 2); x < ((int)worldSize.x / 2); x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                for (int z = -((int)worldSize.z / 2); z < ((int)worldSize.z / 2); z++)
                {
                    CreateChunk(x * (Chunk.chunkSize), y * (Chunk.chunkSize), z * (Chunk.chunkSize));
                }
            }
        }
        */
    }

    void GenerateWorld()
    {
        for (int x = -((int)worldSize.x / 2); x < ((int)worldSize.x / 2); x++)
        {
            for (int y = 0; y < (int)worldSize.y; y++)
            {
                for (int z = -((int)worldSize.z / 2); z < ((int)worldSize.z / 2); z++)
                {
                    CreateChunk(x * (Chunk.chunkSize), y * (Chunk.chunkSize), z * (Chunk.chunkSize));
                }
            }

        }
    }

    
    

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Creates a chunk on the given coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void CreateChunk(int x, int y, int z)
    {
        //the coordinates of this chunk in the world
        WorldPos worldPos = new WorldPos(x, y, z);

        //Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunkObject = Instantiate(
                        chunkPrefab, new Vector3(worldPos.x, worldPos.y, worldPos.z),
                        Quaternion.Euler(Vector3.zero),
                        transform
                    ) as GameObject;

        //Get the object's chunk component
        Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        //Assign its values
        newChunk.pos = worldPos;
        newChunk.world = this;

        //Add it to the chunks dictionary with the position as the key
        chunks.Add(worldPos, newChunk);

        //Add the following:
        for (int xi = 0; xi < Chunk.chunkSize; xi++)
        {
            for (int yi = 0; yi < Chunk.chunkSize; yi++)
            {
                for (int zi = 0; zi < Chunk.chunkSize; zi++)
                {
                    if (yi <= Mathf.PerlinNoise(xi * 10, zi * 10))
                    {
                        SetBlock(x + xi, y + yi, z + zi, new Block(new Vector3(0, 0, 0), Color.white));
                    }
                    else
                    {
                        SetBlock(x + xi, y + yi, z + zi, new BlockEmpty(new Vector3(0, 0, 0), Color.white));


                    //PERFORMANCE TESTING RANDOM
                        /*
                        int rand = Random.Range(0, 3);
                        if (rand == 1)
                        {
                            int thisx = Random.Range(0, 4) * 90;
                            int thisy = Random.Range(0, 4) * 90;
                            int thisz = Random.Range(0, 4) * 90;
                            SetBlock(x + xi, y + yi, z + zi, new BlockPyramid(new Vector3(thisx, thisy, thisz), Random.ColorHSV()));
                        }
                        else if (rand == 2)
                        {
                            int thisx = Random.Range(0, 4) * 90;
                            int thisy = Random.Range(0, 4) * 90;
                            int thisz = Random.Range(0, 4) * 90;
                            SetBlock(x + xi, y + yi, z + zi, new Block(new Vector3(thisx, thisy, thisz), Random.ColorHSV()));
                        }
                        else
                        {
                            SetBlock(x + xi, y + yi, z + zi, new BlockEmpty(new Vector3(0, 0, 0), Color.white));
                        }
                        */

                    }
                }
            }
        }

        /*
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        */
    }

    public void DestroyChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }

    //GET AND SET CHUNKS AND CUBES

    public Chunk GetChunk(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;
        Chunk containerChunk = null;
        chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }
    public Block GetBlock(int x, int y, int z)
    {
        Chunk containerChunk = GetChunk(x, y, z);
        if (containerChunk != null)
        {
            Block block = containerChunk.GetBlock(
                x - containerChunk.pos.x,
                y - containerChunk.pos.y,
                z - containerChunk.pos.z);

            return block;
        }
        else
        {
            return new BlockEmpty(new Vector3(0, 0, 0), Color.white);
        }

    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        Chunk chunk = GetChunk(x, y, z);

        if (chunk != null)
        {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);

            chunk.update = true;


            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));

            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));

            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));

            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));

            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));

            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));

        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.update = true;
        }
    }


    //ROTATION CALCULATIONS

    public static int[,,,] precalculatedRotations;

    public void PrecalculateRotations()
    {
        precalculatedRotations = new int[4, 4, 4, 6];

        for (int zi = 0; zi < 4; zi++)
        {
            for (int xi = 0; xi < 4; xi++)
            {
                for (int yi = 0; yi < 4; yi++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        precalculatedRotations[zi, xi, yi, i] = CalculateDirectionBasedOnRotation(xi * 90, yi * 90, zi * 90, i);
                    }
                }
            }
        }

    }

    protected int CalculateDirectionBasedOnRotation(int x, int y, int z, int oInt)
    {
        //int startDirectionNumber;

        //startDirectionNumber = GetNumberByDirection(startDirection);

        int[] sides = new int[6];

        //assigns sides accordingly
        for (int i = 0; i < sides.Length; i++)
        {
            sides[i] = i;
        }

        //Z Rotation
        if (z != 0)
        {
            int[] sidesZ = new int[6];
            for (int i = 0; i < sides.Length; i++)
            {
                sidesZ[i] = sides[RotateSides(0, 0, z, i)];
            }
            for (int i = 0; i < sides.Length; i++)
            {
                sides[i] = sidesZ[i];
            }
        }

        //X Rotation
        if (x != 0)
        {
            int[] sidesX = new int[6];
            for (int i = 0; i < sides.Length; i++)
            {
                sidesX[i] = sides[RotateSides(x, 0, 0, i)];
            }
            for (int i = 0; i < sides.Length; i++)
            {
                sides[i] = sidesX[i];
            }
        }

        //Y Rotation
        if (y != 0)
        {
            int[] sidesY = new int[6];
            for (int i = 0; i < sides.Length; i++)
            {
                sidesY[i] = sides[RotateSides(0, y, 0, i)];
            }
            for (int i = 0; i < sides.Length; i++)
            {
                sides[i] = sidesY[i];
            }
        }

        //return GetDirectionByNumber(sides[startDirectionNumber]);

        return sides[oInt];

    }

    //Returns index of rotation
    protected int RotateSides(int x, int y, int z, int oDir)
    {
        if (z != 0)
        {
            if (z == 0)
            {
            }
            if (z == 90)
            {
                switch (oDir)
                {
                    case 2:
                        return 5; //Down

                    case 3:
                        return 4; //Up

                    case 4:
                        return 2; //East

                    case 5:
                        return 3; //West

                    case 0:
                        return 0; //North

                    case 1:
                        return 1; //South



                }
            }
            if (z == 180)
            {
                switch (oDir)
                {
                    case 2:
                        return 3; //West

                    case 3:
                        return 2; //East

                    case 4:
                        return 5; //Down

                    case 5:
                        return 4; //Up

                    case 0:
                        return 0; //North

                    case 1:
                        return 1; //South



                }
            }
            if (z == 270)
            {
                switch (oDir)
                {
                    case 2:
                        return 4; //Up

                    case 3:
                        return 5; //Down

                    case 4:
                        return 3; //West

                    case 5:
                        return 2; //East

                    case 0:
                        return 0; //North

                    case 1:
                        return 1; //South



                }
            }
        }

        if (x != 0)
        {
            if (x == 0)
            {
            }
            if (x == 90)
            {
                switch (oDir)
                {
                    case 0:
                        return 4; //Up

                    case 1:
                        return 5; //Down

                    case 4:
                        return 1; //South

                    case 5:
                        return 0; //North

                    case 2:
                        return 2; //East

                    case 3:
                        return 3; //West



                }
            }
            if (x == 180)
            {
                switch (oDir)
                {
                    case 0:
                        return 1; //South

                    case 1:
                        return 0; //North

                    case 4:
                        return 5; //Down

                    case 5:
                        return 4; //Up

                    case 2:
                        return 2; //East

                    case 3:
                        return 3; //West



                }
            }
            if (x == 270)
            {
                switch (oDir)
                {
                    case 0:
                        return 5; //Down

                    case 1:
                        return 4; //Up

                    case 4:
                        return 0; //North

                    case 5:
                        return 1; //South

                    case 2:
                        return 2; //East

                    case 3:
                        return 3; //West



                }
            }

        }

        if (y != 0)
        {

            if (y == 0)
            {
            }
            if (y == 90)
            {
                switch (oDir)
                {
                    case 0:
                        return 3; //West

                    case 1:
                        return 2; //East

                    case 2:
                        return 0; //North

                    case 3:
                        return 1; //South

                    case 4:
                        return 4; //Up

                    case 5:
                        return 5; //Down



                }
            }
            if (y == 180)
            {
                switch (oDir)
                {
                    case 0:
                        return 1; //South

                    case 1:
                        return 0; //North

                    case 2:
                        return 3; //West

                    case 3:
                        return 2; //East

                    case 4:
                        return 4; //Up

                    case 5:
                        return 5; //Down

                }
            }
            if (y == 270)
            {
                switch (oDir)
                {
                    case 0:
                        return 2; //East

                    case 1:
                        return 3; //West

                    case 2:
                        return 1; //South

                    case 3:
                        return 0; //North

                    case 4:
                        return 4; //Up

                    case 5:
                        return 5; //Down

                }
            }
        }


        return oDir;
    }




}