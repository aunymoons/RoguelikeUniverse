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
    public enum Direction { north, east, south, west, up, down };

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

    #region Mesh Generation

    /// <summary>
    /// The block constructor
    /// </summary>
    public Block(Vector3 rotation, Color color)
    {
        blockColor = color;
        blockRotation = rotation;
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

        if (!chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z))
        //if(GetBlockByRotation(chunk, x, y, z, Direction.up))
        {
            //meshData = FaceDataUp(chunk, x, y, z, meshData);
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, Direction.up);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z))
        //if (GetBlockByRotation(chunk, x, y, z, Direction.down))
        {
            //meshData = FaceDataDown(chunk, x, y, z, meshData);
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, Direction.down);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1))
        //if (GetBlockByRotation(chunk, x, y, z, Direction.north))
        {
            //meshData = FaceDataNorth(chunk, x, y, z, meshData);
            GetMeshDataFromRotation(chunk, x, y, z, meshData, Direction.north);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1))
        //if (GetBlockByRotation(chunk, x, y, z, Direction.south))
        {
            //meshData = FaceDataSouth(chunk, x, y, z, meshData);
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, Direction.south);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsCovered(Direction.west, chunk, x + 1, y, z))
        //if (GetBlockByRotation(chunk, x, y, z, Direction.east))
        {
            //meshData = FaceDataEast(chunk, x, y, z, meshData);
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, Direction.east);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsCovered(Direction.east, chunk, x - 1, y, z))
        //if (GetBlockByRotation(chunk, x, y, z, Direction.west))
        {
            //meshData = FaceDataWest(chunk, x, y, z, meshData);
            meshData = GetMeshDataFromRotation(chunk, x, y, z, meshData, Direction.west);
        }

        CollisionBlockdata(chunk, x, y, z, meshData);

        return meshData;
    }

    /*
    public bool GetBlockByRotation(Chunk chunk, int x, int y, int z, Direction direction)
    {
        Direction currentDirection = direction;
        bool result = true;


        for(int ix = 0; ix < 4; ix++)
        {
            for (int iy = 0; iy < 4; ix++)
            {
                for (int iz = 0; iz < 4; ix++)
                {
                    if(blockRotation.x == ix * 90)
                    {

                    }
                    if(blockRotation.y == iy * 90)
                    {

                    }
                    if(blockRotation.z == iz * 90)
                    {

                    }
                }
            }
        }

        //THIS COULD BE A LOOP
        if(blockRotation.x == 0)
        {
            if(blockRotation.y == 0)
            {
                if(blockRotation.z == 0)
                {

                }
                if(blockRotation.z == 90)
                {

                }
                if(blockRotation.z == 180)
                {

                }
                if(blockRotation.z == 270)
                {

                }
            }
            if(blockRotation.y == 90)
            {

            }
        }


        if (direction == Direction.north || direction == Direction.south || direction == Direction.up || direction == Direction.down)
        {
            if (blockRotation.x == 0)
            {
                switch (currentDirection)
                {
                    case Direction.north:
                        result = !chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1); // North
                        currentDirection = Direction.north;
                        break;
                    case Direction.south:
                        result = !chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1); //south
                        currentDirection = Direction.south;
                        break;
                    case Direction.up:
                        result = !chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z); //up
                        currentDirection = Direction.up;
                        break;
                    case Direction.down:
                        result = !chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z); //down
                        currentDirection = Direction.down;
                        break;
                }
            }
            if (blockRotation.x == 90)
            {
                switch (currentDirection)
                {
                    case Direction.north:
                        result = !chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z); //down
                        currentDirection = Direction.down;
                        break;
                    case Direction.south:
                        result = !chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z); //up
                        currentDirection = Direction.up;
                        break;
                    case Direction.up:
                        result = !chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1); // North
                        currentDirection = Direction.north;
                        break;
                    case Direction.down:
                        result = !chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1); //south
                        currentDirection = Direction.south;
                        break;
                }
            }
            if (blockRotation.x == 180)
            {
                switch (currentDirection)
                {
                    case Direction.north:
                        result = !chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1); //south
                        currentDirection = Direction.south;
                        break;
                    case Direction.south:
                        result = !chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1); // North
                        currentDirection = Direction.north;
                        break;
                    case Direction.up:
                        result = !chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z); //down
                        currentDirection = Direction.down;
                        break;
                    case Direction.down:
                        result = !chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z); //up
                        currentDirection = Direction.up;
                        break;
                }
            }
            if (blockRotation.x == 270)
            {
                switch (currentDirection)
                {
                    case Direction.north:
                        result = !chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z); //up
                        currentDirection = Direction.up;
                        break;
                    case Direction.south:
                        result = !chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z); //down
                        currentDirection = Direction.down;
                        break;
                    case Direction.up:
                        result = !chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1); //south
                        currentDirection = Direction.south;
                        break;
                    case Direction.down:
                        result = !chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1); // North
                        currentDirection = Direction.north;
                        break;
                }
            }
        }

        if (direction == Direction.north || direction == Direction.south || direction == Direction.east || direction == Direction.west)
        {
            if (blockRotation.y == 0)
            {
                switch (currentDirection)
                {
                    case Direction.north:
                        result = !chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1); // North
                        currentDirection = Direction.north;
                        break;
                    case Direction.south:
                        result = !chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1); //south
                        currentDirection = Direction.south;
                        break;
                    case Direction.east:
                        result = !chunk.GetBlock(x + 1, y, z).IsCovered(Direction.down, chunk, x + 1, y, z); //east
                        currentDirection = Direction.east; 
                        break;
                    case Direction.west:
                        result = !chunk.GetBlock(x - 1, y, z).IsCovered(Direction.up, chunk, x - 1, y, z); //west
                        currentDirection = Direction.west;
                        break;
                }
            }
            if (blockRotation.y == 90)
            {
                switch (currentDirection)
                {
                    case Direction.north:
                        result = !chunk.GetBlock(x - 1, y, z).IsCovered(Direction.up, chunk, x - 1, y, z); //west
                        currentDirection = Direction.west;
                        break;
                    case Direction.south:
                        result = !chunk.GetBlock(x + 1, y, z).IsCovered(Direction.down, chunk, x + 1, y, z); //east
                        currentDirection = Direction.east;
                        break;
                    case Direction.east:
                        result = !chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1); // North
                        currentDirection = Direction.north;
                        break;
                    case Direction.west:
                        result = !chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1); //south
                        currentDirection = Direction.south;
                        break;
                }
            }
            if (blockRotation.y == 180)
            {
                switch (currentDirection)
                {
                    case Direction.north:
                        result = !chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1); //south
                        currentDirection = Direction.south;
                        break;
                    case Direction.south:
                        result = !chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1); // North
                        currentDirection = Direction.north;
                        break;
                    case Direction.east:
                        result = !chunk.GetBlock(x - 1, y, z).IsCovered(Direction.up, chunk, x - 1, y, z); //west
                        currentDirection = Direction.west;
                        break;
                    case Direction.west:
                        result = !chunk.GetBlock(x + 1, y, z).IsCovered(Direction.down, chunk, x + 1, y, z); //east
                        currentDirection = Direction.east;
                        break;
                }
            }
            if (blockRotation.y == 270)
            {
                switch (currentDirection)
                {
                    case Direction.north:
                        result = !chunk.GetBlock(x + 1, y, z).IsCovered(Direction.down, chunk, x + 1, y, z); //east
                        currentDirection = Direction.east;
                        break;
                    case Direction.south:
                        result = !chunk.GetBlock(x - 1, y, z).IsCovered(Direction.up, chunk, x - 1, y, z); //west
                        currentDirection = Direction.west;
                        break;
                    case Direction.east:
                        result = !chunk.GetBlock(x, y, z - 1).IsCovered(Direction.north, chunk, x, y, z - 1); //south
                        currentDirection = Direction.south;
                        break;
                    case Direction.west:
                        result = !chunk.GetBlock(x, y, z + 1).IsCovered(Direction.south, chunk, x, y, z + 1); // North
                        currentDirection = Direction.north;
                        break;
                }
            }
        }

        if (direction == Direction.east || direction == Direction.west || direction == Direction.up || direction == Direction.down)
        {
            if (blockRotation.z == 0)
            {
                switch (currentDirection)
                {
                    case Direction.up:
                        result = !chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z); //up
                        currentDirection = Direction.up;
                        break;
                    case Direction.down:
                        result = !chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z); //down
                        currentDirection = Direction.down;
                        break;
                    case Direction.east:
                        result = !chunk.GetBlock(x + 1, y, z).IsCovered(Direction.down, chunk, x + 1, y, z); //east
                        currentDirection = Direction.east;
                        break;
                    case Direction.west:
                        result = !chunk.GetBlock(x - 1, y, z).IsCovered(Direction.up, chunk, x - 1, y, z); //west
                        currentDirection = Direction.west;
                        break;
                }
            }
            if (blockRotation.z == 90)
            {
                switch (currentDirection)
                {
                    case Direction.east:
                        result = !chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z); //up
                        currentDirection = Direction.up;
                        break;
                    case Direction.west:
                        result = !chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z); //down
                        currentDirection = Direction.down;
                        break;
                    case Direction.up:
                        result = !chunk.GetBlock(x - 1, y, z).IsCovered(Direction.up, chunk, x - 1, y, z); //west
                        currentDirection = Direction.west;
                        break;
                    case Direction.down:
                        result = !chunk.GetBlock(x + 1, y, z).IsCovered(Direction.down, chunk, x + 1, y, z); //east
                        currentDirection = Direction.east;
                        break;
                }
            }
            if (blockRotation.z == 180)
            {
                switch (currentDirection)
                {
                    case Direction.east:
                        result = !chunk.GetBlock(x - 1, y, z).IsCovered(Direction.up, chunk, x - 1, y, z); //west
                        currentDirection = Direction.west;
                        break;
                    case Direction.west:
                        result = !chunk.GetBlock(x + 1, y, z).IsCovered(Direction.down, chunk, x + 1, y, z); //east
                        currentDirection = Direction.east;
                        break;
                    case Direction.up:
                        result = !chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z); //down
                        currentDirection = Direction.down;
                        break;
                    case Direction.down:
                        result = !chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z); //up
                        currentDirection = Direction.up;
                        break;
                }
            }
            if (blockRotation.z == 270)
            {
                switch (currentDirection)
                {
                    case Direction.east:
                        result = !chunk.GetBlock(x, y - 1, z).IsCovered(Direction.up, chunk, x, y - 1, z); //down
                        currentDirection = Direction.down;
                        break;
                    case Direction.west:
                        result = !chunk.GetBlock(x, y + 1, z).IsCovered(Direction.down, chunk, x, y + 1, z); //up
                        currentDirection = Direction.up;
                        break;
                    case Direction.up:
                        result = !chunk.GetBlock(x + 1, y, z).IsCovered(Direction.down, chunk, x + 1, y, z); //east
                        currentDirection = Direction.east;
                        break;
                    case Direction.down:
                        result = !chunk.GetBlock(x - 1, y, z).IsCovered(Direction.up, chunk, x - 1, y, z); //west
                        currentDirection = Direction.west;
                        break;
                }
            }
        }

        return result;
    }
    */

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
            //return GetSolidity(direction);
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
        Direction currentDirection = direction;
        bool result = GetSolidity(currentDirection);

        //if (direction == Direction.north || direction == Direction.south || direction == Direction.up || direction == Direction.down)
        //{
        if (blockRotation.z == 0)
        {
            result = GetSolidity(currentDirection);
        }
        if (blockRotation.z == 90)
        {
            switch (currentDirection)
            {
                case Direction.east:
                    result = GetSolidity(Direction.down);
                    currentDirection = Direction.down;
                    break;
                case Direction.west:
                    result = GetSolidity(Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.up:
                    result = GetSolidity(Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.down:
                    result = GetSolidity(Direction.west);
                    currentDirection = Direction.west;
                    break;
                case Direction.north:
                    result = GetSolidity(Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.south:
                    result = GetSolidity(Direction.south);
                    currentDirection = Direction.south;
                    break;
                default:
                    result = GetSolidity(currentDirection);
                    break;
            }
        }
        if (blockRotation.z == 180)
        {
            switch (currentDirection)
            {
                case Direction.east:
                    result = GetSolidity(Direction.west);
                    currentDirection = Direction.west;
                    break;
                case Direction.west:
                    result = GetSolidity(Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.up:
                    result = GetSolidity(Direction.down);
                    currentDirection = Direction.down;
                    break;
                case Direction.down:
                    result = GetSolidity(Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.north:
                    result = GetSolidity(Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.south:
                    result = GetSolidity(Direction.south);
                    currentDirection = Direction.south;
                    break;
                default:
                    result = GetSolidity(currentDirection);
                    break;
            }
        }
        if (blockRotation.z == 270)
        {
            switch (currentDirection)
            {
                case Direction.east:
                    result = GetSolidity(Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.west:
                    result = GetSolidity(Direction.down);
                    currentDirection = Direction.down;
                    break;
                case Direction.up:
                    result = GetSolidity(Direction.west);
                    currentDirection = Direction.west;
                    break;
                case Direction.down:
                    result = GetSolidity(Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.north:
                    result = GetSolidity(Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.south:
                    result = GetSolidity(Direction.south);
                    currentDirection = Direction.south;
                    break;
                default:
                    result = GetSolidity(currentDirection);
                    break;
            }
        }

        if (blockRotation.x == 0)
        {
            result = GetSolidity(currentDirection);
        }
        if (blockRotation.x == 90)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    result = GetSolidity(Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.south:
                    result = GetSolidity(Direction.down);
                    currentDirection = Direction.down;
                    break;
                case Direction.up:
                    result = GetSolidity(Direction.south);
                    currentDirection = Direction.south;
                    break;
                case Direction.down:
                    result = GetSolidity(Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.east:
                    result = GetSolidity(Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.west:
                    result = GetSolidity(Direction.west);
                    currentDirection = Direction.west;
                    break;
                default:
                    result = GetSolidity(currentDirection);
                    break;
            }
        }
        if (blockRotation.x == 180)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    result = GetSolidity(Direction.south);
                    currentDirection = Direction.south;
                    break;
                case Direction.south:
                    result = GetSolidity(Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.up:
                    result = GetSolidity(Direction.down);
                    currentDirection = Direction.down;
                    break;
                case Direction.down:
                    result = GetSolidity(Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.east:
                    result = GetSolidity(Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.west:
                    result = GetSolidity(Direction.west);
                    currentDirection = Direction.west;
                    break;
                default:
                    result = GetSolidity(currentDirection);
                    break;
            }
        }
        if (blockRotation.x == 270)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    result = GetSolidity(Direction.down);
                    currentDirection = Direction.down;
                    break;
                case Direction.south:
                    result = GetSolidity(Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.up:
                    result = GetSolidity(Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.down:
                    result = GetSolidity(Direction.south);
                    currentDirection = Direction.south;
                    break;
                case Direction.east:
                    result = GetSolidity(Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.west:
                    result = GetSolidity(Direction.west);
                    currentDirection = Direction.west;
                    break;
                default:
                    result = GetSolidity(currentDirection);
                    break;
            }
        }


        //}

        //if (direction == Direction.north || direction == Direction.south || direction == Direction.east || direction == Direction.west)
        //{
        if (blockRotation.y == 0)
        {
            result = GetSolidity(currentDirection);
        }
        if (blockRotation.y == 90)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    result = GetSolidity(Direction.west);
                    currentDirection = Direction.west;
                    break;
                case Direction.south:
                    result = GetSolidity(Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.east:
                    result = GetSolidity(Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.west:
                    result = GetSolidity(Direction.south);
                    currentDirection = Direction.south;
                    break;
                case Direction.up:
                    result = GetSolidity(Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.down:
                    result = GetSolidity(Direction.down);
                    currentDirection = Direction.down;
                    break;
                default:
                    result = GetSolidity(currentDirection);
                    break;
            }
        }
        if (blockRotation.y == 180)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    result = GetSolidity(Direction.south);
                    currentDirection = Direction.south;
                    break;
                case Direction.south:
                    result = GetSolidity(Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.east:
                    result = GetSolidity(Direction.west);
                    currentDirection = Direction.west;
                    break;
                case Direction.west:
                    result = GetSolidity(Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.up:
                    result = GetSolidity(Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.down:
                    result = GetSolidity(Direction.down);
                    currentDirection = Direction.down;
                    break;
                default:
                    result = GetSolidity(currentDirection);
                    break;
            }
        }
        if (blockRotation.y == 270)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    result = GetSolidity(Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.south:
                    result = GetSolidity(Direction.west);
                    currentDirection = Direction.west;
                    break;
                case Direction.east:
                    result = GetSolidity(Direction.south);
                    currentDirection = Direction.south;
                    break;
                case Direction.west:
                    result = GetSolidity(Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.up:
                    result = GetSolidity(Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.down:
                    result = GetSolidity(Direction.down);
                    currentDirection = Direction.down;
                    break;
                default:
                    result = GetSolidity(currentDirection);
                    break;
            }
        }
        //}

        //if (direction == Direction.east || direction == Direction.west || direction == Direction.up || direction == Direction.down)
        //{

        //}

        if (debugIsTrue)
        {
            Debug.Log("SOLIDITY: the direction " + currentDirection + " is " + GetSolidity(currentDirection));
            /*
            switch (currentDirection)
            {
                case Direction.north:
                    result = GetSolidity(Direction.south);
                    break;
                case Direction.south:
                    result = GetSolidity(Direction.north);
                    break;
                case Direction.east:
                    result = GetSolidity(Direction.west);
                    break;
                case Direction.west:
                    result = GetSolidity(Direction.east);
                    break;
                case Direction.up:
                    result = GetSolidity(Direction.down);
                    break;
                case Direction.down:
                    result = GetSolidity(Direction.up);
                    break;
            }
            */

        }


        return result;
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
        Direction currentDirection = direction;
        //MeshData result;

        if (Input.GetKey(KeyCode.Space))
            Debug.Log(currentDirection);

        //if (direction == Direction.north || direction == Direction.south || direction == Direction.up || direction == Direction.down)
        //{
        if (blockRotation.z == 0)
        {
            //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
        }
        if (blockRotation.z == 90)
        {
            switch (currentDirection)
            {
                case Direction.east:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.up);
                    currentDirection = Direction.down;
                    break;
                case Direction.west:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.down);
                    currentDirection = Direction.up;
                    break;
                case Direction.up:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.west);
                    currentDirection = Direction.east;
                    break;
                case Direction.down:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.east);
                    currentDirection = Direction.west;
                    break;
                case Direction.north:
                    currentDirection = Direction.north;
                    break;
                case Direction.south:
                    currentDirection = Direction.south;
                    break;
                default:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
                    break;
            }
        }
        if (blockRotation.z == 180)
        {
            switch (currentDirection)
            {
                case Direction.east:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.west);
                    currentDirection = Direction.west;
                    break;
                case Direction.west:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.up:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.down);
                    currentDirection = Direction.down;
                    break;
                case Direction.down:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.north:
                    currentDirection = Direction.north;
                    break;
                case Direction.south:
                    currentDirection = Direction.south;
                    break;
                default:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
                    break;
            }
        }
        if (blockRotation.z == 270)
        {
            switch (currentDirection)
            {
                case Direction.east:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.down);
                    currentDirection = Direction.up;
                    break;
                case Direction.west:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.up);
                    currentDirection = Direction.down;
                    break;
                case Direction.up:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.east);
                    currentDirection = Direction.west;
                    break;
                case Direction.down:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.west);
                    currentDirection = Direction.east;
                    break;
                case Direction.north:
                    currentDirection = Direction.north;
                    break;
                case Direction.south:
                    currentDirection = Direction.south;
                    break;
                default:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
                    break;
            }
        }

        if (blockRotation.x == 0)
        {
            //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
        }
        if (blockRotation.x == 90)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.down);
                    currentDirection = Direction.up;
                    break;
                case Direction.south:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.up);
                    currentDirection = Direction.down;
                    break;
                case Direction.up:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.north);
                    currentDirection = Direction.south;
                    break;
                case Direction.down:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.south);
                    currentDirection = Direction.north;
                    break;
                case Direction.east:
                    currentDirection = Direction.east;
                    break;
                case Direction.west:
                    currentDirection = Direction.west;
                    break;
                default:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
                    break;
            }
        }
        if (blockRotation.x == 180)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.south);
                    currentDirection = Direction.south;
                    break;
                case Direction.south:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.up:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.down);
                    currentDirection = Direction.down;
                    break;
                case Direction.down:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.up);
                    currentDirection = Direction.up;
                    break;
                case Direction.east:
                    currentDirection = Direction.east;
                    break;
                case Direction.west:
                    currentDirection = Direction.west;
                    break;
                default:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
                    break;
            }
        }
        if (blockRotation.x == 270)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.up);
                    currentDirection = Direction.down;
                    break;
                case Direction.south:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.down);
                    currentDirection = Direction.up;
                    break;
                case Direction.up:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.south);
                    currentDirection = Direction.north;
                    break;
                case Direction.down:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.north);
                    currentDirection = Direction.south;
                    break;
                case Direction.east:
                    currentDirection = Direction.east;
                    break;
                case Direction.west:
                    currentDirection = Direction.west;
                    break;
                default:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
                    break;
            }
        }
        //}
        if (Input.GetKey(KeyCode.Space))
            Debug.Log(currentDirection);
        //if (direction == Direction.north || direction == Direction.south || direction == Direction.east || direction == Direction.west)
        //{

        if (blockRotation.y == 0)
        {
            //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
        }
        if (blockRotation.y == 90)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.west);
                    currentDirection = Direction.west;
                    break;
                case Direction.south:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.east:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.west:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.south);
                    currentDirection = Direction.south;
                    break;
                case Direction.up:
                    currentDirection = Direction.up;
                    break;
                case Direction.down:
                    currentDirection = Direction.down;
                    break;
                default:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
                    break;
            }
        }
        if (blockRotation.y == 180)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.south);
                    currentDirection = Direction.south;
                    break;
                case Direction.south:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.east:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.west);
                    currentDirection = Direction.west;
                    break;
                case Direction.west:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.up:
                    currentDirection = Direction.up;
                    break;
                case Direction.down:
                    currentDirection = Direction.down;
                    break;
                default:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
                    break;
            }
        }
        if (blockRotation.y == 270)
        {
            switch (currentDirection)
            {
                case Direction.north:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.east);
                    currentDirection = Direction.east;
                    break;
                case Direction.south:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.west);
                    currentDirection = Direction.west;
                    break;
                case Direction.east:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.south);
                    currentDirection = Direction.south;
                    break;
                case Direction.west:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,Direction.north);
                    currentDirection = Direction.north;
                    break;
                case Direction.up:
                    currentDirection = Direction.up;
                    break;
                case Direction.down:
                    currentDirection = Direction.down;
                    break;
                default:
                    //meshData = GetMeshData(chunk,x,y,z,meshData,currentDirection);
                    break;
            }
        }
        //}
        if (Input.GetKey(KeyCode.Space))
            Debug.Log(currentDirection);
        //if (direction == Direction.east || direction == Direction.west || direction == Direction.up || direction == Direction.down)
        //{



        //}
        if (Input.GetKey(KeyCode.Space))
            Debug.Log(currentDirection);

        if (debugIsTrue)
        {
            Debug.Log("MESH DATA: the direction " + direction + " has returned " + currentDirection);

           
        }
        meshData = GetMeshData(chunk, x, y, z, meshData, currentDirection);

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
    /*
    Vector3 RotateVectorSequencially(int x, int y, int z, Vector3 targetVector)
    {
        Vector3 zRotation = new Vector3(0, 0, blockRotation.z);
        Vector3 xRotation = new Vector3(0, blockRotation.x, 0);
        Vector3 yRotation = new Vector3(blockRotation.y, 0, 0);

        targetVector = Quaternion.Euler(zRotation) * (targetVector - new Vector3(x, y, z)) + new Vector3(x, y, z);
        targetVector = Quaternion.Euler(xRotation) * (targetVector - new Vector3(x, y, z)) + new Vector3(x, y, z);
        targetVector = Quaternion.Euler(yRotation) * (targetVector - new Vector3(x, y, z)) + new Vector3(x, y, z);

    }
    */
    protected virtual MeshData FaceDataUp
         (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.up));

        return meshData;
    }

    protected virtual MeshData FaceDataDown
         (Chunk chunk, float x, float y, float z, MeshData meshData)
    {
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.down));

        return meshData;
    }

    protected virtual MeshData FaceDataNorth
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.north));

        return meshData;
    }

    protected virtual MeshData FaceDataEast
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.east));

        return meshData;
    }

    protected virtual MeshData FaceDataSouth
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y + 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x + 0.5f, y - 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);

        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.south));

        return meshData;
    }

    protected virtual MeshData FaceDataWest
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {

        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z + 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y + 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);
        meshData.AddVertex(Quaternion.Euler(blockRotation) * (new Vector3(x - 0.5f, y - 0.5f, z - 0.5f) - new Vector3(x, y, z)) + new Vector3(x, y, z), blockColor);

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

}
