using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform spearThrowTrans;
    public float cameraHeight = 1.25f;
    public PlayerController script;
    public GameObject eye;
    public static bool overview;
    public static bool switchedView;

    public static Quaternion spawnRot;

    private float rotY;
    private float sens;

    public Vector3 overviewOffset = new Vector3(20, 15, 40);
    public static Vector3 overviewPos;
    
    void Start()
    {
        sens = script.sensitivity_x;
        overviewPos = overviewOffset;
        spawnRot = transform.rotation;
    }

    
    void Update()
    {
        sens = script.sensitivity_x;

        if (!overview && !switchedView)
        {
            rotY -= Input.GetAxis("Mouse Y") * Time.deltaTime * sens;

            transform.localPosition = Vector3.up * (cameraHeight - 0.5f);
            this.transform.rotation = player.rotation;

            this.transform.rotation = Quaternion.Euler(rotY, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z);
            spearThrowTrans.rotation = Quaternion.Euler(rotY + 90, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z);
        }

        if (switchedView)
        {
            

            if (overview)
            {
                if (eye != null)
                {
                    eye.GetComponent<SimpleFollowScript>().Pause();
                }
                GameManager.paused = true;
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PlayerController.currentState = PlayerController.States.pause;
                transform.position = Vector3.Slerp(transform.position, overviewPos, Time.deltaTime * 15);

                Quaternion lookOnLook = Quaternion.LookRotation(GameManager.midPoint - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 15);

                if (transform.position == overviewPos)
                {
                    switchedView = false;
                }
            }
            else
            {
                if (GameManager.endMenu)
                {
                    GameManager.endMenu = false;
                    //GameManager.paused = false;
                    if(eye != null)
                    {
                        eye.transform.position = eye.GetComponent<SimpleFollowScript>().spawnPos;
                    }
                    gameObject.transform.rotation = spawnRot;
                    GameManager.startTime = Time.time;
                    PlayerController.currentState = PlayerController.States.control;
                    DoorScript.called = false;
                }
                else
                {
                    GameManager.paused = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    PlayerController.currentState = PlayerController.States.control;
                    transform.position = Vector3.Slerp(transform.position, player.transform.position + (Vector3.up * (cameraHeight - 0.5f)), Time.deltaTime * 100);
                    if (Vector3.Distance(transform.position, player.transform.position + (Vector3.up * (cameraHeight - 0.5f))) < 1f)
                    {
                        transform.position = player.transform.position + (Vector3.up * (cameraHeight - 0.5f));
                        switchedView = false;
                    }
                }
            }
        }
    }


}
