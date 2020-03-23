using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject lightPoint;
    public GameObject spawnPoint;

    [Range(1, 10)]
    public int stepSize;

    private float size_x, size_y, size_z;
    private Vector3 spawnPos;

    void Start()
    {
        Vector3 scale = gameObject.transform.localScale;

        size_x = scale.x;
        size_y = scale.y;
        size_z = scale.z;

        //spawnPos = transform.position - new Vector3(((size_x * 0.5f) - 0.5f), ((size_y * 0.5f) - 0.5f), ((size_z * 0.5f) - 0.5f));
        spawnPoint.transform.localScale = Vector3.one;
        spawnPoint.transform.localPosition = new Vector3(-0.5f, -0.5f, -0.5f);
        spawnPos = spawnPoint.transform.position;
        spawnPos = new Vector3(spawnPos.x + 0.5f, spawnPos.y + 0.5f, spawnPos.z + 0.5f);

        for (int x = 0; x < size_x * stepSize; x+=stepSize)
        {
            for (int y = 0; y < size_y * stepSize; y+=stepSize)
            {
                for (int z = 0; z < size_z * stepSize; z+=stepSize)
                {
                    GameObject b = Instantiate(lightPoint, spawnPos + new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                    b.transform.parent = gameObject.transform;
                }
            }
        }
    }

    void Update()
    {

    }
}
