using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPyramid : Block {


    public BlockPyramid()
         : base()
     {
        blockColor = Color.red;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override MeshData FaceDataUp
      (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.AddVertex(new Vector3(x, y + 0.5f, z), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);
        
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);

        meshData.uv.AddRange(FaceUVs(Direction.up));
        
        meshData.AddVertex(new Vector3(x, y + 0.5f, z), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);

        meshData.uv.AddRange(FaceUVs(Direction.up)); 

        meshData.AddVertex(new Vector3(x, y + 0.5f, z), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);

        meshData.uv.AddRange(FaceUVs(Direction.up));

        meshData.AddVertex(new Vector3(x, y + 0.5f, z), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), blockColor);
        meshData.AddTriangle(meshData.vertices.Count - 1);
        
        meshData.uv.AddRange(FaceUVs(Direction.up));

        return meshData;
    }
    protected override MeshData FaceDataDown
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

    protected override MeshData FaceDataNorth
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {
        //meshData.uv.AddRange(FaceUVs(Direction.north));
        return meshData;
    }

    protected override MeshData FaceDataEast
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {
        //meshData.uv.AddRange(FaceUVs(Direction.east));
        return meshData;
    }

    protected override MeshData FaceDataSouth
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {
        //meshData.uv.AddRange(FaceUVs(Direction.south));
        return meshData;
    }

    protected override MeshData FaceDataWest
        (Chunk chunk, float x, float y, float z, MeshData meshData)
    {
        //meshData.uv.AddRange(FaceUVs(Direction.west));
        return meshData;
    }


    /// <summary>
    /// Checks if the face in the specified direction is solid
    /// </summary>
    /// <param name="direction">the direction of the face to check</param>
    /// <returns></returns>
    public override bool IsSolid(Direction direction)
    {
        /*
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
        }
        */
        if(direction == Direction.down)
        {
            return true;
        }
        else
        {
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
            UVs[2] = new Vector2(tileSize * tilePos.x + (tileSize/2),
                tileSize * tilePos.y + tileSize);
        }
        
        return UVs;
    }


}
