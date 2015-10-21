using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Difficulty : MonoBehaviour
{
    public Texture logo;

    void OnGUI()
    {
        GUI.contentColor = Color.black;

        //logo
        GUI.DrawTexture(new Rect(Screen.width / 2 - 175, 50, 400, 100), logo, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, 175, 400, 800));
        GUILayout.BeginVertical();

        GUI.Label(new Rect(10, 0, 200, 100),"Pick a Difficulty Level");

        if(GUI.Button(new Rect(10, 50, 200, 100), "EASY"))
        {
            Application.LoadLevel("Easy"); //load Easy scene
        }

        if( GUI.Button(new Rect(10, 200, 200, 100), "HARD"))
        {
            Application.LoadLevel("Hard"); //load Hard scene
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void Update()
    {   
        //load form screen on any mouse click
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            Application.LoadLevel(0);
    }

}