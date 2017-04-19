// Browser.cs
// Basic Columns view file browser for saveing and loading save games at runtime.
// Created Sep 17 by Charles Mehlenbeck
// Contact me here for any questions: http://forum.unity3d.com/members/27746-inventor2010
using UnityEngine;

using System;
using System.IO;

public class Browser : MonoBehaviour
{
    public int defaultWidth = 100;
    public int minWidth = 50;
    public GUISkin skin;
    
    bool WindowOpen = false;
    WindowType WinType;
    String MainFolder = "";
    String FileToSave = "";
    String NameOfFileToSave = "";
    Path[] PathTree = new Path[0];
    String[] OpenPaths = new String[0];
    String[] OpenPaths_Reduced = new String[0];
    String WindowName = "";
    Vector2 OuterScrollPosition = Vector2.zero;
    Vector2[] ScrollPositions = new Vector2[0];
    int[] Widths = new int[0];
    Drag DragStat;
    bool creatingFolder = false;
    String folderName = "";
    Vector2 minWindowScale = new Vector2(455, 150);
    
    static Rect BrowserRect = new Rect(20, 20, 500, 400);

    public void OpenFile(String mainFolder) 
    {
        if(Directory.Exists(mainFolder))
        {
            WindowOpen = true;
            WinType = WindowType.Open;
            DragStat.Dragging = false;    // Ensure nothing is being draged.
        
            MainFolder = mainFolder; // example: Application.dataPath/Saves;
            GetAllSubDirectoriesAndFiles( MainFolder );
            
            AddWidth();
            AddScrollPosition(0);
            AddPath(MainFolder, 0);
        }
        else
        {
            if(File.Exists(mainFolder)) Debug.LogError("I need a main directory, not a file!");
            else Debug.LogError("I need a path for a main directory that actually exists!");
        }
    }

    public void SaveFile(String FilePath, String mainFolder) 
    {
        if(Directory.Exists(mainFolder))
        {
            if(File.Exists(FilePath) || Directory.Exists(FilePath))
            {
                WindowOpen = true;
                WinType = WindowType.Save;
                DragStat.Dragging = false;    // Ensure nothing is being draged.
                
                FileToSave = FilePath;
                
                MainFolder = mainFolder; // example: Application.dataPath/Saves;
                GetAllSubDirectoriesAndFiles( MainFolder );
                
                AddWidth();
                AddScrollPosition(0);
                AddPath(MainFolder, 0);
            }
            else Debug.LogError("I need a path for a temporary save file that actually exists!");
        }
        else
        {
            if(File.Exists(mainFolder)) Debug.LogError("I need a main directory, not a file!");
            else Debug.LogError("I need a path for a main directory that actually exists!");
        }
    }
    
    void OnGUI()
    {
        if(WindowOpen)
        {
            GUI.skin = skin;
            BrowserRect = GUI.Window(0, BrowserRect, DrawBrowser, WindowName);
        }
    }
    
