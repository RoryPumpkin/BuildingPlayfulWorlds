using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spearScript : MonoBehaviour
{
    public float throwDuration;
    public float throwSpeed;
    public float forcePosY;
    public float particleDuration;
    public float spearLength = 1.5f;

    public GameObject hitPoof;
    public GameObject markerCube;
    public GameObject rotPoint;
    public GameObject rayPoint;

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
        if (!spawned)
        {
            rb.AddForce(transform.up * throwSpeed, ForceMode.VelocityChange);
            //gameObject.layer = 0;
        }
        
    }

    void Update()
    {
        if (thrown)
        {
            //gameObject.transform.forward = Vector3.Slerp(-gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.collider.tag == "ground" && thrown)
        {
            ContactPoint point = collision.GetContact(0);

            gameObject.layer = 8;

            if (Physics.Raycast(rayPoint.transform.position, point.point - rayPoint.transform.position, out hit, 10f, ground_layer))
            {
                Debug.DrawLine(rayPoint.transform.position, point.point, Color.green, Mathf.Infinity);
                normal = hit.normal;
            } else
            {
                Debug.DrawLine(rayPoint.transform.position, point.point, Color.red, Mathf.Infinity);
                Debug.Log("spear ray didn't find a collider from " + rayPoint.transform.position + " to " + point.point + " with " + 10f + " as distance");
            }

            GameObject rot = Instantiate(rotPoint, point.point, Quaternion.Euler(Vector3.zero));
            GameObject poof = Instantiate(hitPoof, point.point, thrownRot);

            gameObject.transform.parent = poof.transform.parent = rot.transform;
            gameObject.transform.localPosition = new Vector3(0, -spearLength, 0);
            poof.transform.localPosition = new Vector3(0, -1f, 0);
            poof.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

            rb.constraints = RigidbodyConstraints.FreezeAll;
            rot.transform.rotation = Quaternion.FromToRotation(-Vector3.up, normal);
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
