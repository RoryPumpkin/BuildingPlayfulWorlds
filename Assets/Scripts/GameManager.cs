using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera main;
    public Camera overview;
    public GameObject ui;

    public GameObject player;
    public bool exit = false;

    public Vector3 eyeFollow;

    public bool paused, startWithMenu = true;

    void Start()
    {
        if (startWithMenu)
        {
            Cursor.lockState = CursorLockMode.Confined;
            ui.SetActive(false);
            player.SetActive(false);
            main.depth = 0;
            overview.depth = 1;
            main.gameObject.SetActive(false);
            paused = true;
            eyeFollow = overview.transform.position;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && startWithMenu)
        {
            FirstPerson();
        }
        else
        {
            eyeFollow = main.transform.position;
        }
    }

    public static void FirstPerson()
    {
        GameManager gm = (GameManager)FindObjectOfType(typeof(GameManager));

        gm.eyeFollow = gm.main.transform.position;
        gm.main.gameObject.SetActive(true);
        gm.main.depth = 2;
        gm.overview.depth = 0;
        gm.overview.gameObject.SetActive(false);
        gm.main.depth = 1;

        gm.player.SetActive(true);
        gm.ui.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        gm.paused = false;
    }
}
