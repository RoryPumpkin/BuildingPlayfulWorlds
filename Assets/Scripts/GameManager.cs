using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public GameObject ui;
    public GameObject menu;
    public TextMeshProUGUI endTime;
    public TextMeshProUGUI timer;
    public GameObject record;

    public GameObject eye;
    public GameObject player;
    public GameObject door;
    public Camera cam;
    public static Vector3 midPoint;
    private CameraController cc;
    public PlayerController pc;
    public static bool exit = false;
    public static bool nextLevel;
    public static float startTime;
    private float lastFastestTime;
    public static float completionTime;

    private AudioSource confetti, ding;

    public Vector3 eyeFollow;

    public bool startMenu;
    public static bool paused;
    public static bool pauseFrame, unPauseFrame;
    public static bool endMenu;

    void Start()
    {
        confetti = GetComponents<AudioSource>()[1];
        ding = GetComponents<AudioSource>()[2];

        pc = player.GetComponent<PlayerController>();
        cc = cam.transform.parent.GetComponent<CameraController>();

        startTime = Time.time;
        endTime.gameObject.transform.parent.gameObject.SetActive(false);
        exit = false;

        if (startMenu)
        {
            Pause();
        }
        else
        {
            FirstPerson();
        }
    }


    void Update()
    {
        eyeFollow = player.transform.position;

        if (paused)
        {
            
        }
        else
        {
            timer.text = (Time.time - startTime).ToString();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                FirstPerson();
            }
            else
            {
                Pause();
            }
        }
    }

    public void LevelEnd()
    {
        nextLevel = true;
        completionTime = Time.time - startTime;
        float endtime = completionTime;
        endMenu = true;
        ui.SetActive(false);
        menu.SetActive(true);
        endTime.text = endtime.ToString();
        endTime.gameObject.transform.parent.gameObject.SetActive(true);

        Debug.Log(lastFastestTime + " previous fastest");

        if (lastFastestTime > endtime && lastFastestTime != 0)
        {
            record.SetActive(true);
            pc.Confetti();
            confetti.Play();
            Debug.Log("record!! " + endtime);
        }
        else
        {
            ding.Play();
        }

        lastFastestTime = endtime;

        CameraController.overview = true;
        CameraController.switchedView = true;
        CameraController.overviewPos = cam.transform.position + cc.overviewOffset;
        cc.transform.parent = null;
    }

    public void Pause()
    {
        midPoint = player.transform.position + (door.transform.position - player.transform.position) * 0.5f;
        startTime = Time.time;
        ui.SetActive(false);
        menu.SetActive(true);
        record.SetActive(false);

        CameraController.overview = true;
        CameraController.switchedView = true;
        CameraController.overviewPos = cam.transform.position + cc.overviewOffset;
        cc.transform.parent = null;
    }

    public void FirstPerson()
    {
        menu.SetActive(false);
        ui.SetActive(true);
        endTime.gameObject.transform.parent.gameObject.SetActive(false);
        record.SetActive(false);

        CameraController.overview = false;
        CameraController.switchedView = true;
        cc.transform.parent = player.transform;
    }

    public void Respawn()
    {
        pc.Respawn(false);
        eye.transform.position = eye.GetComponent<SimpleFollowScript>().spawnPos;
    }

}
