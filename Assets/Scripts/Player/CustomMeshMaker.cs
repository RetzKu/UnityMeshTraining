using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class CustomMeshMaker : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public float CellSize;
    public Vector3 Offset;
    public int gridSize;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        //StructPixel();
        DoubleTriangles();
        CreateMesh();
    }

    void StructPixel()
    {
        vertices = new Vector3[(gridSize+1)* (gridSize+1)];
        triangles = new int[gridSize * gridSize *6];

        int v = 0;
        int t = 0;

        float vertexoffset = CellSize / 0.5f;
        
        for (int x = 0; x <= gridSize; x++)
        {
            for (int y = 0; y <= gridSize; y++)
            {
                vertices[v] = new Vector3((x * CellSize) - vertexoffset, (y * CellSize) - vertexoffset);
                v++;
            }

        }
        v = 0;
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                triangles[t] = v;
                triangles[t + 1] = v + 1;
                triangles[t + 2] = v + (gridSize + 1);
                triangles[t + 3] = v + (gridSize + 1);
                triangles[t + 4] = v + 1;
                triangles[t + 5] = v + (gridSize + 1) + 1; 
                v++;
                t += 6;
            }
            v++;
        }
    }

    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
    void DoubleTriangles()
    {
        vertices = new Vector3[6];
        triangles = new int[12];
        
        Vector3[] NewCorners = new Vector3[3];
        Vector3[] OldVerts = new Vector3[3];

        vertices[0] = new Vector3(-1, -1);
        vertices[1] = new Vector3(-1, 1);
        vertices[2] = new Vector3(1, -1);


        OldVerts[0] = new Vector3(vertices[0].x, vertices[0].y,vertices[0].z);
        OldVerts[1] = new Vector3(vertices[1].x, vertices[1].y,vertices[1].z);
        OldVerts[2] = new Vector3(vertices[2].x, vertices[2].y,vertices[2].z);

        OldVerts = CorrectOrder(vertices);
        
        NewCorners[0] = new Vector3((OldVerts[0].x + OldVerts[1].x) / 2, (OldVerts[0].y + OldVerts[1].y) / 2,(OldVerts[0].z + OldVerts[1].z)/2);
        NewCorners[1] = new Vector3((OldVerts[1].x + OldVerts[2].x) / 2, (OldVerts[1].y + OldVerts[2].y) / 2,(OldVerts[1].z + OldVerts[2].z)/2);
        NewCorners[2] = new Vector3((OldVerts[2].x + OldVerts[0].x) / 2, (OldVerts[2].y + OldVerts[0].y) / 2,(OldVerts[2].z + OldVerts[0].z)/2);


        vertices[0] = OldVerts[0];
        vertices[1] = NewCorners[0];
        vertices[2] = NewCorners[1];
        vertices[3] = OldVerts[1];
        vertices[4] = NewCorners[2];
        vertices[5] = OldVerts[2];

        triangles[0] = 2; triangles[3] = 2; triangles[6] = 2; triangles[9] =  2;
        triangles[1] = 0; triangles[4] = 1; triangles[7] = 4; triangles[10] = 5;
        triangles[2] = 1; triangles[5] = 3; triangles[8] = 0; triangles[11] = 4;
    }
    Vector3[] CorrectOrder(Vector3[] Vertices)
    {
        Vector3[] NewOrder = new Vector3[3];

        float[] distance = new float[3];
        distance[0] = Vector3.Distance(Vertices[0], Vertices[1]);
        distance[1] = Vector3.Distance(Vertices[1], Vertices[2]);
        distance[2] = Vector3.Distance(Vertices[2], Vertices[0]);
        float longest;

        if(distance[0] > distance[1])
        {
            if(distance[0] > distance[2])
            {
                longest = distance[0];
            }
            else { longest = distance[2]; }
        }
        else
        {
            if(distance[1] > distance[2])
            {
                longest = distance[1];
            }
            else { longest = distance[2]; }
        }
        
        if(distance[0] == longest) {
            NewOrder[0] = Vertices[2];
            NewOrder[1] = Vertices[0];
            NewOrder[2] = Vertices[1];
        }
        if(distance[1] == longest) {
            NewOrder[0] = Vertices[0];
            NewOrder[1] = Vertices[1];
            NewOrder[2] = Vertices[2];
        }
        if(distance[2] == longest) {
            NewOrder[0] = Vertices[1];
            NewOrder[1] = Vertices[2];
            NewOrder[2] = Vertices[0];
        }

        return NewOrder;
    }
}