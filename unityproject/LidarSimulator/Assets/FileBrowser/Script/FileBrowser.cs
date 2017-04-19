#define thread //comment out this line if you would like to disable multi-threaded search
using UnityEngine;
using System.Collections;
using System.IO;
#if thread
using System.Threading;
#endif


public class FileBrowser{
//public 
	//Optional Parameters
	public string name = "File Browser"; //Just a name to identify the file browser with
	//GUI Options
	public GUISkin guiSkin; //The GUISkin to use
	//public int layoutType{	get{	return layout;	}	} //returns the current Layout type
	public Texture2D fileTexture,directoryTexture,backTexture,driveTexture; //textures used to represent file types
	public GUIStyle backStyle,cancelStyle,selectStyle; //styles used for specific buttons
	public Color selectedColor = new Color(0.5f,0.5f,0.9f); //the color of the selected file
	public bool isVisible{	get{	return visible;	}	} //check if the file browser is currently visible
	//File Options
	public string searchPattern = "*"; //search pattern used to find files
	//Output
	public FileInfo outputFile; //the selected output file
	//Search
	public bool showSearch = true; //show the search bar
	public bool searchRecursively = true; //search current folder and sub folders
//Protected	
	//GUI
	protected Vector2 fileScroll=Vector2.zero,folderScroll=Vector2.zero,driveScroll=Vector2.zero;
	protected Color defaultColor;
	protected int layout;
	protected Rect guiSize;
	protected GUISkin oldSkin;
	protected bool visible = false;
	//Search
	protected string searchBarString = ""; //string used in search bar
	protected bool isSearching = false; //do not show the search bar if searching
	//File Information
	protected DirectoryInfo currentDirectory;
	protected FileInformation[] files;
	protected DirectoryInformation[] directories,drives;
	protected DirectoryInformation parentDir;
	protected bool getFiles = true,showDrives=false;
	protected int selectedFile = -1;
	//Threading
	protected float startSearchTime = 0f;
	#if thread
	protected Thread t;
	#endif
	
	//Constructors
	public FileBrowser(string directory,int layoutStyle,Rect guiRect){	currentDirectory = new DirectoryInfo(directory);	layout = layoutStyle;	guiSize = guiRect;	}
	#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
		public FileBrowser(string directory,int layoutStyle):this(directory,layoutStyle,new Rect(0,0,Screen.width,Screen.height)){}
		public FileBrowser(string directory):this(directory,1){}
	#else
		public FileBrowser(string directory,int layoutStyle):this(directory,layoutStyle,new Rect(Screen.width*0.125f,Screen.height*0.125f,Screen.width*0.75f,Screen.height*0.75f)){}
		public FileBrowser(string directory):this(directory,0){}
	#endif
	public FileBrowser(Rect guiRect):this(){	guiSize = guiRect;	}
	public FileBrowser(int layoutStyle):this(Directory.GetCurrentDirectory(),layoutStyle){}
	public FileBrowser():this(Directory.GetCurrentDirectory()){}
	
