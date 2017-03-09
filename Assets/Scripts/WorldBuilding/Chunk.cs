/*
 Chunk.cs
 Class that represents a chunk of world, it holds the construction information of every block
 */
using UnityEngine;
using System.Threading;
using System.Collections;

//Required components
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{
    //References
    public World world;
    public WorldPos pos;

    //A 3D array to hold the block information stored on this chunk
    private Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];

    //The average size of a chunk
    public static int chunkSize = 10;
  

    //A flag to mark this chunk whenever its updated so that the information gets recalculated by the end of the frame
    public bool update = true;

    //Components on this chunk
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    
    //Use this for initialization
    void Start()
    {
        //Initializes component variables
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshCollider = gameObject.GetComponent<MeshCollider>();

        /*
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    if (z > 0 && z < chunkSize - 1 && x > 0 && x < chunkSize - 1 && y > 0 && y < chunkSize - 1)
                    {
                        blocks[x,y,z] = (y == 1) ? new Block() : new BlockEmpty();


                    }
                    else
                    {
                        blocks[x, y, z] = new BlockEmpty();
                    }

                }
            }
        }
        */
        //Updates a chunk
        //UpdateChunk();
    }



    //Update is called once per frame
    void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk();
        }
    }

    /// <summary>
    /// Retrieves a block object given the specific coordinates of it
    /// </summary>
    /// <param name="x">The x position of the requested block</param>
    /// <param name="y">The y position of the requested block</param>
    /// <param name="z">The z position of the requested block</param>
    /// <returns></returns>
    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];
        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
    }
    //new function
    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    /// <summary>
    ///Updates the chunk based on its contents
    ///</summary>
    void UpdateChunk()
    {
        //Thread t = new Thread(ThreadBlockData);
        //t.Start();
        //System.GC.Collect();

        ThreadBlockData();
    }

    void ThreadBlockData()
    {
        MeshData meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                }
            }
        }
        RenderMesh(meshData);
    }

    /// <summary>
    /// Sends the calculated mesh info to the mesh and collision components
    /// </summary>
    void RenderMesh(MeshData meshData)
    {
        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = meshData.vertices.ToArray();
        //Debug.Log(meshFilter.mesh.vertices.Length);
        meshFilter.mesh.triangles = meshData.triangles.ToArray();

        meshFilter.mesh.colors = meshData.colors.ToArray();

        meshFilter.mesh.uv = meshData.uv.ToArray();

        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();

        //Collision
        meshCollider.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = mesh;


    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }


}