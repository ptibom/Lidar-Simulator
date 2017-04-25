using UnityEngine;
using System.Collections;

using System;
using System.IO;

public class FileBrowser
{
    //public 
    //Optional Parameters
    public string name = "File Browser"; //Just a name to identify the file browser with
                                         //GUI Options
    public GUISkin guiSkin; //The GUISkin to use
                            //public int layoutType{	get{	return layout;	}	} //returns the current Layout type
    public Texture2D fileTexture, directoryTexture, backTexture, driveTexture; //textures used to represent file types
    public GUIStyle backStyle, cancelStyle, selectStyle; //styles used for specific buttons
    public Color selectedColor = new Color(0.5f, 0.5f, 0.9f); //the color of the selected file
    public bool isVisible { get { return visible; } } //check if the file browser is currently visible
                                                      //File Options
    public string searchPattern = "*"; //search pattern used to find files
                                       //Output
    public FileInfo outputFile; //the selected output file
                                //Search
    public bool showSearch = true; //show the search bar
    public bool showPopUp;
    public bool searchRecursively = true; //search current folder and sub folders
                                          //Protected	
                                          //GUI
    protected Vector2 fileScroll = Vector2.zero, folderScroll = Vector2.zero, driveScroll = Vector2.zero;
    protected Color defaultColor;
    protected int layout;
    protected Rect guiSize;
    protected GUISkin oldSkin;
    protected bool visible = false;
    //Search
    //protected string saveBarString = ""; //string used in search bar
    protected string searchBarString = ""; //string used in search bar
    protected bool isSearching = false; //do not show the search bar if searching
                                        //File Information
    protected DirectoryInfo currentDirectory;
    protected FileInformation1[] files;
    protected DirectoryInformation1[] directories, drives;
    protected DirectoryInformation1 parentDir;
    protected bool getFiles = true, showDrives = false;
    protected int selectedFile = -1;
    //Threading
    protected float startSearchTime = 0f;
#if thread
	protected Thread t;
#endif

    //Constructors
    public FileBrowser(string directory, int layoutStyle, Rect guiRect) { currentDirectory = new DirectoryInfo(directory); layout = layoutStyle; guiSize = guiRect; }
    #if (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
		public FileBrowser(string directory,int layoutStyle):this(directory,layoutStyle,new Rect(0,0,Screen.width,Screen.height)){}
		public FileBrowser(string directory):this(directory,1){}
    #else
    public FileBrowser(string directory, int layoutStyle) : this(directory, layoutStyle, new Rect(Screen.width * 0.125f, Screen.height * 0.125f, Screen.width * 0.75f, Screen.height * 0.75f)) { }
    public FileBrowser(string directory) : this(directory, 0) { }
    #endif
    public FileBrowser(Rect guiRect) : this() { guiSize = guiRect; }
    public FileBrowser(int layoutStyle) : this(Directory.GetCurrentDirectory(), layoutStyle) { }
    public FileBrowser() : this(Directory.GetCurrentDirectory()) { }

