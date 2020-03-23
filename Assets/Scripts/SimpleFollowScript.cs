using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowScript : MonoBehaviour
{
    public Transform player;
    public float followSpeed;

    private GameManager gm;

    private PlayerController pc;
    private Rigidbody rb;
    private ParticleSystem ps;
    private Vector3 spawnPos;
    private float dist;
    private bool paused;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        pc = player.gameObject.GetComponent<PlayerController>();
        ps = gameObject.GetComponent<ParticleSystem>();
        spawnPos = gameObject.transform.position;
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    void Update()
    {
        paused = gm.paused;

        if (!paused)
        {
            rb.AddForce((player.position - gameObject.transform.position).normalized * Time.deltaTime * followSpeed);
            dist = (player.position - gameObject.transform.position).magnitude;

            pc.distanceToEyes = dist;

            if (dist < 2)
            {
                pc.Death();

                gameObject.transform.position = spawnPos;
                ps.Stop();
                ps.Play();
            }
        }
    }
}
