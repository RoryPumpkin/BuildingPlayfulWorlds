using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float speed;

    private Rigidbody rb;
    private Renderer rd;
    private int dir = 0;    //0 is no direction, 1 is left, 2 is right

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rd = gameObject.GetComponent<Renderer>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (dir == 0 || dir == 2)
        {
            rd.material.color = Color.yellow;
        }
        else rd.material.color = Color.blue;

        int tempDir = dir;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (tempDir == 0)
            {
                rb.velocity = Vector3.right * speed;
                dir = 1;
            }

            if (tempDir == 1)
            {
                rb.velocity = Vector3.left * speed;
                dir = 2;
            }

            if (tempDir == 2)
            {
                rb.velocity = Vector3.right * speed;
                dir = 1;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "obstacle")
        {
            Debug.Log("Game Over");
        }
    }
}
