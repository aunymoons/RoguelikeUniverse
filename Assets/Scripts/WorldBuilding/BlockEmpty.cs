/*
 BlockEmpty.cs
 Describe an empty cube
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEmpty : Block {

    public BlockEmpty(Color color)
         : base(color)
     {
        blockID = 0;
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
