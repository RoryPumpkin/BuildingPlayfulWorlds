using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboardScript : MonoBehaviour
{
    private GameManager gm;

    public float interpolationAmount;

    private Vector3 follow;

    void Start()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }


    void Update()
    {
        follow = gm.eyeFollow;

        if (interpolationAmount != -1)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(follow - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * interpolationAmount);
        }
        else
        {
            transform.LookAt(follow, Vector3.up);
        }
    }
}