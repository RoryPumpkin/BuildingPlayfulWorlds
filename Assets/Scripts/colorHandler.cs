using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorHandler : MonoBehaviour
{

    public Color startColor;

    private MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        mr = this.gameObject.GetComponent<MeshRenderer>();
        mr.material.color = startColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
