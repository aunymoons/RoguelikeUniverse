﻿/*
 Block.cs
 Class describe a cube information
 */
using UnityEngine;
using System.Collections;
public class Block
{

    #region Variables

    /*POSITIONING AND MESH GENERATION*/

    //The face direction enum to calculate face visibility
    public enum Direction { north, east, south, west, up, down };

    public int cubeDirection; //24 possible positions for each block // change for boolean flips, its easier

    public Vector3 blockPosition;

    /*COLOR*/
    public Color blockColor = Color.white;

    /*TEXTURING*/

    //Struct for tile positioning
    public struct Tile { public int x; public int y; }

    //Tile size constant
    public float tileSize = 0.25f;

	//Flags
	public bool covered;

    #endregion

    #region Mesh Generation

    /// <summary>
    /// The block constructor
    /// </summary>
    public Block()
    {
    }
    
    /// <summary>
    /// Function that returns the meshdata in a position given a set of coordinates and a chunk
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns></returns>
    public virtual MeshData Blockdata (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        //meshData.useRenderDataForCol = true;

        if (!chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z))
        {
            meshData = FaceDataDown(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1))
        {
            meshData = FaceDataNorth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1))
        {
            meshData = FaceDataSouth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsCovered(Direction.west, chunk, x + 1, y, z))
        {
            meshData = FaceDataEast(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsCovered(Direction.east, chunk, x - 1, y, z))
        {
            meshData = FaceDataWest(chunk, x, y, z, meshData);
        }

        CollisionBlockdata(chunk,x,y,z,meshData);

        return meshData;
    }

    

    protected virtual MeshData FaceDataUp
         (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), blockColor);
        
        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.up));

        return meshData;
    }
    protected virtual MeshData FaceDataDown
         (Chunk chunk, float x, float y, float z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.down));

        return meshData;
    }

    protected virtual MeshData FaceDataNorth
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.north));

        return meshData;
    }

    protected virtual MeshData FaceDataEast
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.east));

        return meshData;
    }

    protected virtual MeshData FaceDataSouth
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.south));

        return meshData;
    }

    protected virtual MeshData FaceDataWest
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), blockColor);
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.west));

        return meshData;
    }


    /// <summary>
    /// Checks if the face in the specified direction is solid
    /// </summary>
    /// <param name="direction">the direction of the face to check</param>
    /// <returns></returns>
    public virtual bool IsSolid(Direction direction, bool forCollision = false)
    {

        if (covered || forCollision)
        {
            return true;
        }
        else
        {
            switch (direction)
        {
            case Direction.north:
                return true;
            case Direction.east:
                return true;
            case Direction.south:
                return true;
            case Direction.west:
                return true;
            case Direction.up:
                return true;
            case Direction.down:
                return true;
		default:
			return false;
        }
        }
    }

    public bool IsCovered(Direction direction, Chunk chunk, int x, int y, int z, bool forCollision = false)
    {
        

        if (
            chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up, forCollision) &&
            chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down, forCollision) &&
            chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north, forCollision) &&
            chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south, forCollision) &&
            chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east, forCollision) &&
            chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west, forCollision)
        )
        {
            return true;
        }
        else
        {
            return IsSolid(direction, forCollision);
        }
    }


    #endregion

    #region ColliderData

    /// <summary>
    /// Function that returns the Collision meshdata in a position given a set of coordinates and a chunk
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns></returns>
    public virtual MeshData CollisionBlockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        if (!chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z, true))
        {
            meshData = ColFaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z, true))
        {
            meshData = ColFaceDataDown(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1, true))
        {
            meshData = ColFaceDataNorth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1, true))
        {
            meshData = ColFaceDataSouth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsCovered(Direction.west, chunk, x + 1, y, z, true))
        {
            meshData = ColFaceDataEast(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsCovered(Direction.east, chunk, x - 1, y, z, true))
        {
            meshData = ColFaceDataWest(chunk, x, y, z, meshData);
        }

        return meshData;
    }



    protected virtual MeshData ColFaceDataUp
         (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.AddColVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddColVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddColVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddColVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

        meshData.AddColQuadTriangles();

        return meshData;
    }
    protected virtual MeshData ColFaceDataDown
         (Chunk chunk, float x, float y, float z, MeshData meshData)
    {
        meshData.AddColVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddColVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddColVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddColVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddColQuadTriangles();

        return meshData;
    }

    protected virtual MeshData ColFaceDataNorth
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddColVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddColVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddColVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddColVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddColQuadTriangles();

        return meshData;
    }

    protected virtual MeshData ColFaceDataEast
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddColVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddColVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddColVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddColVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddColQuadTriangles();

        return meshData;
    }

    protected virtual MeshData ColFaceDataSouth
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddColVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddColVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddColVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddColVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddColQuadTriangles();

        return meshData;
    }

    protected virtual MeshData ColFaceDataWest
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddColVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddColVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddColVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddColVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddColQuadTriangles();

        return meshData;
    }

    #endregion

    #region Texturing

    /// <summary>
    /// Get new tile struct positions
    /// </summary>
    /// <param name="direction">the face direction</param>
    /// <returns></returns>
    public virtual Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }

    /// <summary>
    /// Gets vectors for the tile position on the UV based on the face direction
    /// </summary>
    /// <param name="direction">the face direction</param>
    /// <returns></returns>
    public virtual Vector2[] FaceUVs(Direction direction)
    {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(direction);
        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y);
        return UVs;
    }

    #endregion

}
