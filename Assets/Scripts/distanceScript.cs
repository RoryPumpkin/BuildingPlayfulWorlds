using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distanceScript : MonoBehaviour
{
    public GameObject player;
    private float minDist;
    public float maxDist;

    private float dist;
    private Material m;
    private PlayerController pc;
    
    void Start()
    {
        m = GetComponent<MeshRenderer>().material;
        pc = player.GetComponent<PlayerController>();
        minDist = pc.grappleRange;
    }

    
    void Update()
    {
        dist = (gameObject.transform.position - player.transform.position).magnitude;

        float d = Mathf.Clamp(dist, minDist, maxDist);

        if (dist < minDist)
        {
            m.color = new Color(0f, 0.8f, 0.2f, 1 - (d / 255));
        }
        else
        {
            m.color = new Color(1, 1, 1, 1 - (d / 255));
        }
        
    }
}
