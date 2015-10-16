using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class RegistrationForm : MonoBehaviour
{
    public string enteredName = "Firstname Lastname";
    public string enteredPhone = "+971551234567";
    public string enteredEmail = "email@example.com";

    public Texture logo;

    void OnGUI()
    {
        GUI.contentColor = Color.black;
        if (!logo)
        {
            Debug.LogError("Assign a Texture in the inspector.");
            return;
        }

        GUI.DrawTexture(new Rect(200, 100, 400, 100), logo, ScaleMode.StretchToFill, true, 10.0F);

        // Wrap everything in the designated GUI Area
        GUILayout.BeginArea(new Rect(275, 250, 200, 400));

        // Begin the singular Vertical Group
        GUILayout.BeginVertical();


        // GUILayout.Label("DEWA PUZZLE");
        GUILayout.Space(20);

        GUILayout.Label("Enter Your Name");
        enteredName = GUILayout.TextField(enteredName, 25);

        GUILayout.Label("Enter Your Phone Number");
        enteredPhone = GUILayout.TextField(enteredPhone, 25);

        GUILayout.Label("Enter Your Email");
        enteredEmail = GUILayout.TextField(enteredEmail, 25);

        GUILayout.Space(20);
        if (GUILayout.Button("PLAY"))
        {
            addData();
            Application.LoadLevel(1);
        }

        // End the Groups and Area
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    public void addData()
    {
        // Following line adds data to CSV file
        File.AppendAllText(getPath() + "/PlayerData.csv", "\n" + enteredName 
            + "," + enteredPhone + "," + enteredEmail);
        //UnityEditor.AssetDatabase.Refresh();
        //readData(); // send player data to next game scene then win/lose scene
        Debug.Log(enteredPhone);
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