	//set variables
	public void setDirectory(string dir){	currentDirectory=new DirectoryInfo(dir);	}
	//public void setLayout(int l){	layout=l;	}
	public void setGUIRect(Rect r){	guiSize=r;	}
	
	
	//gui function to be called during OnGUI
	public bool draw(){
		if(getFiles){
			getFileList(currentDirectory); 
			getFiles=false;
		}
		if(guiSkin){
			oldSkin = GUI.skin;
			GUI.skin = guiSkin;
		}
		GUILayout.BeginArea(guiSize);
		GUILayout.BeginVertical("box");
		switch(layout){
			case 0:
				GUILayout.BeginHorizontal("box");
					GUILayout.FlexibleSpace();
					GUILayout.Label(currentDirectory.FullName);
					GUILayout.FlexibleSpace();
					if(showSearch){
						drawSearchField();
						GUILayout.Space(10);
					}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal("box");
					GUILayout.BeginVertical(GUILayout.MaxWidth(300));
						folderScroll = GUILayout.BeginScrollView(folderScroll);
						if(showDrives){
							foreach(DirectoryInformation di in drives){
								if(di.button()){	getFileList(di.di);	}
							}
						}else{
							if((backStyle != null)?parentDir.button(backStyle):parentDir.button())
								getFileList(parentDir.di);
						}
						foreach(DirectoryInformation di in directories){
							if(di.button()){	getFileList(di.di);	}
						}
						GUILayout.EndScrollView();
					GUILayout.EndVertical();
					GUILayout.BeginVertical("box");
						if(isSearching){
							drawSearchMessage();
						}else{
							fileScroll = GUILayout.BeginScrollView(fileScroll);
							for(int fi=0;fi<files.Length;fi++){
								if(selectedFile==fi){
									defaultColor = GUI.color;
									GUI.color = selectedColor;
								}
								if(files[fi].button()){
									outputFile = files[fi].fi;
									selectedFile = fi;
								}
								if(selectedFile==fi)
									GUI.color = defaultColor;
							}
							GUILayout.EndScrollView();
						}
						GUILayout.BeginHorizontal("box");
						GUILayout.FlexibleSpace();
						if((cancelStyle == null)?GUILayout.Button("Cancel"):GUILayout.Button("Cancel",cancelStyle)){
							outputFile = null;
							return true;
						}
						GUILayout.FlexibleSpace();
						if((selectStyle == null)?GUILayout.Button("Select"):GUILayout.Button("Select",selectStyle)){	return true;	}
						GUILayout.FlexibleSpace();
						GUILayout.EndHorizontal();
					GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				break;
			case 1: //mobile preferred layout
			default:
				if(showSearch){
					GUILayout.BeginHorizontal("box");
						GUILayout.FlexibleSpace();
						drawSearchField();
						GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
				}
				fileScroll = GUILayout.BeginScrollView(fileScroll);
				
				if(isSearching){
					drawSearchMessage();
				}else{
					if(showDrives){
						GUILayout.BeginHorizontal();
						foreach(DirectoryInformation di in drives){
							if(di.button()){	getFileList(di.di);	}
						}
						GUILayout.EndHorizontal();
					}else{
						if((backStyle != null)?parentDir.button(backStyle):parentDir.button())
							getFileList(parentDir.di);
					}
					
					
					foreach(DirectoryInformation di in directories){
						if(di.button()){	getFileList(di.di);	}
					}
					for(int fi=0;fi<files.Length;fi++){
						if(selectedFile==fi){
							defaultColor = GUI.color;
							GUI.color = selectedColor;
						}
						if(files[fi].button()){
							outputFile = files[fi].fi;
							selectedFile = fi;
						}
						if(selectedFile==fi)
							GUI.color = defaultColor;
					}
				}
				GUILayout.EndScrollView();
				
				if((selectStyle == null)?GUILayout.Button("Select"):GUILayout.Button("Load",selectStyle)){	return true;	}
             
                if ((cancelStyle == null)?GUILayout.Button("Cancel"):GUILayout.Button("Cancel",cancelStyle)){
					outputFile = null;
					return true;
				}
				break;
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		if(guiSkin){GUI.skin = oldSkin;}
		return false;
	}
	
	protected void drawSearchField(){
		if(isSearching){
			GUILayout.Label("Searching For: \""+searchBarString+"\"");
		}else{
			searchBarString = GUILayout.TextField(searchBarString,GUILayout.MinWidth(150));
			if(GUILayout.Button("search")){
				if(searchBarString.Length > 0){
					isSearching = true;
					#if thread
					startSearchTime = Time.time;
					t = new Thread(threadSearchFileList);
					t.Start(true);
					#else
					searchFileList(currentDirectory);
					#endif
				}else{
					getFileList(currentDirectory);
				}
			}
		}
	}
	
	protected void drawSearchMessage(){
		float tt = Time.time-startSearchTime;
		if(tt>1)
			GUILayout.Button("Searching");
		if(tt>2)
			GUILayout.Button("For");
		if(tt>3)
			GUILayout.Button("\""+searchBarString+"\"");
		if(tt>4)
			GUILayout.Button(".....");
		if(tt>5)
			GUILayout.Button("It's");
		if(tt>6)
			GUILayout.Button("Taking");
		if(tt>7)
			GUILayout.Button("A");
		if(tt>8)
			GUILayout.Button("While");
		if(tt>9)
			GUILayout.Button(".....");
	}

    
    public void getFileList(DirectoryInfo di){
		//set current directory
		currentDirectory = di;
		//get parent
		if(backTexture)
			parentDir = (di.Parent==null)?new DirectoryInformation(di,backTexture):new DirectoryInformation(di.Parent,backTexture);
		else
			parentDir = (di.Parent==null)?new DirectoryInformation(di):new DirectoryInformation(di.Parent);
		showDrives = di.Parent==null;
		
		//get drives
		string[] drvs = System.IO.Directory.GetLogicalDrives();
		drives = new DirectoryInformation[drvs.Length];
		for(int v=0;v<drvs.Length;v++){
			drives[v]= (driveTexture==null)?new DirectoryInformation(new DirectoryInfo(drvs[v])):new DirectoryInformation(new DirectoryInfo(drvs[v]),driveTexture);
		}
		
		//get directories
		DirectoryInfo[] dia = di.GetDirectories();
		directories = new DirectoryInformation[dia.Length];
		for(int d=0;d<dia.Length;d++){
			if(directoryTexture)
				directories[d] = new DirectoryInformation(dia[d],directoryTexture);
			else
				directories[d] = new DirectoryInformation(dia[d]);
		}
		
		//get files
		FileInfo[] fia = di.GetFiles(searchPattern);
		//FileInfo[] fia = searchDirectory(di,searchPattern);
		files = new FileInformation[fia.Length];
		for(int f=0;f<fia.Length;f++){
			if(fileTexture)
				files[f] = new FileInformation(fia[f],fileTexture);
			else
				files[f] = new FileInformation(fia[f]);
		}
	}
	
	
	public void searchFileList(DirectoryInfo di){
		searchFileList(di,fileTexture!=null);
	}
	
	protected void searchFileList(DirectoryInfo di,bool hasTexture){
		//(searchBarString.IndexOf("*") >= 0)?searchBarString:"*"+searchBarString+"*"; //this allows for more intuitive searching for strings in file names
		FileInfo[] fia = di.GetFiles((searchBarString.IndexOf("*") >= 0)?searchBarString:"*"+searchBarString+"*",(searchRecursively)?SearchOption.AllDirectories:SearchOption.TopDirectoryOnly);
		files = new FileInformation[fia.Length];
		for(int f=0;f<fia.Length;f++){
			if(hasTexture)
				files[f] = new FileInformation(fia[f],fileTexture);
			else
				files[f] = new FileInformation(fia[f]);
		}
		#if thread
		#else
		isSearching = false;
		#endif
	}
	
	protected void threadSearchFileList(object hasTexture){
		searchFileList(currentDirectory,(bool)hasTexture);
		isSearching = false;
	}
	
	//search a directory by a search pattern, this is optionally recursive
	public static FileInfo[] searchDirectory(DirectoryInfo di,string sp,bool recursive){
		return di.GetFiles(sp,(recursive)?SearchOption.AllDirectories:SearchOption.TopDirectoryOnly);
	}
	public static FileInfo[] searchDirectory(DirectoryInfo di,string sp){
		return searchDirectory(di,sp,false);
	}
	
	public float brightness(Color c){	return	c.r*.3f+c.g*.59f+c.b*.11f;	}
	
	//to string
	public override string ToString(){
		return "Name: "+name+"\nVisible: "+isVisible.ToString()+"\nDirectory: "+currentDirectory+"\nLayout: "+layout.ToString()+"\nGUI Size: "+guiSize.ToString()+"\nDirectories: "+directories.Length.ToString()+"\nFiles: "+files.Length.ToString();
	}
}

public class FileInformation{
	public FileInfo fi;
	public GUIContent gc;
	
	public FileInformation(FileInfo f){
		fi=f;
		gc = new GUIContent(fi.Name);
	}
	
	
public FileInformation(FileInfo f,Texture2D img){
		fi = f;
		gc = new GUIContent(fi.Name,img);
	}
	
	public bool button(){return GUILayout.Button(gc);}
	public void label(){	GUILayout.Label(gc);	}
	public bool button(GUIStyle gs){return GUILayout.Button(gc,gs);}
	public void label(GUIStyle gs){	GUILayout.Label(gc,gs);	}
}

public class DirectoryInformation{
	public DirectoryInfo di;
	public GUIContent gc;
	
	public DirectoryInformation(DirectoryInfo d){
		di=d;
		gc = new GUIContent(d.Name);
	}
	
	public DirectoryInformation(DirectoryInfo d,Texture2D img){
		di=d;
		gc = new GUIContent(d.Name,img);
	}
	
	public bool button(){return GUILayout.Button(gc);}
	public void label(){	GUILayout.Label(gc);	}
	public bool button(GUIStyle gs){return GUILayout.Button(gc,gs);}
	public void label(GUIStyle gs){	GUILayout.Label(gc,gs);	}
}