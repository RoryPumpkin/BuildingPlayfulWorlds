using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldPlayerController : MonoBehaviour
{
    private float rightKey, leftKey, upKey, downKey, jumpKey, crouchKey, action1Key;

    public Transform cam;

    public GameObject spear;
    public Transform spearThrowTrans;

    public PhysicMaterial frictionMat;

    public float jumpVel;
    public float moveVel, airMoveVel;
    public float maxSpeed, airMaxSpeed;
    public float friction, frictionlessTime;
    public float spearThrowSpeed;

    private bool ground = true;
    private float velX, velY, velZ;
    private float speed;

    private Vector2 xzPlane;
    private Rigidbody rb;
    private Vector3 currentRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        speed = rb.velocity.sqrMagnitude;


        //Vector2 mp = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mp = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //Debug.Log(mp);
        currentRotation = this.transform.rotation.eulerAngles;

        currentRotation.y += mp.x;

        this.transform.rotation = Quaternion.Euler(currentRotation);


        velX = rb.velocity.x;
        velY = rb.velocity.y;

        CheckInputs();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowSpear();
        }

        //set velocity from right and left keys
        velX = rightKey - leftKey;
        velZ = upKey - downKey;

        //ground plane velocity
        if (ground)
        {
            if (speed > maxSpeed)
            {
                xzPlane = new Vector2(velX * moveVel * 0.05f, velZ * moveVel * 0.05f);
                //Debug.Log(xzPlane.sqrMagnitude);
            }
            else xzPlane = new Vector2(velX * moveVel, velZ * moveVel);

        }
        else
        {

            xzPlane = new Vector2(velX * airMoveVel, velZ * airMoveVel);
        }

        if (!ground)
        {
            frictionMat.dynamicFriction = 0f;
        }

        if (ground && jumpKey > 0.5)
        {
            ground = false;


            float x = rb.velocity.x;
            float z = rb.velocity.z;
            //velY = jumpVel;
            rb.velocity = new Vector3(x, jumpVel, z);

        }

        //rb.velocity = new Vector3(xzPlane.x, velY, xzPlane.y);

        if (rb.velocity.sqrMagnitude > maxSpeed)
        {
            Debug.Log(rb.velocity.sqrMagnitude);
        }
        else rb.AddForce(this.transform.TransformDirection(xzPlane.x, 0, xzPlane.y).normalized * moveVel);



        //Debug.Log(rb.velocity.sqrMagnitude);
    }

    IEnumerator FrictionlessTime()
    {
        frictionMat.dynamicFriction = 0f;
        yield return new WaitForSeconds(frictionlessTime);
        frictionMat.dynamicFriction = friction;
    }

    void ThrowSpear()
    {
        //make the spear aim towards transform.forward
        GameObject lastSpear = Instantiate<GameObject>(spear, spearThrowTrans.position, spearThrowTrans.rotation);
        spearScript sc = lastSpear.GetComponent<spearScript>();
        sc.thrown = true;
        sc.thrownRot = spearThrowTrans.rotation;
        sc.throwSpeed = spearThrowSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "ground")
        {
            ground = true;
            StartCoroutine(FrictionlessTime());
        }
        else
        {

        }
    }

    private void OnTriggerEnter(Collider col)
    {

    }

    void CheckInputs()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            action1Key = 0;
        }

        if (Input.GetKeyUp("right") || Input.GetKeyUp(KeyCode.D))
        {
            rightKey = 0;
        }
        if (Input.GetKeyUp("left") || Input.GetKeyUp(KeyCode.A))
        {
            leftKey = 0;
        }
        if (Input.GetKeyUp("up") || Input.GetKeyUp(KeyCode.W))
        {
            upKey = 0;
        }
        if (Input.GetKeyUp("down") || Input.GetKeyUp(KeyCode.S))
        {
            downKey = 0;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpKey = 0;
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            action1Key = 1;
        }
        if (Input.GetKeyDown("right") || Input.GetKeyDown(KeyCode.D))
        {
            rightKey = 1;
        }
        if (Input.GetKeyDown("left") || Input.GetKeyDown(KeyCode.A))
        {
            leftKey = 1;
        }
        if (Input.GetKeyDown("up") || Input.GetKeyDown(KeyCode.W))
        {
            upKey = 1;
        }
        if (Input.GetKeyDown("down") || Input.GetKeyDown(KeyCode.S))
        {
            downKey = 1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKey = 1;
        }
    }
}