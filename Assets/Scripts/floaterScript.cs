using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floaterScript : MonoBehaviour
{
    public float upForce;

    

    private Rigidbody rb;
    private Transform startTF;
    private Vector3 lfront, rfront, lback, rback;
    private Vector3 floatPos;

    void Start()
    {
        startTF = this.transform;

        //floatPos = new Vector3(startTF.position.x, startTF.position.y - 0.3f, startTF.position.z);
        floatPos = startTF.position;

        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        lfront = new Vector3(this.transform.position.x + (this.transform.localScale.x * 0.5f), this.transform.position.y - (this.transform.localScale.y * 0.5f), this.transform.position.z - (this.transform.localScale.z * 0.5f));
        rfront = new Vector3(this.transform.position.x - (this.transform.localScale.x * 0.5f), this.transform.position.y - (this.transform.localScale.y * 0.5f), this.transform.position.z - (this.transform.localScale.z * 0.5f));
        lback = new Vector3(this.transform.position.x + (this.transform.localScale.x * 0.5f), this.transform.position.y - (this.transform.localScale.y * 0.5f), this.transform.position.z + (this.transform.localScale.z * 0.5f));
        rback = new Vector3(this.transform.position.x - (this.transform.localScale.x * 0.5f), this.transform.position.y - (this.transform.localScale.y * 0.5f), this.transform.position.z + (this.transform.localScale.z * 0.5f));


        if (lfront.y < floatPos.y)
        {
            //float up = floatPos.y - this.transform.position.y;
            //up *= upForce;

            rb.AddForceAtPosition(Vector3.up * upForce, lfront);
        }

        if (rfront.y < floatPos.y)
        {
            //float up = floatPos.y - this.transform.position.y;
            //up *= upForce;

            rb.AddForceAtPosition(Vector3.up * upForce, rfront);
        }

        if (lback.y < floatPos.y)
        {
            //float up = floatPos.y - this.transform.position.y;
            //up *= upForce;

            rb.AddForceAtPosition(Vector3.up * upForce, lback);
        }

        if (rback.y < floatPos.y)
        {
            //float up = floatPos.y - this.transform.position.y;
            //up *= upForce;

            rb.AddForceAtPosition(Vector3.up * upForce, rback);
        }
        */

        rb.velocity = (floatPos - transform.position);
    }
}
