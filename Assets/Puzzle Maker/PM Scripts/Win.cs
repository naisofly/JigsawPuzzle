using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;

public class Win : MonoBehaviour
{
    public Texture dewaLogo, wedLogo, mascot_noor, win;
    public TimeSpan time, timeLimit = new TimeSpan(0, 0, 10); //10 sec timeout
    protected Stopwatch timer = new Stopwatch();

    void Update()
    {
        time = timer.Elapsed;
        UnityEngine.Debug.Log("COUNTDOWN: "+ (timeLimit - time).Seconds.ToString());

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

    void OnGUI()
    {

        if (!mascot_noor)
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

        //NOOR
        GUI.DrawTexture(new Rect(Screen.width / 2 - 700, Screen.height / 3 - 200, 450, 600), mascot_noor, ScaleMode.StretchToFill, true, 10.0F);

        //YouWin.png
        GUI.DrawTexture(new Rect(Screen.width / 2 - 200, Screen.height / 3 - 100, 640, 480), win, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void Start()
    {
        timer.Start();
        saveData();
    }

    private void saveData()
    {
        string[] records = File.ReadAllLines("DewaJigsawPuzzle_Data/PlayerData.csv"); //read all players

        string winner = records[records.Length - 1]; //read winner (last record)

        string[] details = winner.Split(','); //winner details

        string enteredName = details[0];
        string enteredPhone = details[1];
        string enteredEmail = details[2];

        addData(enteredName, enteredPhone, enteredEmail);
    }

    public void addData(string enteredName, string enteredPhone, string enteredEmail)
    {
        // Following line adds data to CSV file
        //File.AppendAllText(getPath() + "/GameApp_DataWinners.csv", enteredName + "\n" );
        File.AppendAllText("DewaJigsawPuzzle_Data/GameApp_DataWinners.csv", enteredName + "\n");

    }

    // Get path for given CSV file
    private static string getPath()
    {
        return Application.dataPath;
    }
}