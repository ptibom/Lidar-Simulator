using UnityEngine;
using System.Collections;

using System;
using System.IO;
public class TestFileBrowser : MonoBehaviour
{
    public ExportManager exportManager;
    Texture2D file, folder, back, drive;
    FileBrowser fb = new FileBrowser();
    bool active = false;
    public BrowseState state;




    string output = "no file";

    // Use this for initialization
    void Start()
    {
        exportManager = GetComponent<ExportManager>();
        fb.fileTexture = file;
        fb.directoryTexture = folder;
        fb.backTexture = back;
        fb.driveTexture = drive;
        //show the search bar
        fb.showSearch = true;
        FileBrowser.ToggleFileBrowser += ToggleFileBrowser;
        //search recursively (setting recursive search may cause a long delay)
        fb.searchRecursively = true;
        fb.SetExportManager(exportManager); // sets the export manager in the filebrowser
        
        if(state == BrowseState.Open)
        {
            fb.SetState(FileBrowser.BrowseState.Open);
        } else
        {
            fb.SetState(FileBrowser.BrowseState.Save);

        }
        FileBrowser.ToggleFileBrowser += Disable;
    }

    void OnDestroy()
    {
        FileBrowser.ToggleFileBrowser -= ToggleFileBrowser;
        FileBrowser.ToggleFileBrowser -= Disable;
    }

    void OnGUI()
    {
        if (active)
        {
            if (fb.showConfirm)
            {

                GUI.BeginGroup(new Rect((Screen.width + 300) / 2, (Screen.height - 100) / 2, 200, 100));

                GUI.Box(new Rect(0, 0, 200, 100), "");
                GUI.Label(new Rect(10, 10, 200, 100), "Do you want to save?");
                if (GUI.Button(new Rect(30, 40, 60, 30), "Cancel")) {
                    fb.showConfirm = false;
                };
                if (GUI.Button(new Rect(100, 40, 60, 30), "Save")) {
                    fb.SaveFile();
                    fb.showConfirm = false;
                    ToggleFileBrowser();
                };
                GUI.EndGroup();
            }
            if (fb.showExit)
            {

                GUI.BeginGroup(new Rect((Screen.width + 200) / 2, (Screen.height - 100) / 2, 200, 100));

                GUI.Box(new Rect(0, 0, 200, 100), "");
                GUI.Label(new Rect(10, 10, 200, 100), "Do you want to Exit?");
                if (GUI.Button(new Rect(30, 40, 60, 30), "Cancel")) { fb.showExit = false; };
                if (GUI.Button(new Rect(100, 40, 60, 30), "Exit")) {
                    fb.showExit = false;
                    ToggleFileBrowser();
                };

                GUI.EndGroup();
            }




            if (fb.Draw())
            { //true is returned when a file has been selected
              //the output file is a member if the FileInfo class, if cancel was selected the value is null

                //output = (fb.outputFile == null) ? "cancel hit" : fb.outputFile.ToString();
                //gameObject.SetActive(false);

            }
        }
    }

    public enum BrowseState {
        Open,Save
    }

    public void Disable()
    {
        //this.enabled = false;
    }

    public void ToggleFileBrowser()
    {
        if (active)
        {
            active = false;
        }
        else
        {
            active = true;
        }
    }

}
