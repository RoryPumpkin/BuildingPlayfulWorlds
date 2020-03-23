using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class proceduralMesh : MonoBehaviour
{
    //Board to bits games - procedural mesh tutorial on youtube


    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    //grid settings
    public float cellSize = 1;
    public Vector3 gridOffset;
    public int gridSize;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Start()
    {
        MakeContiguousProceduralGrid();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        //MakeMeshData();
        //CreateMesh();
    }

    void MakeMeshData()
    {
        vertices = new Vector3[]{new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 1) };
        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }

    void MakeDiscreteProceduralGrid()
    {
        //set array sizes
        vertices = new Vector3[gridSize * gridSize * 4];
        triangles = new int[gridSize * gridSize * 6];

        //set tracker integers
        int v = 0;
        int t = 0;

        //set vertex offset
        float vertexOffset = cellSize * 0.5f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 cellOffset = new Vector3(x * cellSize, 0, y * cellSize);

                //populate the vertices and triangles 
                vertices[v] = new Vector3(-vertexOffset, 0, -vertexOffset) + transform.position + cellOffset + gridOffset;
                vertices[v+1] = new Vector3(-vertexOffset, 0, vertexOffset) + transform.position + cellOffset + gridOffset;
                vertices[v+2] = new Vector3(vertexOffset, 0, -vertexOffset) + transform.position + cellOffset + gridOffset ;
                vertices[v+3] = new Vector3(vertexOffset, 0, vertexOffset) + transform.position + cellOffset + gridOffset;

                triangles[t] = v;
                triangles[t+1] = triangles[t+4] = v+1;
                triangles[t+2] = triangles[t+3] = v+2;
                triangles[t+5] = v+3;

                v += 4;
                t += 6;
            }
        }
    }

    void MakeContiguousProceduralGrid()
    {
        //set array sizes
        vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
        triangles = new int[gridSize * gridSize * 6];

        //set tracker integers
        int v = 0;
        int t = 0;

        //set vertex offset
        float vertexOffset = cellSize * 0.5f;

        //create vertex grid
        for (int x = 0; x <= gridSize; x++)
        {
            for (int y = 0; y <= gridSize; y++)
            {
                vertices[v] = new Vector3((x * cellSize) - vertexOffset, (x + y) * 0.2f, (y * cellSize) - vertexOffset);
                v++;
            }
        }

        //reset vertex tracker
        v = 0;

        //setting each cell's triangles
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                triangles[t] = v;
                triangles[t + 1] = triangles[t + 4] = v + 1;
                triangles[t + 2] = triangles[t + 3] = v + (gridSize + 1);
                triangles[t + 5] = v + (gridSize + 1) + 1;
                v++;
                t += 6;
            }
            v++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

    }
}
