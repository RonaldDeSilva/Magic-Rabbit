using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class InGameTimer : MonoBehaviour
{

    public static InGameTimer instance; 
    public Text timeCounter;
    private TimeSpan timePlaying;   
    private bool timerGoing;

    private float elapsedTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeCounter.text = "Time: 00:00.00";
        timerGoing = false;

    }
    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }
    public void EndTimer()
    {
        timerGoing = false;
    }                                   

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingStr;
            yield return null;
        }
    }

}

//Need to be added to calulated the time while the player is still alive.
//Once the player dies/wins, stop the timer and display the final time on the screen.
//Add to the player script to call BeginTimer() when the game starts and EndTimer() when the player dies/wins.
//Note: The timer is not added yet 
