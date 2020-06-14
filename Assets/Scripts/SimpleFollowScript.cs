using UnityEngine;

public class SimpleFollowScript : MonoBehaviour
{
    public Transform player;
    public float followSpeed;

    private PlayerController pc;
    private Rigidbody rb;
    public Vector3 spawnPos;
    private Vector3 vel;
    private bool unpause;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        pc = player.gameObject.GetComponent<PlayerController>();
        spawnPos = gameObject.transform.position;
    }

    void Update()
    {
        if (!GameManager.paused)
        {
            if (unpause)
            {
                rb.isKinematic = false;
                rb.velocity = vel;
                unpause = false;
            }
            rb.AddForce((player.position - gameObject.transform.position).normalized * Time.deltaTime * followSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            pc.Respawn(true);

            gameObject.transform.position = spawnPos;
        }
    }
    public void Pause()
    {
        if(rb != null)
        {
            vel = rb.velocity;
            rb.isKinematic = true;
            unpause = true;
        }
    }
}
