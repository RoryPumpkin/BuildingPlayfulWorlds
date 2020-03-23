using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spearScript : MonoBehaviour
{
    public float throwDuration;
    public float throwSpeed;
    public float forcePosY;
    public float particleDuration;

    public GameObject hitPoof;
    public GameObject markerCube;
    public GameObject rotPoint;

    public LayerMask ground_layer;
    public Transform rayCastPoint;

    private Rigidbody rb;

    private Vector3 normal;
    private RaycastHit hit;

    public bool thrown = false, particles = true, spawned = false;

    public Quaternion thrownRot;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * throwSpeed, ForceMode.VelocityChange);
        gameObject.layer = 0;
    }

    void Update()
    {
        if (thrown)
        {
            gameObject.transform.forward = Vector3.Slerp(-gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "ground" && thrown)
        {
            ContactPoint point = collision.GetContact(0);

            gameObject.layer = 8;

            if (Physics.Raycast(gameObject.transform.position, collision.gameObject.transform.position - gameObject.transform.position, out hit, 2f, ground_layer))
            {
                normal = hit.normal;
                //Debug.Log(normal);
                //Debug.Log(collision.gameObject.name);
            }


            GameObject rot = Instantiate(rotPoint, point.point, thrownRot);
            GameObject poof = Instantiate(hitPoof, point.point, thrownRot);

            gameObject.transform.parent = poof.transform.parent = rot.transform;
            gameObject.transform.localPosition = new Vector3(0, -1.5f, 0);
            poof.transform.localPosition = new Vector3(0, -1f, 0);
            poof.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

            rb.constraints = RigidbodyConstraints.FreezeAll;
            rot.transform.rotation = Quaternion.FromToRotation(Vector3.right, normal);
            rot.transform.rotation = Quaternion.Euler( 0, rot.transform.rotation.eulerAngles.y, rot.transform.rotation.eulerAngles.z);

            rot.transform.parent = collision.gameObject.transform;

            rb.isKinematic = true;

            thrown = false;

            
        }
        else if(thrown)
        {
            ContactPoint point = collision.GetContact(0);
            normal = point.normal;

            GameObject poof = Instantiate(hitPoof, point.point, thrownRot);
            //poof.transform.localPosition = new Vector3(0, -1f, 0);
            poof.transform.localRotation = Quaternion.Euler(-90, 0, 0);

            //rb.isKinematic = true;

            thrown = false;
        }
        /*
        else if (collision.collider.tag == "Object" && thrown)
        {
            ContactPoint point = collision.GetContact(0);

            Instantiate(markerCube, point.point, thrownRot);

            //gameObject.transform.rotation = thrownRot;


            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            FixedJoint fix = point.otherCollider.gameObject.AddComponent<FixedJoint>();
            fix.connectedBody = gameObject.GetComponent<Rigidbody>();
            //fix.enablePreprocessing = false;

            thrown = false;
        }
        */
    }
}
