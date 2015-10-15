using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(JPPuzzleController))]
public class EJPPuzzleController : Editor {

    string[] FileNames = null;

    public void OnEnable()
    {
        FileNames = System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/", "*.pm");
    }

    public override void OnInspectorGUI()
    {

        JPPuzzleController myTarget = (JPPuzzleController)target;

        EditorGUILayout.Space();

        myTarget.UseFilePath = EditorGUILayout.Toggle("Use file", myTarget.UseFilePath);

        if (myTarget.UseFilePath)
        {
            //Populate files data in combobox
            
            if (FileNames == null)
            {
                EditorGUILayout.HelpBox("Currently no file present, please create file using PMFileCreator from Window->PuzzleMaker->CreatePMFile", MessageType.Error);
            }
            else if ( FileNames.Length == 0 )
            {
                EditorGUILayout.HelpBox("Currently no file present, please create file using PMFileCreator from Window->PuzzleMaker->CreatePMFile", MessageType.Error);
            }
            else if ( FileNames.Length > 0 ){
                
                GUIContent[] _contentList = new GUIContent[FileNames.Length];

                for (int i = 0; i < FileNames.Length; i++)
                {
                    _contentList[i] = new GUIContent( System.IO.Path.GetFileName( FileNames[i] ) );
                }

                myTarget._selectedFileIndex = EditorGUILayout.Popup(new GUIContent( "PM File" ), myTarget._selectedFileIndex, _contentList);

                myTarget.PMFilePath = FileNames[myTarget._selectedFileIndex];
            }

        }
        else
        {
            base.OnInspectorGUI();
        }



        EditorUtility.SetDirty(target);

    }

}
