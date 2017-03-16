using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPyramid : Block
{

    int faceCounter;

   

    public BlockPyramid(Vector3 blockRotation, Color color)
         : base(blockRotation, color)
    {
        blockID = 2;
        pieces = new bool[1];
    }

    public override MeshData Blockdata
    (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        ClearPieces();
        return base.Blockdata(chunk, x, y, z, meshData);
    }

    protected override MeshData FaceDataUp
      (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        if (!pieces[0])
        {
            pieces[0] = true;

            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x, y + 0.5f, z) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);
            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x - 0.5f, y - 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);
            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x + 0.5f, y - 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);

            meshData.uv.AddRange(FaceUVs(Direction.up));

            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x, y + 0.5f, z) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);
            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x + 0.5f, y - 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);
            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x - 0.5f, y - 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);

            meshData.uv.AddRange(FaceUVs(Direction.up));

            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x, y + 0.5f, z) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);
            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x - 0.5f, y - 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);
            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x - 0.5f, y - 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);

            meshData.uv.AddRange(FaceUVs(Direction.up));

            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x, y + 0.5f, z) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);
            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x + 0.5f, y - 0.5f, z + 0.5f) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);
            meshData.AddVertex(blockRotationQuaternion * (new Vector3(x + 0.5f, y - 0.5f, z - 0.5f) - blockPosition) + blockPosition, blockColor);
            meshData.AddTriangle(meshData.vertices.Count - 1);

            meshData.uv.AddRange(FaceUVs(Direction.up));

        }

        return meshData;
    }

    protected override MeshData FaceDataNorth
      (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData = FaceDataUp(chunk, x, y, z, meshData);
        return meshData;
    }

    protected override MeshData FaceDataEast
      (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData = FaceDataUp(chunk, x, y, z, meshData);
        return meshData;
    }

    protected override MeshData FaceDataSouth
      (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData = FaceDataUp(chunk, x, y, z, meshData);
        return meshData;
    }

    protected override MeshData FaceDataWest
      (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData = FaceDataUp(chunk, x, y, z, meshData);
        return meshData;
    }

    /// <summary>
    /// Returns if an object is solid
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public override bool GetSolidity(Direction direction)
    {
        switch (direction)
        {
            case Direction.north:
                return false;
            case Direction.east:
                return false;
            case Direction.south:
                return false;
            case Direction.west:
                return false;
            case Direction.up:
                return false;
            case Direction.down:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Gets vectors for the tile position on the UV based on the face direction
    /// </summary>
    /// <param name="direction">the face direction</param>
    /// <returns></returns>
    public override Vector2[] FaceUVs(Direction direction)
    {
        Vector2[] UVs;
        if (direction == Direction.down)
        {
            UVs = new Vector2[4];
            Tile tilePos = TexturePosition(direction);
            UVs[0] = new Vector2(tileSize * tilePos.x + tileSize,
                tileSize * tilePos.y);
            UVs[1] = new Vector2(tileSize * tilePos.x + tileSize,
                tileSize * tilePos.y + tileSize);
            UVs[2] = new Vector2(tileSize * tilePos.x,
                tileSize * tilePos.y + tileSize);
            UVs[3] = new Vector2(tileSize * tilePos.x,
                tileSize * tilePos.y);
        }
        else
        {
            UVs = new Vector2[3];
            Tile tilePos = TexturePosition(direction);
            UVs[0] = new Vector2(tileSize * tilePos.x,
                tileSize * tilePos.y);
            UVs[1] = new Vector2(tileSize * tilePos.x + tileSize,
                tileSize * tilePos.y);
            UVs[2] = new Vector2(tileSize * tilePos.x + (tileSize / 2),
                tileSize * tilePos.y + tileSize);
        }

        return UVs;
    }


}
