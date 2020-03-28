using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDots : MonoBehaviour
{
    private PlantShapeBehaviour psb;
    private Material m;
    private Material def;
    
    void Start()
    {
        psb = transform.parent.gameObject.GetComponent<PlantShapeBehaviour>();
        m = GetComponent<MeshRenderer>().material;
        def = m;
    }

    
    void Update()
    {
        m.color = new Color(def.color.r, def.color.g, def.color.b, psb.dotTransparency);
    }
}
