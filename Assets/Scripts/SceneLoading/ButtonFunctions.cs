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
        if (gm.nextLevel)
        {
            
            Debug.Log("You wanna play the next level?");

            int val = SceneManager.GetActiveScene().buildIndex + 1;

            Loader.nextScene = (Loader.Scene)val;
            Loader.Load(Loader.nextScene);

            gm.nextLevel = false;
        }
        else
        {
            gm.FirstPerson();
        }
    }

    public void Restart()
    {
        gm.nextLevel = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
