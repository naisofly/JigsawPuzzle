using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Difficulty : MonoBehaviour
{
    public Texture logo, mascot_noor, mascot_hayat;

    void OnGUI()
    {
        GUI.contentColor = Color.black;

        //logo
        GUI.DrawTexture(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 585, 150), logo, ScaleMode.StretchToFill, true, 10.0F);
        //GUI.DrawTexture(new Rect(Screen.width / 2 - 175, 50, 400, 100), logo, ScaleMode.StretchToFill, true, 10.0F);

        // GUILayout.BeginArea(new Rect(Screen.width / 2 - 175, 175, 400, 800));

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 500, 200, 1000, 800));

        GUILayout.BeginHorizontal();

        //noor mascot
        GUI.DrawTexture(new Rect(0, 0, 270, 380), mascot_noor, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.BeginVertical();
        GUI.Label(new Rect(350, 20, 300, 100),"Pick a Difficulty Level");

        if (GUI.Button(new Rect(350, Screen.height/2 - 250, 300, 100), "KIDS"))
        {
            Application.LoadLevel(2); //load Easy scene
        }

        if( GUI.Button(new Rect(350, Screen.height / 2 - 50, 300, 100), "ADULTS"))
        {
            Application.LoadLevel(3); //load Hard scene
        }
        GUILayout.EndVertical();

        //hayat mascot
        GUI.DrawTexture(new Rect(730, 0, 270, 380), mascot_hayat, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

}