using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform spearThrowTrans;
    public float cameraHeight = 0.75f;
    public PlayerController script;

    private float rotY;
    private float sens;
    
    void Start()
    {
        sens = script.sensitivity_x;
    }

    
    void Update()
    {
        rotY -= Input.GetAxis("Mouse Y") * Time.deltaTime * sens;

        this.transform.position = new Vector3(player.position.x, player.position.y + cameraHeight, player.position.z);
        this.transform.rotation = player.rotation;

        this.transform.rotation = Quaternion.Euler(rotY, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z);
        spearThrowTrans.rotation = Quaternion.Euler(rotY + 90, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z);
    }
}
