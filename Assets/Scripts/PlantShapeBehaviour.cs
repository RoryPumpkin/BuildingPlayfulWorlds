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
    private AudioSource audioExplosion, audioCrackle;
    private bool activated;
    public bool visible;

    public float shrinkStep;
    public float growStep;

    public float grow = 60;
    public float spreadTop = 90;
    public float spreadBottom = 100;

    public float plantJumpMultiplier = 1.3f;
    public float explosionForce = 500, explosionRadius = 10;

    public float dotTransparency;
    
    void Start()
    {
        smr = gameObject.GetComponent<SkinnedMeshRenderer>();
        r = gameObject.GetComponent<Renderer>();
        bc = gameObject.GetComponent<BoxCollider>();
        audioExplosion = gameObject.GetComponents<AudioSource>()[0];
        audioCrackle = gameObject.GetComponents<AudioSource>()[1];
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

            if (r.material.color.a < 3 && grow > 50 && type == States.explosive)
            {
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, r.material.color.a + (Time.deltaTime * 5));
            }
            
        }
        else
        {
            if (spreadTop > 0)
            {
                spreadTop -= shrinkStep;
            }
        }

        if (spreadTop > 60 && grow > 50)
        {
            bc.enabled = true;
            activated = false;
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
        visible = true;
    }

    private void OnBecameInvisible()
    {
        visible = false;

        if (spreadTop > 80 && grow > 50)
        {
            audioExplosion.Play();
            activated = true;
        }

        r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, 0f);

        grow = 0;
        spreadTop = 80;
        dotTransparency = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!visible && type == States.explosive && other.gameObject.tag == "Player" && activated)
        {
            Rigidbody temp_rb = other.gameObject.GetComponent<Rigidbody>();
            temp_rb.AddExplosionForce(explosionRadius, gameObject.transform.position, explosionRadius);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerController>().justJumped && type == States.jumpy)
        {
            Rigidbody temp_rb = collision.gameObject.GetComponent<Rigidbody>();
            temp_rb.velocity = new Vector3(temp_rb.velocity.x, temp_rb.velocity.y * plantJumpMultiplier, temp_rb.velocity.z);
        }
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
