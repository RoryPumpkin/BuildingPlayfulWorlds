using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    private GameManager gm;
    public Slider slider;


    private void Start()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    public void Play()
    {
        if (GameManager.nextLevel)
        {
            int val = SceneManager.GetActiveScene().buildIndex + 1;
            if(val == 4)
            {
                val = 0;
            }
            Loader.nextScene = (Loader.Scene)val;
            Loader.Load(Loader.nextScene);
            GameManager.nextLevel = false;
        }

        gm.FirstPerson();
    }

    public void Restart()
    {
        gm.Respawn();
        gm.FirstPerson();
        //int val = SceneManager.GetActiveScene().buildIndex;
        //Loader.nextScene = (Loader.Scene)val;
        //Loader.Load(Loader.nextScene);
    }

    public void doExitGame()
    {
        Application.Quit();
    }

    public void ChangeSensitivity()
    {
        gm.pc.sensitivity_x = gm.pc.sensitivity_y = slider.value * 200;
    }
}
