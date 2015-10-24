using UnityEngine;
using System;
using System.Diagnostics;
using System.Threading;

public class Timer : MonoBehaviour
{
    public TimeSpan timeLimit = new TimeSpan(0, 3, 0);
    protected Boolean timeOut = false;

    protected Stopwatch timer = new Stopwatch();
    protected TimeSpan time, remaining;
    void Start()
    {
        timer.Start();
    }

    void Update()
    {
        time = timer.Elapsed;
        remaining = timeLimit - time;

        if (time > timeLimit)
        {
            timer.Reset();
            timeOut = true; //OUT OF TIME - LOSE SCREEN
            Application.LoadLevel(5);
        }
    }

    void OnApplicationQuit()
    {
        timer.Stop();
        time = timer.Elapsed;
        //UnityEngine.Debug.Log(timeOut);
    }

    void OnGUI()
    {
        // GUI.Label(new Rect(350, 100, 100, 20), ("ELAPSED \t" + time.Minutes.ToString() + ":" + time.Seconds.ToString()));
        GUI.Button(new Rect(Screen.width - 200, 0, 200, 80), ("TIME \t" + remaining.Minutes.ToString() + ":" + remaining.Seconds.ToString() + " "));
        //GUI.Button(new Rect(0, 0, Screen.width, 80), (Application.dataPath));
        //GUI.Button(new Rect(0, 100, Screen.width, 80), (Application.dataPath+ "/dewa/" + "0" + ".jpg"));

        UnityEngine.Debug.Log("DATAPATH: " + Application.dataPath);
        UnityEngine.Debug.Log("filepath: " + Application.streamingAssetsPath);
    }
}