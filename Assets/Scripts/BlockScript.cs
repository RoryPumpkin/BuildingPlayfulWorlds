using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public Transform player;

    public float minDist, maxDist, minDistS, maxDistS;
    public float so, yo;

    private float dist, distS;
    private MeshRenderer mesh;
    private Vector3 startPos;
    
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.enabled = true;
        startPos = gameObject.transform.position;
        //gameObject.transform.localScale = Vector3.one;
    }

    
    void Update()
    {
        //add distances from spear and player together as dist value then clamp
        dist = ((player.position + player.forward * 4f) - gameObject.transform.position).magnitude;

        float d = Mathf.Clamp(dist, minDist, maxDist);
        //distS = Mathf.Clamp(dist, minDistS, maxDistS);

        if (d < maxDist)
        {
            mesh.enabled = true;

            gameObject.transform.position = new Vector3(startPos.x, startPos.y + ((d - minDist) * yo), startPos.z);
            float s = ((-d + maxDist) / (maxDist-minDist)) * so;
            //float s = 1 - (distS - minDistS);
            gameObject.transform.localScale = new Vector3(s, s, s);
        }
        else
        {
            mesh.enabled = false;
        }

        
    }
}
