/*
 BlockEmpty.cs
 Describe an empty cube
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEmpty : Block {

    public BlockEmpty(Vector3 blockRotation, Color color)
         : base(blockRotation, color)
     {
    }
    public override MeshData Blockdata
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }
    public override bool IsSolid(Block.Direction direction, bool forCollision = false)
    {
        return false;
    }
}
