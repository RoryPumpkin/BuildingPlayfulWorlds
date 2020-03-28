using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public enum Scene
    {
        level1, Playground
    }

    public void SwitchToPlayground()
    {
        Debug.Log("Now Switchin...");
        Loader.Load(Loader.Scene.Playground);

        GameManager.FirstPerson();
        Debug.Log("Now Switchin... but its after loading");
    }

    
}
