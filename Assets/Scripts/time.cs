using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time : MonoBehaviour
{

    private int seconds;

    private int sec;
    private int min;

    void Start()
    {
        
    }

    
    void Update()
    {
        seconds = Mathf.RoundToInt(Time.time);

        min = Mathf.RoundToInt(seconds / 60);
        sec = seconds - (min * 60);

        Debug.Log(min + ":" + sec);
    }
}
