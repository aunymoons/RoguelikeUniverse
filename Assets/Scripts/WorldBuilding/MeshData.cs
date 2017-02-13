/*
 Meshdata.cs
 Class to store the mesh data to build a specific mesh
 */
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MeshData
{
    //Lists to hold all the vertices triangles and UV's
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uv = new List<Vector2>();

    //Color
    public List<Color> colors = new List<Color>();
    
    //Collisions
    public bool useRenderDataForCol;
    public List<Vector3> colVertices = new List<Vector3>();
    public List<int> colTriangles = new List<int>();

    //Constructor method
    public MeshData() { }

    //Adds a vertex to the meshData 
    public void AddVertex(Vector3 vertex, Color vertexColor)
    {
        vertices.Add(vertex);

        colors.Add(vertexColor);
        
        if (useRenderDataForCol)
        {
            colVertices.Add(vertex);
        }
    }

    //To be used for non-cubical shapes
    public void AddTriangle(int tri)
    {
        triangles.Add(tri);
        if (useRenderDataForCol)
        {
            colTriangles.Add(tri - (vertices.Count - colVertices.Count));
        }
    }
    

    public void AddQuadTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        //Collision data
        if (useRenderDataForCol)
        {
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 3);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 1);
        }
    }
}
