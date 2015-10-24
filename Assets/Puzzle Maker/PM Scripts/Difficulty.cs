using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Difficulty : MonoBehaviour
{
    public Texture dewaLogo, wedLogo, mascot_noor, mascot_hayat;

    private float _oldWidth;
    private float _oldHeight;
    private float _fontSize = 30;
    public float EffectiveFontSizePercent = 0.1f; // value between 0 and 1

    void OnGUI()
    {
        GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = GUI.skin.textField.fontSize = (int)_fontSize;


        GUI.contentColor = Color.white;

        //logos
        GUI.DrawTexture(new Rect(Screen.width / 2 - 320, Screen.height - 350, 644, 300), wedLogo, ScaleMode.StretchToFill, true, 10.0F);
        //GUI.DrawTexture(new Rect(Screen.width - 800, 20, 780, 200), dewaLogo, ScaleMode.StretchToFill, true, 10.0F);

        // GUILayout.BeginArea(new Rect(Screen.width / 2 - 175, 175, 400, 800));

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 800, Screen.height / 2 - 600, 1600, 1080));

        GUILayout.BeginHorizontal();

        //NOOR
        GUI.DrawTexture(new Rect(0, Screen.height / 2 - 200, 450, 600), mascot_noor, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.BeginVertical();
        GUI.Label(new Rect(Screen.width / 3 - 50, Screen.height / 3 - 100, 400, 100), "Pick a Difficulty Level");

        if (GUI.Button(new Rect(Screen.width / 3 - 50, Screen.height / 3 , 400, 150), "KIDS"))
        {
            Application.LoadLevel(2); //load Easy scene
        }

        if (GUI.Button(new Rect(Screen.width / 3 - 50, Screen.height / 3 + 200, 400, 150), "ADULTS"))
        {
            Application.LoadLevel(3); //load Hard scene
        }
        GUILayout.EndVertical();

        //HAYAT
        GUI.DrawTexture(new Rect(Screen.width / 2 + 200, Screen.height / 2 - 200, 506, 600), mascot_hayat, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

}