    void DrawBrowser(int windowID)
    {
        Event E = Event.current;
        Rect bottom = new Rect(5, BrowserRect.height-40, BrowserRect.width-10, 4);
        GUI.Box(bottom, "");
        bottom.x=10;    //Get bottom bar rect.
        bottom.y+=10;
        bottom.width = 120;
        bottom.height = 20;
        if(creatingFolder) // Folder creation tool.
        {
            folderName = GUI.TextField(bottom, folderName);
            bottom.x += bottom.width+5;
            bottom.width = 52;
            if(GUI.Button(bottom, "Create"))
            {
				String newDirectory;
				if( Directory.Exists(OpenPaths[OpenPaths.Length-1]) )
                	newDirectory = OpenPaths[OpenPaths.Length-1] + "/" + folderName;
				else
					newDirectory = OpenPaths[OpenPaths.Length-2] + "/" + folderName;
				
                if(!Directory.Exists(newDirectory)) 
                {
                    Directory.CreateDirectory(newDirectory);
                    GetAllSubDirectoriesAndFiles( MainFolder );
                }
                creatingFolder = false;
            }
        } else 
        if(GUI.Button(bottom, "Create New Folder")) 
        {
            folderName = "";
            creatingFolder = true;
        }
        
        bottom.width = 12; // Draw resize handle 
        bottom.height = 12;
        bottom.x = BrowserRect.width-bottom.width-5;
        bottom.y+=12;
        GUI.Box(bottom, "");
        Rect realBottom = bottom;
        realBottom.x += BrowserRect.x;
        realBottom.y += BrowserRect.y;
        if( Input.GetMouseButtonDown(0) && realBottom.Contains(GetMouseCord()) )
        {
            DragStat.Dragging = true;
            DragStat.startMouse = GetMouseCord();
            DragStat.startPosition = new Vector2(BrowserRect.width, BrowserRect.height);
            DragStat.type = DragType.WindowScaler;
        } else
        if(DragStat.Dragging && DragStat.type==DragType.WindowScaler)
        {
            Vector2 MouseCord = GetMouseCord();
            Vector2 newScale = DragStat.startPosition - new Vector2(    DragStat.startMouse.x - MouseCord.x,
                                                                        DragStat.startMouse.y - MouseCord.y);
            BrowserRect.width = newScale.x;
            BrowserRect.height = newScale.y;
            if(BrowserRect.width < minWindowScale.x) BrowserRect.width = minWindowScale.x;
            if(BrowserRect.height < minWindowScale.y) BrowserRect.height = minWindowScale.y;
        }
        
        // Draw Open/Save Cancel buttons, and make them function:
        bottom.y-=12;
        bottom.height = 20;
        bottom.width = 42;
        bottom.x -= bottom.width+5;
        if(WinType == WindowType.Open && GUI.Button(bottom, "Open"))
        {
            String FileToOpen = OpenPaths[OpenPaths.Length-1];
            if(File.Exists(FileToOpen) || Directory.Exists(FileToOpen))
            {
                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                //                                                                                                      //
                //    Run Your Open file code here! Have your open file code in a function that accepts a path (string) //
                //                                    and pass to it: FileToOpen.                                       //
                //                                                                                                      //
                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                Debug.Log("Opening File: " + FileToOpen);
            }
            else Debug.LogError("The File you are trying to open must have gotten moved, or deleated.");
            
            WindowOpen = false;
        }
        if(WinType == WindowType.Save && GUI.Button(bottom, "Save"))
        {
            if(File.Exists(FileToSave) || Directory.Exists(FileToSave))
            {
                //NameOfFileToSave
                String SaveLocation = OpenPaths[OpenPaths.Length-1];
                if(Directory.Exists( SaveLocation ))
                    OverwriteDuplicates(FileToSave, SaveLocation+"/"+NameOfFileToSave);
                else
                {
                    if(File.Exists(SaveLocation)) // If the user chooses a file instead of a folder the parent folder will be used.
                    {
                        SaveLocation = (new FileInfo(SaveLocation)).DirectoryName;
                        OverwriteDuplicates(FileToSave, SaveLocation+"/"+NameOfFileToSave);
                    }
                    else Debug.LogError("The directory you're trying to save in no longer exists");
                }
            }
            else Debug.LogError("The file/folder your trying to save no longer exists.");
            WindowOpen = false;
        }
        bottom.width = 52;
        bottom.x -= bottom.width+5;
        if(GUI.Button(bottom, "Cancel"))
        {
            if(WinType == WindowType.Save)
                
			{
				if(File.Exists(FileToSave))
					File.Delete(FileToSave); // Deleate FileToSave if the user chooses cancel.
				if(Directory.Exists(FileToSave))
					Directory.Delete(FileToSave, true); // Deleate FileToSave if the user chooses cancel.
				Debug.Log("Deleate: " + FileToSave);
			}
				//Directory.Delete(FileToSave, true); // Deleate the file to save if the user chooses cancel.
            WindowOpen = false;
        }
        if(WinType == WindowType.Save)
        {
            bottom.width = 130;
            bottom.x -= bottom.width+5;
            NameOfFileToSave = GUI.TextField(bottom, NameOfFileToSave);
        }
        
        //////////////////////////////////////////////////////////////////
        // The rest of OnGUI is to draw the columns part of the window. //
        //////////////////////////////////////////////////////////////////
        const int verticalSpace = 2;
        const int horiznontalSpace = 10;
        
        Rect OuterPosition = BrowserRect;
        OuterPosition.x=5;
        OuterPosition.y=20;
        OuterPosition.width-=10;
        OuterPosition.height-=67;
        Rect OuterViewRect = OuterPosition;
        
        OuterViewRect.height-=17;
        OuterViewRect.width = 0;
        for(int i=0; i<ScrollPositions.Length; ++i) OuterViewRect.width += horiznontalSpace+Widths[i];
        OuterViewRect.width += 15;
        //OuterPosition.width = 0;
        //for(int i=0; i<Widths.Length; ++i) OuterPosition.width += 12+Widths[i];
        
        // Makes scroll wheel on columns view scroll horizontally, as well allowing vertical scrolling in each column.
        if(!Input.GetMouseButton(0))
        {
            DragStat.Dragging = false;
            
            Rect realOuterPosition = OuterPosition;
            realOuterPosition.x += BrowserRect.x;
            realOuterPosition.y += BrowserRect.y;
            if( realOuterPosition.Contains(GetMouseCord()) && E.type == EventType.ScrollWheel )
                OuterScrollPosition.x += Mathf.Clamp(E.delta.x,-2f,2f)*20;
        }
        
        if(OuterPosition.width >= OuterViewRect.width) 
        {
            OuterPosition.height+=20;
            OuterViewRect.height+=20;
        }
        OuterScrollPosition = GUI.BeginScrollView (OuterPosition, OuterScrollPosition, OuterViewRect); 
        GUI.BeginGroup (OuterViewRect);
        
            for(int i=0; i<ScrollPositions.Length; ++i)
            {
                int ButtonHeight = 20;
                int[] PathIndexes = GetPathIndexesAt( OpenPaths[i] );
                Rect Position = GetRect(i, horiznontalSpace, (int)OuterViewRect.height);
                Rect viewRect = Position;
                viewRect.height = PathIndexes.Length*ButtonHeight + (PathIndexes.Length-1)*verticalSpace;
                viewRect.width -=17;
                viewRect.x = 0;
                viewRect.y = 0;
                // Make the buttons fill the space available:
                if(PathIndexes.Length*ButtonHeight + (PathIndexes.Length-1)*verticalSpace <= Position.height) viewRect.width += 15; 
                
                ScrollPositions[i] = GUI.BeginScrollView (Position, ScrollPositions[i], viewRect); // Draw Column i.
                    viewRect.height = ButtonHeight;
                    for(int j=0; j<PathIndexes.Length; ++j)
                    {
                        int index = PathIndexes[j];
                        
                        if(i+1<OpenPaths_Reduced.Length && PathTree[index].Name == OpenPaths_Reduced[i+1]) GUI.backgroundColor = Color.blue;
                        else GUI.backgroundColor = Color.white;
                        
                        Vector2 posbackup = ScrollPositions[i]; // Prevent it from scrolling to the top on button click.
                        if(GUI.Button(viewRect, PathTree[index].Name))
                        {
                            OuterScrollPosition.x = Mathf.Infinity; // Scroll to end of outer scroll view.
                            if(PathTree[index].File)
                            {
                                if(i==OpenPaths.Length-1) AddWidth();
                                AddScrollPosition(i);
                            }
                            else
                            {
                                if(i==OpenPaths.Length-1) AddWidth();
                                AddScrollPosition(i+1);
                            }
                            AddPath(PathTree[index].Parent + "/" + PathTree[index].Name, i+1);
                        }
                        ScrollPositions[i] = posbackup;
                        viewRect.y += ButtonHeight+verticalSpace;
                    }
                GUI.EndScrollView();
                
                Position.x += Position.width;            // Draw Column Resizer Bar:
                Position.width = horiznontalSpace-2;
                GUI.Box(Position, "");
                Rect realPosition = Position;            // Make it function:
                realPosition.x -= OuterScrollPosition.x - OuterPosition.x-2;
                //realPosition.y -= OuterScrollPosition.y;
                realPosition.x += BrowserRect.x;
                realPosition.y += BrowserRect.y;
                if( Input.GetMouseButtonDown(0) && realPosition.Contains(GetMouseCord()) )
                {
                    DragStat.Dragging = true;
                    DragStat.startMouse = Input.mousePosition;
                    DragStat.startPosition = new Vector2(Widths[i], 0);
                    DragStat.type = DragType.Column;
                    DragStat.column = i;
                } else
                if(DragStat.Dragging && DragStat.type==DragType.Column && DragStat.column==i)
                {
                    if(realPosition.x > BrowserRect.x+BrowserRect.width) BrowserRect.width = realPosition.x-BrowserRect.x;
                    int newWidth = (int)(DragStat.startPosition.x - DragStat.startMouse.x + Input.mousePosition.x);
                    if(newWidth < minWidth) newWidth=minWidth;
                    Widths[i] = newWidth;
                }
            }
        GUI.EndGroup ();
        GUI.EndScrollView();
        
        //GUI.Button(new Rect(10, 20, 400, 20), "Can't drag me");
        //GUI.DragWindow();
        
        if(Input.GetMouseButtonDown(0))
        {
            Rect topBar = BrowserRect;
            topBar.height = 15;
            if( topBar.Contains(GetMouseCord()) )
            {
                DragStat.Dragging = true;
                DragStat.startMouse = GetMouseCord();
                DragStat.startPosition = new Vector2(BrowserRect.x, BrowserRect.y);
                DragStat.type = DragType.Window;
            }
        }
        if(DragStat.Dragging && DragStat.type==DragType.Window)
        {
            Vector2 MouseCord = GetMouseCord();
            Vector2 newPos = DragStat.startPosition - new Vector2(    DragStat.startMouse.x - MouseCord.x,
                                                                    DragStat.startMouse.y - MouseCord.y);
            BrowserRect.x = newPos.x;
            BrowserRect.y = newPos.y;
        }
    }
    
