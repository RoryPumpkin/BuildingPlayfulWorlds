using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantShapeBehaviour : MonoBehaviour
{
    public enum States { explosive, bouncy, jumpy}
    public States type;

    private SkinnedMeshRenderer smr;
    private Renderer r;
    private BoxCollider bc;

    public float shrinkStep;
    public float growStep;

    public float grow = 60;
    public float spreadTop = 90;
    public float spreadBottom = 100;

    public float dotTransparency;
    
    void Start()
    {
        smr = gameObject.GetComponent<SkinnedMeshRenderer>();
        r = gameObject.GetComponent<Renderer>();
        bc = gameObject.GetComponent<BoxCollider>();
    }

    
    void Update()
    {
        if (r.isVisible)
        {
            if (grow < 60)
            {
                grow += growStep;
            }

            if (spreadTop > 0 && grow > 30 && spreadBottom < 90)
            {
                spreadTop -= growStep;
            }
            else if(grow > 50 && spreadTop < 90)
            {
                spreadTop += growStep;
            }

            if (spreadBottom < 100 && grow > 30)
            {
                spreadBottom += growStep;
            }

            if (dotTransparency < 0.6f && grow > 40)
            {
                dotTransparency += 0.1f;
            }
        }
        else
        {
            if (spreadTop > 0)
            {
                spreadTop -= shrinkStep;
            }
        }

        if (spreadTop > 60)
        {
            bc.enabled = true;
        }

        smr.SetBlendShapeWeight(0, grow);
        smr.SetBlendShapeWeight(1, spreadTop);
        smr.SetBlendShapeWeight(2, spreadBottom);


        /* semi old method, I prefer just putting all the blendshapes to the zero state when you can't see it
        if (r.isVisible)
        {
            if (grow < 100)
            {
                grow += growStep;
            }

            if(spreadTop < 100)
            {
                spreadTop += growStep;
            }

            if (spreadBottom < 100)
            {
                spreadBottom += growStep;
            }
        }
        else
        {
            if (grow > 0)
            {
                grow -= shrinkStep;
            }

            if (spreadTop > 0)
            {
                spreadTop -= shrinkStep;
            }

            if (spreadBottom > 0)
            {
                spreadBottom -= shrinkStep;
            }
        }
        */
    }

    private void OnBecameVisible()
    {
        //Debug.Log("you can see me");
    }

    private void OnBecameInvisible()
    {
        grow = 0;
        //spreadBottom = 0;
        spreadTop = 80;
        dotTransparency = 0;
    }

    /* old method that works weirdly cause there is no will not render equivalent
    private void OnWillRenderObject()
    {
        if(Camera.current.name == "Main Camera")
        {
            print(gameObject.name + " is being rendered by " + Camera.current.name);

            if (grow < 100)
            {
                grow += growStep;
            }

            if (spreadTop < 100)
            {
                spreadTop += growStep;
            }

            if (spreadBottom < 100)
            {
                spreadBottom += growStep;
            }
        }
    }
    */
}
