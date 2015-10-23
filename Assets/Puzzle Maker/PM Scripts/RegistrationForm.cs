using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class RegistrationForm : MonoBehaviour
{
    public string enteredName = "";
    public string enteredPhone = "";
    public string enteredEmail = "";

    public Texture logo, mascot_noor, mascot_hayat;

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

        //if (_oldWidth != Screen.width || _oldHeight != Screen.height)
        //{
        //    _oldWidth = Screen.width;
        //    _oldHeight = Screen.height;
        //    _fontSize = Mathf.Min(Screen.width, Screen.height) * EffectiveFontSizePercent;
        //    GUI.skin.label.fontSize = (int)_fontSize;
        //}

        //GUI.skin.label.font = GUI.skin.button.font = GUI.skin.box.font = font;
        GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = GUI.skin.textField.fontSize = (int)_fontSize;

        GUI.contentColor = Color.black;
        if (!logo)
        {
            Debug.LogError("Assign a Texture in the inspector.");
            return;
        }

        //logo
        GUI.DrawTexture(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 585, 150), logo, ScaleMode.StretchToFill, true, 10.0F);
        //GUI.DrawTexture(new Rect(Screen.width / 2 - 175, 50, 400, 100), logo, ScaleMode.StretchToFill, true, 10.0F);

        // Wrap everything in the designated GUI Area
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 500, 200, 1000, 800));

        GUILayout.BeginHorizontal();

        //noor mascot
        GUI.DrawTexture(new Rect(0, 0, 270, 380), mascot_noor, ScaleMode.StretchToFill, true, 10.0F);


        // Begin the singular Vertical Group
        GUILayout.BeginVertical();
        // GUILayout.Label("DEWA PUZZLE");
        GUILayout.Space(20);
        GUI.Label(new Rect(350, 0, 300, 30),"Enter Your Name");
        enteredName = GUI.TextField(new Rect(350, 40, 300, 30), enteredName, 25);
        GUI.Label(new Rect(350, 80, 300, 30), "Enter Your Phone Number");
        enteredPhone = GUI.TextField(new Rect(350, 120, 300, 30), enteredPhone, 25);
        GUI.Label(new Rect(350, 160, 300, 30), "Enter Your Email");
        enteredEmail = GUI.TextField(new Rect(350, 200, 300, 30), enteredEmail, 25);
       // GUILayout.Space(20);
        if (GUI.Button(new Rect(350, 300, 300, 50), "PLAY"))
        {
            addData();
            Application.LoadLevel(1);
        }
        // End the Groups and Area
        GUILayout.EndVertical();

        //hayat mascot
        GUI.DrawTexture(new Rect(730, 0, 270, 380), mascot_hayat, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    public void addData()
    {
        // Following line adds data to CSV file
        File.AppendAllText("DewaJigsawPuzzle_Data/PlayerData.csv", "\n" + enteredName 
            + "," + enteredPhone + "," + enteredEmail);
        //UnityEditor.AssetDatabase.Refresh();
        //readData(); // send player data to next game scene then win/lose scene
       // Debug.Log(enteredPhone);
    }

    // Get path for given CSV file
    private static string getPath()
    {
    //#if UNITY_EDITOR
        return Application.dataPath;
    //#elif UNITY_ANDROID
    //return Application.persistentDataPath;// +fileName;
    //#elif UNITY_IPHONE
    //return GetiPhoneDocumentsPath();// +"/"+fileName;
    //#else
    //return Application.dataPath;// +"/"+ fileName;
    //#endif
    }

}