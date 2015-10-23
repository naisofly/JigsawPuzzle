using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Picture : MonoBehaviour
{
    public Texture logo, mascot_noor, mascot_hayat;
    public Texture [] images;

    private float _oldWidth;
    private float _oldHeight;
    private float _fontSize = 20;
    public float EffectiveFontSizePercent = 0.1f; // value between 0 and 1


    //void Update()
    //{
    //    //RESPONSIVE GUI FONT SIZE
    //    if (_oldWidth != Screen.width || _oldHeight != Screen.height)
    //    {
    //        _oldWidth = Screen.width;
    //        _oldHeight = Screen.height;
    //        _fontSize = Mathf.Min(Screen.width, Screen.height) * EffectiveFontSizePercent;
    //        GetComponent<GUIText>().fontSize = (int)_fontSize;
    //    }
    //}

    void OnGUI()
    {

        GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = GUI.skin.textField.fontSize = (int)_fontSize;

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
        GUI.Label(new Rect(350, 0, 300, 100),"Pick an Image");

        //GUI.DrawTexture(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 250, 100, 100), images[0], ScaleMode.StretchToFill, true, 10.0F);
        GUI.Button(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 250, 100, 100), images[0]);

        GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2 - 250 , 157,   100), images[1], ScaleMode.StretchToFill, true, 10.0F);

        GUI.DrawTexture(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 125,   113, 100), images[2], ScaleMode.StretchToFill, true, 10.0F);

        GUI.DrawTexture(new Rect(Screen.width / 2 +50, Screen.height / 2 - 125, 100, 100), images[3], ScaleMode.StretchToFill, true, 10.0F);

        GUI.DrawTexture(new Rect(Screen.width / 2 - 200, Screen.height / 2 , 150, 100), images[4], ScaleMode.StretchToFill, true, 10.0F);

        GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 200, 100), images[5], ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.EndVertical();

        //hayat mascot
        GUI.DrawTexture(new Rect(730, 0, 270, 380), mascot_hayat, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

}