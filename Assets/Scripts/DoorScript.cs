using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private GameManager gm;
    public static bool called;

    void Start()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !called)
        {
            called = true;
            gm.LevelEnd();
            //Loader.Load(Loader.Scene.level1);
        }
    }
}
