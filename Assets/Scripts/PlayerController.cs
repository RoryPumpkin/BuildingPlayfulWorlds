using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum States { pause, move, jump, fall}
    public States currentState;

    private float rightKey, leftKey, upKey, downKey, jumpKey, crouchKey, action1Key;
    private float grabKey;

    public Transform cam;
    public GameObject dragPoint;
    private Vector3 dragVec;
    public GameObject swingPoint;

    public GameObject rope;

    public GameObject spear;
    public Transform spearThrowTrans;

    public PhysicMaterial frictionMat;

    public float jumpVel;
    public float moveVel;
    public float maxSpeed;
    public float friction, frictionlessTime;

    public float sensitivity_x, sensitivity_y;

    //groundMargin is the range where you touch the ground
    public float groundMargin = 0.5f;

    //the distance you can start a wallrun to the left and right
    public float wallCheckDist = 1f;

    //stepHeight is the max height for automatically stepping over objects
    public float stepHeight = 0.65f;

    //the time you can be airborne but still jump
    public float canJumpDelay = 0.4f;

    public float spearThrowSpeed;
    public float grabRange = 10, grabStrength = 4f, grabDownSize = 0.5f, grappleRange = 30, ropeGrabForce = 20f, throwForce = 80f;
    public LayerMask ground_layer;

    //public Text groundText;
    //public Text distanceToEyesText;
    public float distanceToEyes;

    private bool ground, canJump, justJumped, isGrabbing, isGrappling, hold, startHold;

    public bool canGrab = true;
    private float velX, velY, velZ;
    [HideInInspector]
    public float speed, distance;

    private RaycastHit groundHit, grab, wallCheck;
    private Vector3 groundNormal, slingPoint, pointVel, spawnPos;
    private float mouseHorizontal;
    private Rigidbody rb;
    private ConfigurableJoint cj;
    private SoftJointLimit limit;
    private GameObject ropeInstance;
    private LineRenderer lr;
    private GameObject swing;
    private Vector3 currentRotation;
    [HideInInspector]
    public Vector3 currentDirection, targetDirection;

    private Rigidbody grabbedObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPos = gameObject.transform.position;
        dragVec = dragPoint.transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateCameraHorizontal();

        getGroundNormal();

        //store the current velocity direction
        currentDirection = rb.velocity.normalized;

        //calculate the current s p e e d
        speed = rb.velocity.magnitude;

        //check the values for all the buttons
        CheckInputs();

        //throw the spear on Q press
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowSpear();
        }

        if (Input.GetMouseButtonUp(1) || !canGrab)
        {
            EndAllGrab();
        }

        if (Input.GetMouseButton(1) && canGrab && !isGrabbing && !isGrappling)
        {
            StartGrab();
            StartGrapple();
        }

        if (Input.GetMouseButton(1) && canGrab && isGrabbing)
        {
            if (Input.GetMouseButton(0) && !startHold && !hold)
            {
                Debug.Log("begin holding?");
                StartCoroutine(Hold());
                startHold = true;
            }

            if (Input.GetMouseButtonUp(0) && !hold && startHold)
            {
                Debug.Log("nope. fire away!");
                grabbedObject.transform.position = spearThrowTrans.position;
                grabbedObject.AddForce(cam.forward * throwForce);
                canGrab = false;
                StartCoroutine(grabDelay());
                EndAllGrab();
                hold = false;
                startHold = false;
            }

            if (Input.GetMouseButton(0) && hold)
            {
                Debug.Log("yup, holding it.");
                dragPoint.transform.position = spearThrowTrans.position;
                grabbedObject.transform.localScale = Vector3.one;
            }
            else if(Input.GetMouseButtonUp(0) && hold)
            {
                Debug.Log("okay. stop.");
                hold = false;
                dragPoint.transform.localPosition = dragVec;
                grabbedObject.transform.localScale = Vector3.one * grabDownSize;
            }
        }

        

        //set velocity from right and left keys
        velX = rightKey - leftKey;
        velZ = upKey - downKey;

        targetDirection = transform.TransformVector(velX , 0, velZ);
        targetDirection = Vector3.ProjectOnPlane(targetDirection, groundNormal);

        if (ground && Input.GetKeyDown(KeyCode.Space) || canJump && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //UIHandler();
    }

    private void FixedUpdate()
    {
        if (isGrabbing && !isGrappling)
        {
            HoldGrab();
        }

        if (isGrappling && !isGrabbing)
        {
            HoldGrapple();
        }

        /*
        //wallrun left
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.right), out wallCheckL, wallCheckDist + 0.5f, ground_layer))
        {
            //need to be made local to player transform
                targetDirection = new Vector3(-0.1f, 1, 1);
                Debug.Log("wallrun left");
        }

        //wallrun right
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out wallCheckL, wallCheckDist + 0.5f, ground_layer))
        {
            targetDirection = new Vector3(0.1f, 1, 1);
                Debug.Log("wallrun right");
        }
        */

        WallRun();
        

        if (speed < maxSpeed)
        {
            rb.AddForce(targetDirection.normalized * Time.deltaTime * moveVel, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 current = rb.velocity.normalized;
            rb.AddForce((targetDirection - current) * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            //StartCoroutine(FrictionlessTime());
            justJumped = false;
        }

        //if it is a moving ship or something add force to the player to move along here

        if (collision.gameObject.tag == "death")
        {
            Death();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //you can keep jumping after disconnecting from a wall (unintended)
        if (collision.gameObject.tag == "ground" && !justJumped || collision.gameObject.tag == "spear" && !justJumped)
        {
            StartCoroutine(CanJumpDelay());
        }
    }

    void getGroundNormal()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out groundHit, 1.4f, ground_layer))
        {
            groundNormal = groundHit.normal;
        }
        else
        {
            groundNormal = Vector3.up;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & ground_layer) != 0)
        {
            ground = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & ground_layer) != 0)
        {
            ground = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & ground_layer) != 0)
        {
            ground = false;
        }
    }

    public void Death()
    {
        gameObject.transform.position = spawnPos;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpVel, rb.velocity.z);
        canJump = false;
        justJumped = true;
        ground = false;
    }

    void WallRun()
    {
        if (Physics.Raycast(transform.position - Vector3.up * 1.20f, targetDirection, out wallCheck, wallCheckDist + 0.5f, ground_layer))
        {
            if (ground)
            {
                rb.velocity = new Vector3(0, 6f, 0.5f);
            }
            else if(Physics.Raycast(transform.position - Vector3.up * 1f, targetDirection, out wallCheck, wallCheckDist + 0.5f, ground_layer))
            {
                rb.velocity = new Vector3(0, 6f, 0.5f);
            }
            
        }

    }

    void UpdateCameraHorizontal()
    {
        //handle camera and player rotation on the y axis
        mouseHorizontal = Input.GetAxis("Mouse X");
        currentRotation = this.transform.rotation.eulerAngles;
        currentRotation.y += mouseHorizontal * Time.deltaTime * sensitivity_x;
        this.transform.rotation = Quaternion.Euler(currentRotation);
    }

    void StartGrab()
    {
        if (Physics.Raycast(cam.position, cam.forward, out grab, grabRange, ground_layer))
        {
            if (grab.collider.tag == "Object")
            {
                grabbedObject = grab.collider.attachedRigidbody;

                //dragPoint.transform.position = grabbedObject.position;
                grabbedObject.transform.localScale = Vector3.one * grabDownSize;
                isGrabbing = true;
            }
        }
        else return;
    }

    void HoldGrab()
    {
        grabbedObject.velocity = ((dragPoint.transform.position - grabbedObject.position) * grabStrength);
        //grabbedObject.transform.position = Vector3.Slerp(dragPoint.transform.position, grabbedObject.transform.position, Time.deltaTime * 0.5f);
        grabbedObject.transform.rotation = Quaternion.Euler(Vector3.Slerp(dragPoint.transform.rotation.eulerAngles, grabbedObject.transform.rotation.eulerAngles, Time.deltaTime * 0.02f));
    }

    void StartGrapple()
    {
        if (Physics.Raycast(cam.position, cam.forward, out grab, 500, ground_layer))
        {
            if (grab.collider.tag == "ground")
            {

                grabbedObject = grab.collider.attachedRigidbody;


                if ((grabbedObject.transform.position - gameObject.transform.position).magnitude < grappleRange)
                {
                    isGrappling = true;

                    swing = Instantiate(swingPoint, grab.point, Quaternion.Euler(0, 0, 0));

                    cj = swing.GetComponent<ConfigurableJoint>();
                    cj.connectedBody = rb;

                    limit = new SoftJointLimit();
                    limit.limit = (gameObject.transform.position - grab.point).magnitude;
                    cj.linearLimit = limit;

                    CreateRope((cam.position - cam.up) + cam.forward, swing.transform.position);

                    //startcoroutine RopeStartDelay, wait 0.6s and if afterwards you touch ground EndAllGrab()


                    //Debug.Log((grabbedObject.transform.position - transform.position).magnitude);
                    if ((grabbedObject.transform.position - transform.position).magnitude < grappleRange)
                    {
                        //doesnt work because the blocks are one giant object right now
                    }
                }
            }
        }
    }

    void HoldGrapple()
    {
        if (grabbedObject.tag == "ground")
        {
            
            //If you want to change the limit over time do that here
            limit.limit -= Time.deltaTime * 5;
            cj.linearLimit = limit;

            if (limit.limit < 1f) //or if you touch ground after 0.6s of swinging (not implemented, but would be nice)
            {
                EndAllGrab();
            }
           
        }
        DrawRope((cam.position - cam.up) + cam.forward, swing.transform.position);
    }

    void CreateRope(Vector3 start, Vector3 end)
    {
        if (!lr)
        {
            ropeInstance = Instantiate(rope);
            lr = ropeInstance.GetComponent<LineRenderer>();
        }
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    void DrawRope(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    void EndAllGrab()
    {
        if (isGrabbing)
        {
            //Debug.Log("stop grab");
            dragPoint.transform.localPosition = dragVec;
            grabbedObject.transform.localScale = Vector3.one;
            grabKey = 0;
            isGrabbing = false;
        }

        if (isGrappling)
        {
            //Debug.Log("no grapple");

            isGrappling = false;

            if (cj != null)
            {
                cj.connectedBody = null;
            }

            Destroy(swing);
            lr = null;
            Destroy(ropeInstance);
        } 
    }

    void ThrowSpear()
    {
        GameObject lastSpear = Instantiate<GameObject>(spear, spearThrowTrans.position, spearThrowTrans.rotation);
        spearScript sc = lastSpear.GetComponent<spearScript>();
        sc.thrown = true;
        sc.thrownRot = spearThrowTrans.rotation;
        sc.throwSpeed = spearThrowSpeed;
        sc = null;
    }

    /*
    void UIHandler()
    {
        groundText.text = "speed: " + System.Math.Round(speed, 2);
        distanceToEyesText.text = "distance to eyes: " + System.Math.Round(distanceToEyes, 2);

    }
    */
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
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouchKey = 1f;
        }
        if (Input.GetMouseButtonDown(1))
        {
            grabKey = 0f;
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
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKey = 1f;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouchKey = 1f;
        }
        */
        if (Input.GetMouseButtonDown(1))
        {
            grabKey = 1f;
        }
        
    }

    //a delay so you dont immedtiately grab a thrown crate
    IEnumerator grabDelay()
    {
        yield return new WaitForSeconds(0.6f);
        canGrab = true;
    }

    //a delay so you can jump canJumpDelay of time after losing contact with the ground
    IEnumerator CanJumpDelay()
    {
        float duration = canJumpDelay;
        float totalTime = 0;

        while (totalTime <= duration && !justJumped)
        {
            canJump = true;
            totalTime += Time.deltaTime ;
            yield return null;
        }

        if (duration <= totalTime || justJumped)
        {
            canJump = false;
            yield return null;
        }
    }

    IEnumerator Hold()
    {
        float duration = 0.5f;
        float totalTime = 0;

        while (totalTime <= duration && Input.GetMouseButton(0))
        {
            hold = false;
            totalTime += Time.deltaTime;
            yield return null;
        }

        if (duration <= totalTime)
        {
            hold = true;
            startHold = false;
            yield return null;
        }
    }

    //a certain time where the friction is set to 0 after landing
    IEnumerator FrictionlessTime()
    {
        frictionMat.dynamicFriction = 0f;
        yield return new WaitForSeconds(frictionlessTime);
        frictionMat.dynamicFriction = friction;
    }
}