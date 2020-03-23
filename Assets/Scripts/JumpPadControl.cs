using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadControl : MonoBehaviour
{
    public PlayerController pc;
    public bool canGrab = false;
    public float launchForce = 20;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            pc.canGrab = false;
            StartCoroutine(grabDelay());
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * launchForce, ForceMode.VelocityChange);
        }
        else if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * launchForce, ForceMode.VelocityChange);
        }
        
    }

    IEnumerator grabDelay()
    {
        yield return new WaitForSeconds(0.6f);
        pc.canGrab = true;
    }
}
