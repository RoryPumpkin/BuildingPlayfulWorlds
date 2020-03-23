using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{

    public GameObject obst;
    public Transform player;
    public float spawnTimer = 10;

    private Vector3 startPos;
    private float timer;

    void Start()
    {
        //timer = spawnTimer;
        startPos = this.transform.position;
    }

    
    void Update()
    {
        this.transform.position = new Vector3(player.position.x + Random.Range( -3f, 3f), startPos.y, startPos.z);

        timer = timer - Time.deltaTime * 10;

        if (timer <= 0)
        {
            Instantiate<GameObject>(obst, this.transform.position, this.transform.rotation);
            timer = spawnTimer + Random.Range(-3f, 3f);
        }
        
    }
}
