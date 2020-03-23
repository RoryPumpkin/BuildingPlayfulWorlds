using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboardScript : MonoBehaviour
{
    private GameManager gm;

    private Vector3 follow;

    void Start()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }


    void Update()
    {
        follow = gm.eyeFollow;
        //transform.rotation = Quaternion.FromToRotation(new Vector3(0, -1f, 0), follow - transform.position);
        transform.LookAt(follow, Vector3.up);
    }
}