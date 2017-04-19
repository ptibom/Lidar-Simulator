using UnityEngine;
using System.Collections;

using System;
using System.IO;
public class TestFileBrowser : MonoBehaviour
{
    Texture2D file, folder, back, drive;
    FileBrowser fb = new FileBrowser();
    string output = "no file";

    // Use this for initialization
    void Start()
    {
        fb.fileTexture = file;
        fb.directoryTexture = folder;
        fb.backTexture = back;
        fb.driveTexture = drive;
        //show the search bar
        fb.showSearch = true;
        
        //search recursively (setting recursive search may cause a long delay)
        fb.searchRecursively = true;
    }


    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        
        GUILayout.EndVertical();
        GUILayout.Space(80);
        //GUILayout.Label("Overwrite exiting file: " + output);
        GUILayout.EndHorizontal();
        //draw and display output
      
        if (fb.Draw())
        { //true is returned when a file has been selected
          //the output file is a member if the FileInfo class, if cancel was selected the value is null

            output = (fb.outputFile == null) ? "cancel hit" : fb.outputFile.ToString();
            gameObject.SetActive(false);

        }
    }
}
