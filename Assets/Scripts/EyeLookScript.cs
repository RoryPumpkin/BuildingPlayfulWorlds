using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLookScript : MonoBehaviour
{
    public GameManager gm;

    private Vector3 follow;
    
    void Start()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    
    void Update()
    {
        follow = gm.eyeFollow;
        transform.rotation = Quaternion.Lerp( transform.rotation,Quaternion.FromToRotation(Vector3.forward, follow - transform.position), Time.deltaTime * 1f);
    }
}
