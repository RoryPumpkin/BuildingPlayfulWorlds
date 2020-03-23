using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeFX : MonoBehaviour
{

    private Rigidbody rb;
    private GameObject copy;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        copy = Instantiate(gameObject, gameObject.transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        gameObject.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        rb.AddForce(new Vector3(5, 3, 0));
        copy.GetComponent<Rigidbody>().AddForce(new Vector3(-5, 3, 0));
        copy.GetComponent<SmokeFX>().enabled = false;
    }

    
    void Update()
    {
        if (gameObject.transform.localScale.x > 0)
        {
            gameObject.transform.localScale -= Vector3.one * 0.01f;
            copy.transform.localScale -= Vector3.one * 0.01f;
        }
    }
}
