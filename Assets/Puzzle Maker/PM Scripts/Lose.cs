using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;

public class Lose : MonoBehaviour
{
    public Texture dewaLogo, wedLogo, mascot_hayat, lose;
    public TimeSpan time, timeLimit = new TimeSpan(0, 0, 10); //10 sec timeout
    protected Stopwatch timer = new Stopwatch();

    void Update()
    {
        time = timer.Elapsed;
        UnityEngine.Debug.Log("COUNTDOWN: " + (timeLimit - time).Seconds.ToString());

        if (time > timeLimit)
        {
            UnityEngine.Debug.Log("timeout 10 secs");
            Application.LoadLevel(0);
        }

        ////load form screen on any mouse click
        //if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        //{
        //    //addData();
        //    Application.LoadLevel(0);
        //}
    }

    void Start()
    {
        timer.Start();
    }
    void OnGUI()
    {

        if (!mascot_hayat)
        {
            UnityEngine.Debug.LogError("Assign a Texture in the inspector.");
            return;
        }
        //logos
        GUI.DrawTexture(new Rect(Screen.width / 2 - 320, Screen.height - 350, 644, 300), wedLogo, ScaleMode.StretchToFill, true, 10.0F);
        //GUI.DrawTexture(new Rect(Screen.width - 800, 20, 780, 200), dewaLogo, ScaleMode.StretchToFill, true, 10.0F);

        // Wrap everything in the designated GUI Area
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 800, Screen.height / 2 - 600, 1600, 1080));
        GUILayout.BeginHorizontal();

        //HAYAT
        GUI.DrawTexture(new Rect(Screen.width / 2 - 700, Screen.height / 3 - 200, 506, 600), mascot_hayat, ScaleMode.StretchToFill, true, 10.0F);

        //YouLose.png
        GUI.DrawTexture(new Rect(Screen.width / 2 - 200, Screen.height / 3 - 100, 640, 480), lose, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

}