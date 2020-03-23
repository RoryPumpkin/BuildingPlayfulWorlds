using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directionUI : MonoBehaviour
{
    public GameObject player;
    public GameObject center;
    public GameObject up;

    public float lineLength = 0.1f;

    private PlayerController pc;
    private LineRenderer lr;
    private float speed;
    private Vector3 dir;

    void Start()
    {
        pc = player.GetComponent<PlayerController>();
        lr = gameObject.GetComponent<LineRenderer>();
    }

    
    void Update()
    {
        speed = pc.speed;
        dir = pc.currentDirection;

        lr.SetPosition(0, center.transform.position);
        lr.SetPosition(1, center.transform.position + dir * lineLength);
    }
}
