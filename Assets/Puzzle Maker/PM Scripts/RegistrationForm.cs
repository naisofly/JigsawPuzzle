using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class RegistrationForm : MonoBehaviour
{
    public string enteredName = "";
    public string enteredPhone = "";
    public string enteredEmail = "";

    public Texture dewaLogo, wedLogo, mascot_noor, mascot_hayat;

    private float _oldWidth;
    private float _oldHeight;
    private float _fontSize = 30;
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

        GUI.contentColor = Color.white;
        if (!dewaLogo)
        {
            Debug.LogError("Assign a Texture in the inspector.");
            return;
        }

        //logos
        GUI.DrawTexture(new Rect(Screen.width / 2 - 320, Screen.height - 350, 644, 300), wedLogo, ScaleMode.StretchToFill, true, 10.0F);
        //GUI.DrawTexture(new Rect(Screen.width - 800, 20, 780, 200), dewaLogo, ScaleMode.StretchToFill, true, 10.0F);

        // Wrap everything in the designated GUI Area
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 800, Screen.height / 2 - 600, 1600, 1080));

        GUILayout.BeginHorizontal();

        //NOOR
        GUI.DrawTexture(new Rect(0, Screen.height / 2 - 200, 450, 600), mascot_noor, ScaleMode.StretchToFill, true, 10.0F);

        // Begin the singular Vertical Group
        GUILayout.BeginVertical();
        // GUILayout.Label("DEWA PUZZLE");
        GUI.Label(new Rect(Screen.width / 3 - 50, Screen.height / 3 - 100, 400, 50), "SOLVE THE PUZZLE!");

        GUI.Label(new Rect(Screen.width / 3 - 50, Screen.height / 3, 400, 50), "Name");
        enteredName = GUI.TextField(new Rect(Screen.width / 3 - 50, Screen.height / 3 + 50, 400, 50), enteredName, 25);
        GUI.Label(new Rect(Screen.width / 3 - 50, Screen.height / 3 + 100, 400, 50), "Phone Number");
        enteredPhone = GUI.TextField(new Rect(Screen.width / 3 - 50, Screen.height / 3 + 150, 400, 50), enteredPhone, 25);
        //GUI.Label(new Rect(Screen.width / 3-50, Screen.height / 3 + 200, 400, 50), "Email");
        //enteredEmail = GUI.TextField(new Rect(Screen.width / 3-50, Screen.height / 3 + 250, 400, 50), enteredEmail, 25);
        // GUILayout.Space(20);
        if (GUI.Button(new Rect(Screen.width / 3 - 50, Screen.height / 3 + 250, 400, 100), "PLAY"))
        {
            addData();
            Application.LoadLevel(1);
        }
        // End the Groups and Area
        GUILayout.EndVertical();

        //HAYAT
        GUI.DrawTexture(new Rect(Screen.width / 2 + 200, Screen.height / 2 - 200, 506, 600), mascot_hayat, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    public void addData()
    {
        // Following line adds data to CSV file
        File.AppendAllText("DewaJigsawPuzzle_Data/PlayerData.csv", "\n" + enteredName
            + "," + enteredPhone + "," + "...@...");
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