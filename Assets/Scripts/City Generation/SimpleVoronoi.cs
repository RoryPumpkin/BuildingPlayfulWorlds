using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriangleNet;
using TriangleNet.Voronoi;

public class SimpleVoronoi : MonoBehaviour
{
    public VoronoiBase vb;

    private TriangleNet.Mesh plane;

    void Start()
    {
        //plane.triangles.pool = this.GetComponent<MeshFilter>().mesh.triangles;
    }

    void Update()
    {
        
    }
}
