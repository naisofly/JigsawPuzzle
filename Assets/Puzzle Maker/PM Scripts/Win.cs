using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Win : MonoBehaviour
{
    public Texture logo, mascot, win;

    void OnGUI()
    {
        
        if (!mascot)
        {
            Debug.LogError("Assign a Texture in the inspector.");
            return;
        }
        //logo
        GUI.DrawTexture(new Rect(Screen.width/2 - 175, 50, 400, 100), logo, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.BeginArea(new Rect(Screen.width/2 - 350, 150, 900, 800));
        GUILayout.BeginHorizontal();

        //mascot
        GUI.DrawTexture(new Rect(0, 0, 270, 380), mascot, ScaleMode.StretchToFill, true, 10.0F);
        //YouWin.png
        GUI.DrawTexture(new Rect(270, 20, 480, 360), win, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void Update()
    {   
        //load form screen on any mouse click
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            Application.LoadLevel(0);
    }
}