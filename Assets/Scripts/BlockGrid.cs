using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrid : MonoBehaviour
{
    public GameObject smolblock;
    public GameObject block;
    public GameObject player;
    public GameObject spawnPoint;

    [Range(1, 10)]
    public int stepSize;

    [Range(0, 200)]
    public float minDist;
    [Range(0, 200)]
    public float maxDist;
    //[Range(0, 200)]
    //public float minDistS;
    //[Range(0, 200)]
    //public float maxDistS;


    public float scaleOffset;
    public float heightOffset;


    private float size_x, size_y, size_z;
    private int sx, sy, sz;
    private Vector3 spawnPos;
    private MeshRenderer mesh;

    void Start()
    {

        BlockScript sc = smolblock.GetComponent<BlockScript>();
        sc.minDist = minDist;
        //sc.minDistS = minDistS;
        sc.maxDist = maxDist;
        //sc.maxDistS = maxDistS;
        sc.yo = heightOffset;
        sc.so = scaleOffset;

        mesh = block.GetComponent<MeshRenderer>();
        mesh.enabled = false;

        Vector3 scale = block.transform.localScale;
        
        size_x = scale.x;
        size_y = scale.y;
        size_z = scale.z;

        //spawnPos = transform.position - new Vector3(((size_x * 0.5f) - 0.5f), ((size_y * 0.5f) - 0.5f), ((size_z * 0.5f) - 0.5f));
        //spawnPoint.transform.localScale = Vector3.one;
        //spawnPoint.transform.localPosition = new Vector3(-0.5f, -0.5f, -0.5f);
        spawnPos = spawnPoint.transform.position;

        for(int x = 0; x < size_x; x+=stepSize)
        {
            for (int y = 0; y < size_y; y+=stepSize)
            {
                for (int z = 0; z < size_z; z+=stepSize)
                {
                    GameObject b = Instantiate(smolblock, spawnPos + new Vector3( x, y, z), Quaternion.Euler(0, 0, 0));
                    b.GetComponent<BlockScript>().player = player.transform;
                    b.transform.parent = gameObject.transform;
                    //Debug.Log("spawned a cube");
                }
            }
        }
    }

    void Update()
    {
        
    }
}
