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

    Vector3[] TMPVertices;
    int[] TMPIndices;

    int AddedIndices;
    int AddedVertices;

    public float CellSize;
    public Vector3 Offset;
    public int gridSize;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;

    }

    private void Start()
    {
        StructPixel();
        CreateMesh();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Home) == true)
        {
            DoubleTriangles();
            OptimizeMesh();
            CreateMesh();
        }
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
        TMPVertices = new Vector3[triangles.Length * 2];
        TMPIndices = new int[TMPVertices.Length*2];

        TMPVertices[0] = vertices[0];
        TMPVertices[1] = vertices[1];
        TMPVertices[2] = vertices[2];

        Vector3[] TMPHolder = new Vector3[6];
        int v = 0;

        for (int i = 0; i < triangles.Length; i += 3)
        {

            Vector3[] NewCorners = new Vector3[3];
            Vector3[] OldVerts = new Vector3[3];

            Vector3[] NewV = fetchnewvertices(i);

            OldVerts[0] = new Vector3(NewV[0].x, NewV[0].y, NewV[0].z);
            OldVerts[1] = new Vector3(NewV[1].x, NewV[1].y, NewV[1].z);
            OldVerts[2] = new Vector3(NewV[2].x, NewV[2].y, NewV[2].z);

            OldVerts = CorrectOrder(OldVerts);
            TMPVertices[AddedVertices] = OldVerts[0];

            NewCorners[0] = new Vector3((OldVerts[0].x + OldVerts[1].x) / 2, (OldVerts[0].y + OldVerts[1].y) / 2, (OldVerts[0].z + OldVerts[1].z) / 2);
            NewCorners[1] = new Vector3((OldVerts[1].x + OldVerts[2].x) / 2, (OldVerts[1].y + OldVerts[2].y) / 2, (OldVerts[1].z + OldVerts[2].z) / 2);
            NewCorners[2] = new Vector3((OldVerts[2].x + OldVerts[0].x) / 2, (OldVerts[2].y + OldVerts[0].y) / 2, (OldVerts[2].z + OldVerts[0].z) / 2);


            TMPHolder[0] = OldVerts[0];
            TMPHolder[1] = OldVerts[1];
            TMPHolder[2] = OldVerts[2];
            TMPHolder[3] = NewCorners[0];
            TMPHolder[4] = NewCorners[2];
            TMPHolder[5] = NewCorners[1];

            for(int o = 0; o < 5; o++)
            {
                TMPVertices[o + AddedVertices+1] = TMPHolder[o+1];
            }

            TMPIndices[0+v] = 5 + AddedIndices; TMPIndices[3+v] = 5 + AddedIndices; TMPIndices[6+v] = 5 + AddedIndices; TMPIndices[9+v] = 5 + AddedIndices;
            TMPIndices[1+v] = 0 + AddedIndices; TMPIndices[4+v] = 3 + AddedIndices; TMPIndices[7+v] = 4 + AddedIndices; TMPIndices[10+v] = 2 + AddedIndices;
            TMPIndices[2+v] = 3 + AddedIndices; TMPIndices[5+v] = 1 + AddedIndices; TMPIndices[8+v] = 0 + AddedIndices; TMPIndices[11+v] = 4 + AddedIndices;

            AddedVertices += 6;
            AddedIndices += 6;
            v += 12;
        }
        triangles = TMPIndices;
        vertices = TMPVertices;
        AddedIndices = 0;
        AddedVertices = 0;
    }   
    Vector3[] fetchnewvertices(int at)
    {
        int[] indices = new int[3];
        
        for(int i = 0 + at; i < 3 + at; i++)
        {
            indices[i-at] = triangles[i];
        }

        Vector3[] NewVertices = new Vector3[3];
        for (int i = 0; i < 3;  i++)
        {
            NewVertices[i] = vertices[indices[i]];
        }
        return NewVertices;
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

    void OptimizeMesh()
    {
        int RMVerts = 0;
        List<Vector3> optimizedlist = new List<Vector3>();
        List<int> optimizedIndicesList = new List<int>();

        optimizedlist.Add(vertices[0]);
        optimizedIndicesList.Add(triangles[0]);
        optimizedIndicesList.Add(triangles[1]);
        optimizedIndicesList.Add(triangles[2]);

        for(int i = 1; i < vertices.Length; i++)
        {
            bool wasfound = false;

            /*Checking if this vertice is already declared*/
            Vector3 Host = vertices[i];

            //Going through list again -1 where we are
            for(int o = 0; o < (vertices.Length+i) - vertices.Length; o++)
            {
                //if host == previously declared vertice
                if(Host.x == vertices[o].x && Host.y == vertices[o].y)
                {

                    wasfound = true;
                    RMVerts++;
                }
            }
            if(!wasfound)
            {
                optimizedlist.Add(Host);
            }
        }
        print(RMVerts + " Verticejä poistettu");
    }
}