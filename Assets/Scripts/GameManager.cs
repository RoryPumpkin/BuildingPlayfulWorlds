using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera main;
    public Camera overview;

    public GameObject player;

    public Vector3 eyeFollow;

    public bool paused, startWithMenu = true;

    void Start()
    {
        if (startWithMenu)
        {
            player.SetActive(false);
            main.depth = 0;
            overview.depth = 1;
            main.gameObject.SetActive(false);
            paused = true;
            eyeFollow = overview.transform.position;
        }
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && startWithMenu)
        {
            eyeFollow = main.transform.position;
            main.gameObject.SetActive(true);
            main.depth = 2;
            overview.depth = 0;
            overview.gameObject.SetActive(false);
            main.depth = 1;

            player.SetActive(true);

            paused = false;
        }
        else
        {
            eyeFollow = main.transform.position;
        }
    }
}