    Rect GetRect(int index, int space, int height)
    {
        int x = 10;
        for(int i=0; i<index; ++i) x += space+Widths[i];
        return new Rect(x, 0, Widths[index], height);
    }
    
    int[] GetPathIndexesAt(string Parent)
    {
        int[] Temp = new int[PathTree.Length];
        int n = 0; // Number of paths found at Parent.
        for(int i=0; i<PathTree.Length; ++i)
        {
            if(System.IO.Path.GetFullPath(PathTree[i].Parent) == System.IO.Path.GetFullPath(Parent))
            {
                Temp[n] = i;
                n++;
            }
        }
        int[] OUT = new int[n];
        for(int i=0; i<OUT.Length; ++i)
        {
            OUT[i] = Temp[i];
        }
        return OUT;
    }
    
    void GetAllSubDirectoriesAndFiles(String MainDirectory)
    {
        PathTree = new Path[0];
        WalkTheTree( new DirectoryInfo(MainDirectory) );
    }

    void WalkTheTree(DirectoryInfo DI)
    {
        String PARENT = DI.FullName;
        DirectoryInfo[] diArr = DI.GetDirectories();
        FileInfo[] fiArr = DI.GetFiles();
        
        //Debug
        Path[] PathTreeTemp = new Path[ diArr.Length + fiArr.Length ];
        for (int i=0; i<diArr.Length; ++i)
        {
            //Debug.Log(diArr[i].Name);
            PathTreeTemp[i].Name = diArr[i].Name;
            PathTreeTemp[i].Parent = PARENT;
            PathTreeTemp[i].File = false;
        }
        int diArrLength = diArr.Length;
        for (int i=diArrLength; i<PathTreeTemp.Length; ++i)
        {
            PathTreeTemp[i].Name = fiArr[i-diArrLength].Name;
            PathTreeTemp[i].Parent = PARENT;
            PathTreeTemp[i].File = true;
        }
        
        PathTree = AddPathArrays( PathTree, PathTreeTemp);
        
        foreach (DirectoryInfo di in diArr)
            WalkTheTree( di );
    }
    
    
    
