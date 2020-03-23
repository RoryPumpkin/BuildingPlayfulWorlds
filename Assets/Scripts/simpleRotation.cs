using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleRotation : MonoBehaviour
{

    public float rotateSpeed;

    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 rot = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(rot.x, rot.y + rotateSpeed, rot.z);
    }
}
