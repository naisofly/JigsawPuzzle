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
        GUI.DrawTexture(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 585, 150), logo, ScaleMode.StretchToFill, true, 10.0F);
        //GUI.DrawTexture(new Rect(Screen.width / 2 - 175, 50, 400, 100), logo, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.BeginArea(new Rect(Screen.width/2 - 350, 150, 900, 800));
        GUILayout.BeginHorizontal();

        //mascot
        GUI.DrawTexture(new Rect(0, 0, 270, 380), mascot, ScaleMode.StretchToFill, true, 10.0F);
        //YouWin.png
        GUI.DrawTexture(new Rect(270, 20, 480, 360), win, ScaleMode.StretchToFill, true, 10.0F);

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
	void Start()
	{
		readData();
	}
    void Update()
    {
        
        //load form screen on any mouse click
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            //addData();
            Application.LoadLevel(0);
        }
    }

    private void readData()
    {
        Debug.Log("read winner data"); Debug.Log(getPath());
        //string[] records = File.ReadAllLines(getPath() + "/PlayerData.csv"); //read all players
		string[] records = File.ReadAllLines("DewaJigsawPuzzle_Data/PlayerData.csv"); //read all players
        
        string winner = records[records.Length - 1]; //read winner (last record)

        string[] details = winner.Split(','); //winner details

        string enteredName = details[0];
        string enteredPhone = details[1];
        string enteredEmail = details[2];

        Debug.Log("write winner data");
        addData(enteredName, enteredPhone, enteredEmail);

    }

    public void addData(string enteredName, string enteredPhone, string enteredEmail)
    {
        // Following line adds data to CSV file
        //File.AppendAllText(getPath() + "/GameApp_DataWinners.csv", enteredName + "\n" );
		File.AppendAllText("DewaJigsawPuzzle_Data/GameApp_DataWinners.csv", enteredName + "\n" );

        Debug.Log("Winner Data written");
    }

    // Get path for given CSV file
    private static string getPath()
    {
        return Application.dataPath;
    }
}