    void OverwriteDuplicates(String FTS, String FL)
    {
        if(Directory.Exists(FL)) Directory.Delete(FL, true);
        else if (File.Exists(FL)) File.Delete(FL);
        Directory.Move(FTS, FL);
    }
    
    void AddPath(String add, int indexOfAdd)
    {
        String[] Temp = new String[indexOfAdd+1];
        for(int i=0; i<indexOfAdd; ++i) Temp[i] = OpenPaths[i];
        Temp[Temp.Length-1] = add;
        OpenPaths = Temp;
        
        Temp = new String[indexOfAdd+1];
        for(int i=0; i<indexOfAdd; ++i) Temp[i] = OpenPaths_Reduced[i];
        Temp[Temp.Length-1] = (new DirectoryInfo(add)).Name;
        OpenPaths_Reduced = Temp;
        
        WindowName = ""; // Get the Name of the window.
        for(int i=0; i<OpenPaths_Reduced.Length; ++i) WindowName += "/"+OpenPaths_Reduced[i];
    }
    
    void AddWidth()
    {
        int[] Temp = new int[Widths.Length+1];
        Widths.CopyTo(Temp, 0);
        Temp[Temp.Length-1] = defaultWidth;
        Widths = Temp;
    }
    
    void AddScrollPosition(int indexOfAdd)
    {
        Vector2[] Temp = new Vector2[indexOfAdd+1];
        for(int i=0; i<indexOfAdd; ++i) Temp[i] = ScrollPositions[i];
        Temp[Temp.Length-1] = Vector2.zero;
        ScrollPositions = Temp;
    }
    
    Path[] AddPathArrays(Path[] IN1, Path[] IN2)
    {
        Path[] Temp = new Path[IN1.Length + IN2.Length];
        IN1.CopyTo(Temp, 0);
        IN2.CopyTo(Temp, IN1.Length);
        return Temp;
    }
    
    Vector2 GetMouseCord()
    {
        Vector2 mouse = Input.mousePosition;
        mouse.y = Screen.height-mouse.y;
        return mouse;
    }
}

struct Path
{
    public String Parent;
    public String Name;
    public bool File; // If false, it's a folder.
}

struct Drag
{
    public bool Dragging;
    public Vector2 startMouse;
    public Vector2 startPosition;
    public DragType type;
    public int column;
}

enum DragType
{
    Column,
    Window,
    WindowScaler
}

enum WindowType
{
    Open,
    Save
}