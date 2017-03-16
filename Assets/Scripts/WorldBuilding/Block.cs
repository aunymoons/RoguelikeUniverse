/*
 Block.cs
 Class describe a cube information
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Block
{

    #region Variables

    /*POSITIONING AND MESH GENERATION*/

    //The face direction enum to calculate face visibility
    public enum Direction { north = 0, south = 1, east = 2, west = 3, up = 4, down = 5 };

    public int cubeDirection; //24 possible positions for each block // change for boolean flips, its easier

    public Vector3 blockRotation;

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

    public bool debugIsTrue;
    public bool[] pieces;
    public int[] sides;
    Direction[] invertedDirections;

    #region Mesh Generation

    /// <summary>
    /// The block constructor
    /// </summary>
    public Block(Vector3 rotation, Color color)
    {
        blockColor = color;
        blockRotation = rotation;

        sides = new int[6];
        invertedDirections = new Direction[6];

        for (int i = 0; i < 6; i++)
        {
            sides[i] = World.precalculatedRotations[(int)blockRotation.z / 90, (int)blockRotation.x / 90, (int)blockRotation.y / 90, i];
        }

        for (int i = 0; i < invertedDirections.Length; i++)
        {
            invertedDirections[i] = ReturnDirectionBasedOnRotation((Direction)i);
        }


    }

    /*

    protected Direction GetDirectionByNumber(int num)
    {
        switch (num)
        {
            case 0:
                return Direction.north;
            case 1:
                return Direction.south;
            case 2:
                return Direction.east;
            case 3:
                return Direction.west;
            case 4:
                return Direction.up;
            case 5:
                return Direction.down;
            default:
                //throw exception
                return Direction.north;
        }
    }

    protected int GetNumberByDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.north:
                return 0;
            case Direction.south:
                return 1;
            case Direction.east:
                return 2;
            case Direction.west:
                return 3;
            case Direction.up:
                return 4;
            case Direction.down:
                return 5;
            default:
                //throw exception
                return 0;
        }
    }

    */

    protected Direction GetInvertedDirection(Direction direction)
    {


        for (int i = 0; i < invertedDirections.Length; i++)
        {
            if (invertedDirections[i] == direction)
            {
                return (Direction)i;
            }
        }
        return direction;
    }

    protected void ClearPieces()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i] = false;
        }
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

    public virtual MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    {


        //meshData.useRenderDataForCol = true;
        /*
                if (
                    chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up, false) &&
                    chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down, false) &&
                    chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north, false) &&
                    chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south, false) &&
                    chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east, false) &&
                    chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west, false)
                )
                    return meshData;

                */

        blockPosition = new Vector3(x, y, z);

        if (!chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z))
        {
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, ReturnDirectionBasedOnRotation(GetInvertedDirection(Direction.up)));
        }

        if (!chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z))
        {
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, ReturnDirectionBasedOnRotation(GetInvertedDirection(Direction.down)));
        }

        if (!chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1))
        {
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, ReturnDirectionBasedOnRotation(GetInvertedDirection(Direction.north)));
        }

        if (!chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1))
        {
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, ReturnDirectionBasedOnRotation(GetInvertedDirection(Direction.south)));
        }

        if (!chunk.GetBlock(x + 1, y, z).IsCovered(Direction.west, chunk, x + 1, y, z))
        {
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, ReturnDirectionBasedOnRotation(GetInvertedDirection(Direction.east)));
        }

        if (!chunk.GetBlock(x - 1, y, z).IsCovered(Direction.east, chunk, x - 1, y, z))
        {
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, ReturnDirectionBasedOnRotation(GetInvertedDirection(Direction.west)));
        }


        CollisionBlockdata(chunk, x, y, z, meshData);


        return meshData;
    }

    /// <summary>
    /// Returns true if block is completely covered on all six sides, otherwise, returns the appropiate bool value for the asked face solidity
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="forCollision"></param>
    /// <returns></returns>
    public bool IsCovered(Direction direction, Chunk chunk, int x, int y, int z, bool forCollision = false)
    {
        //if (forCollision) return true;

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
            return CheckSolidityWithRotation(direction);
        }
    }

    /// <summary>
    /// Returns the solidity value of the asked face taking rotation in consideration
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public virtual bool CheckSolidityWithRotation(Direction direction)
    {
        return GetSolidity(ReturnDirectionBasedOnRotation(direction));
    }

    /// <summary>
    /// Returns if a face is solid based on the original construction values of the mesh
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public virtual bool GetSolidity(Direction direction)
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
                return true;
        }
    }


    /// <summary>
    /// Returns mesh data for this specific direction considering the blockrotation
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public MeshData GetMeshDataFromRotation(Chunk chunk, int x, int y, int z, MeshData meshData, Direction direction)
    {
        meshData = GetMeshData(chunk, x, y, z, meshData, ReturnDirectionBasedOnRotation(direction));

        return meshData;
    }

    MeshData GetMeshData(Chunk chunk, int x, int y, int z, MeshData meshData, Direction direction)
    {
        switch (direction)
        {
            case Direction.north:
                FaceDataNorth(chunk, x, y, z, meshData);
                break;
            case Direction.east:
                FaceDataEast(chunk, x, y, z, meshData);
                break;
            case Direction.south:
                FaceDataSouth(chunk, x, y, z, meshData);
                break;
            case Direction.west:
                FaceDataWest(chunk, x, y, z, meshData);
                break;
            case Direction.up:
                FaceDataUp(chunk, x, y, z, meshData);
                break;
            case Direction.down:
                FaceDataDown(chunk, x, y, z, meshData);
                break;
        }

        return meshData;
    }

    protected virtual MeshData FaceDataUp
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.up));

        return meshData;
    }

    protected virtual MeshData FaceDataDown
         (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.down));

        return meshData;
    }

    protected virtual MeshData FaceDataNorth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.north));

        return meshData;
    }

    protected virtual MeshData FaceDataEast
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.east));

        return meshData;
    }

    protected virtual MeshData FaceDataSouth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.south));

        return meshData;
    }

    protected virtual MeshData FaceDataWest
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.west));

        return meshData;
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



        switch (direction)
        {
            case Direction.east:
                tile.x = 2;
                tile.y = 3;
                break;
            case Direction.west:
                tile.x = 3;
                tile.y = 3;
                break;
            case Direction.up:
                tile.x = 0;
                tile.y = 2;
                break;
            case Direction.down:
                tile.x = 1;
                tile.y = 2;
                break;
            case Direction.north:
                tile.x = 0;
                tile.y = 3;
                break;
            case Direction.south:
                tile.x = 1;
                tile.y = 3;
                break;
        }

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

    protected Direction ReturnDirectionBasedOnRotation(Direction startDirection)
    {
        /*
        if (debugIsTrue)
        {
            Debug.Log((int)(blockRotation.z / 90) + " " + (int)(blockRotation.x / 90) + " " + (int)(blockRotation.y / 90));
        }
        */

        //return GetDirectionByNumber(World.precalculatedRotations[(int)(blockRotation.z / 90), (int)(blockRotation.x / 90), (int)(blockRotation.y / 90), GetNumberByDirection(startDirection)]);

        return (Direction)sides[(int)startDirection];

        //return GetDirectionByNumber(sides[GetNumberByDirection(startDirection)]);
    }

}