    //set variables
    public void setDirectory(string dir) { currentDirectory = new DirectoryInfo(dir); }
    //public void setLayout(int l){	layout=l;	}
    public void setGUIRect(Rect r) { guiSize = r; }
    //gui function to be called during OnGUI
    public bool Draw()
    {
        if (getFiles)
        {
            GetFileList(currentDirectory);
            getFiles = false;
        }
        if (guiSkin)
        {
            oldSkin = GUI.skin;
            GUI.skin = guiSkin;
        }
        GUILayout.BeginArea(guiSize);
        GUILayout.BeginVertical("box");

        switch (layout)
        {
            case 0:
                GUILayout.BeginHorizontal("box");
                GUILayout.FlexibleSpace();
                GUILayout.Label("Saving to: " + currentDirectory.FullName + "\\" + searchBarString);
                GUILayout.FlexibleSpace();

                if (DrawSearchField())
                {
                    outputFile = null;
                    return true;
                }
                GUILayout.Space(10);

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal("box");
                GUILayout.BeginVertical(GUILayout.MaxWidth(300));
                folderScroll = GUILayout.BeginScrollView(folderScroll);
                if (showDrives)
                {
                    foreach (DirectoryInformation1 di in drives)
                    {
                        if (di.button()) { GetFileList(di.di); }
                    }
                }
                else
                {
                    if ((backStyle != null) ? parentDir.button(backStyle) : parentDir.button())
                        GetFileList(parentDir.di);
                }
                foreach (DirectoryInformation1 di in directories)
                {
                    if (di.button()) { GetFileList(di.di); }
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
                GUILayout.BeginVertical("box");
                if (isSearching)
                {
                    //drawSearchMessage();
                }
                else
                {
                    fileScroll = GUILayout.BeginScrollView(fileScroll);
                    for (int fi = 0; fi < files.Length; fi++)
                    {
                        if (selectedFile == fi)
                        {
                            defaultColor = GUI.color;
                            GUI.color = selectedColor;
                        }
                        if (files[fi].button())
                        {
                            outputFile = files[fi].fi;
                            selectedFile = fi;
                        }
                        if (selectedFile == fi)
                            GUI.color = defaultColor;
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.BeginHorizontal("box");
                GUILayout.FlexibleSpace();
                if ((cancelStyle == null) ? GUILayout.Button("Cancel") : GUILayout.Button("Cancel", cancelStyle))
                {
                    outputFile = null;
                    return true;
                }
                GUILayout.FlexibleSpace();
                if ((selectStyle == null) ? GUILayout.Button("Overwrite exiting file") : GUILayout.Button("Overwrite exiting file", selectStyle))
                {
                    Debug.Log("writing over :" + outputFile);
                    SaveFile(null + outputFile);
                    return true;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                break;
            case 1: //mobile preferred layout
            default:
                if (showSearch)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.FlexibleSpace();
                    DrawSearchField();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                fileScroll = GUILayout.BeginScrollView(fileScroll);

                if (isSearching)
                {
                    //drawSearchMessage();
                }
                else
                {
                    if (showDrives)
                    {
                        GUILayout.BeginHorizontal();
                        foreach (DirectoryInformation1 di in drives)
                        {
                            if (di.button()) { GetFileList(di.di); }
                        }
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        if ((backStyle != null) ? parentDir.button(backStyle) : parentDir.button())
                            GetFileList(parentDir.di);
                    }


                    foreach (DirectoryInformation1 di in directories)
                    {
                        if (di.button()) { GetFileList(di.di); }
                    }
                    for (int fi = 0; fi < files.Length; fi++)
                    {
                        if (selectedFile == fi)
                        {
                            defaultColor = GUI.color;
                            GUI.color = selectedColor;
                        }
                        if (files[fi].button())
                        {
                            outputFile = files[fi].fi;
                            selectedFile = fi;
                        }
                        if (selectedFile == fi)
                            GUI.color = defaultColor;
                    }
                }
                GUILayout.EndScrollView();

                if ((selectStyle == null) ? GUILayout.Button("Overwrite exiting file") : GUILayout.Button("Overwrite exiting file", selectStyle)) { Debug.Log(outputFile); return true; }

                if ((cancelStyle == null) ? GUILayout.Button("Cancel") : GUILayout.Button("Cancel", cancelStyle))
                {
                    outputFile = null;
                    return true;
                }
                break;
                case 2:
                GUILayout.BeginHorizontal("box");
                GUILayout.FlexibleSpace();
                GUILayout.Label(currentDirectory.FullName);
                GUILayout.FlexibleSpace();
                if (showSearch)
                {
                    DrawSearchField();
                    GUILayout.Space(10);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal("box");
                GUILayout.BeginVertical(GUILayout.MaxWidth(300));
                folderScroll = GUILayout.BeginScrollView(folderScroll);
                if (showDrives)
                {
                    foreach (DirectoryInformation1 di in drives)
                    {
                        if (di.button()) { GetFileList(di.di); }
                    }
                }
                else
                {
                    if ((backStyle != null) ? parentDir.button(backStyle) : parentDir.button())
                        GetFileList(parentDir.di);
                }
                foreach (DirectoryInformation1 di in directories)
                {
                    if (di.button()) { GetFileList(di.di); }
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
                GUILayout.BeginVertical("box");

                fileScroll = GUILayout.BeginScrollView(fileScroll);
                for (int fi = 0; fi < files.Length; fi++)
                {
                    if (selectedFile == fi)
                    {
                        defaultColor = GUI.color;
                        GUI.color = selectedColor;
                    }
                    if (files[fi].button())
                    {
                        outputFile = files[fi].fi;
                        selectedFile = fi;
                    }
                    if (selectedFile == fi)
                        GUI.color = defaultColor;
                }
                GUILayout.EndScrollView();

                GUILayout.BeginHorizontal("box");
                GUILayout.FlexibleSpace();
                if ((cancelStyle == null) ? GUILayout.Button("Cancel") : GUILayout.Button("Cancel", cancelStyle))
                {
                    outputFile = null;
                    return true;
                }
                GUILayout.FlexibleSpace();
                if ((selectStyle == null) ? GUILayout.Button("Select") : GUILayout.Button("Select", selectStyle)) { return true; }
                    SaveFile(currentDirectory + "\\" + searchBarString);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    break;
                 }
        
                GUILayout.EndVertical();
                GUILayout.EndArea();
                if (guiSkin) { GUI.skin = oldSkin; }
                return false;
        }
    protected bool DrawSearchField()
    {
        searchBarString = GUILayout.TextField(searchBarString, GUILayout.MinWidth(150));
        if (GUILayout.Button("save"))
        {
            if (searchBarString.Length > 0)
            {
                Debug.Log(currentDirectory + "\\" + searchBarString);
                SaveFile(currentDirectory + "\\" + searchBarString);
                return true;
            }
            else
            {
                GetFileList(currentDirectory);
                Debug.Log("current dir :" + currentDirectory);
            }
        }
        return false;
    }

    public void GetFileList(DirectoryInfo di)
    {
        //set current directory
        currentDirectory = di;
        //get parent
        if (backTexture)
            parentDir = (di.Parent == null) ? new DirectoryInformation1(di, backTexture) : new DirectoryInformation1(di.Parent, backTexture);
        else
            parentDir = (di.Parent == null) ? new DirectoryInformation1(di) : new DirectoryInformation1(di.Parent);
        showDrives = di.Parent == null;

        //get drives
        string[] drvs = System.IO.Directory.GetLogicalDrives();
        drives = new DirectoryInformation1[drvs.Length];
        for (int v = 0; v < drvs.Length; v++)
        {
            drives[v] = (driveTexture == null) ? new DirectoryInformation1(new DirectoryInfo(drvs[v])) : new DirectoryInformation1(new DirectoryInfo(drvs[v]), driveTexture);
        }

        //get directories
        DirectoryInfo[] dia = di.GetDirectories();
        directories = new DirectoryInformation1[dia.Length];
        for (int d = 0; d < dia.Length; d++)
        {
            if (directoryTexture)
                directories[d] = new DirectoryInformation1(dia[d], directoryTexture);
            else
                directories[d] = new DirectoryInformation1(dia[d]);
        }

        //get files
        FileInfo[] fia = di.GetFiles(searchPattern);
        files = new FileInformation1[fia.Length];
        for (int f = 0; f < fia.Length; f++)
        {
            if (fileTexture)
                files[f] = new FileInformation1(fia[f], fileTexture);
            else
                files[f] = new FileInformation1(fia[f]);
        }
    }
    public void SaveFile(string filename)
    {
        string filetosave = filename;
        try
        {
            /**
 
            StreamWriter sr = File.CreateText(filetosave );
           
            sr.WriteLine("This is the existing file which has been ovewritten.");
            sr.WriteLine("testing the save actually works.");

            sr.Close();
    */

            //LidarStorage ls =  GameObject.Find("LidarSensor").GetComponent<LidarSensor>().GetLidarStorage();
            //Debug.Log("Size ls data: " + ls.GetData().Count);
            //SaveManager.SaveToCsv(ls.GetData(), filename);
        }
        catch (IOException e)
        {
            Debug.Log("Access violation, printing file.");
        }
    }



    //search a directory by a search pattern, this is optionally recursive
    public static FileInfo[] searchDirectory(DirectoryInfo di, string sp, bool recursive)
    {
        return di.GetFiles(sp, (recursive) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
    }
    public static FileInfo[] searchDirectory(DirectoryInfo di, string sp)
    {
        return searchDirectory(di, sp, false);
    }

    public float brightness(Color c) { return c.r * .3f + c.g * .59f + c.b * .11f; }

    //to string
    public override string ToString()
    {
        return "Name: " + name + "\nVisible: " + isVisible.ToString() + "\nDirectory: " + currentDirectory + "\nLayout: " + layout.ToString() + "\nGUI Size: " + guiSize.ToString() + "\nDirectories: " + directories.Length.ToString() + "\nFiles: " + files.Length.ToString();
    }
}

public class FileInformation1
{
    public FileInfo fi;
    public GUIContent gc;

    public FileInformation1(FileInfo f)
    {
        fi = f;
        gc = new GUIContent(fi.Name);
    }
public FileInformation1(FileInfo f, Texture2D img)
    {
        fi = f;
        gc = new GUIContent(fi.Name, img);
    }

    public bool button() { return GUILayout.Button(gc); }
    public void label() { GUILayout.Label(gc); }
    public bool button(GUIStyle gs) { return GUILayout.Button(gc, gs); }
    public void label(GUIStyle gs) { GUILayout.Label(gc, gs); }
    }

public class DirectoryInformation1
{
    public DirectoryInfo di;
    public GUIContent gc;

    public DirectoryInformation1(DirectoryInfo d)
    {
        di = d;
        gc = new GUIContent(d.Name);
    }

    public DirectoryInformation1(DirectoryInfo d, Texture2D img)
    {
        di = d;
        gc = new GUIContent(d.Name, img);
    }

    public bool button() { return GUILayout.Button(gc); }
    public void label() { GUILayout.Label(gc); }
    public bool button(GUIStyle gs) { return GUILayout.Button(gc, gs); }
    public void label(GUIStyle gs) { GUILayout.Label(gc, gs); }
}