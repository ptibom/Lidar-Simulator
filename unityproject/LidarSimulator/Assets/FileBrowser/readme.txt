//setup
/*In order to use the FileBrowser class, include the FileBrowser.cs file with somewhere in your project files. If you are using javascript you need to make sure the file is compiled first "http://docs.unity3d.com/Manual/ScriptCompileOrderFolders.html".*/

//usage
/*In order to use the file browser, first construct a member of the class.*/

//default values
Current Directoy = CurrentDirectory();
Layout Type = 1 if mobile 0 otherwise;
GUI Rect = new Rect(0,0,Screen.width,Screen.height) if mobile new Rect(Screen.width*0.125f,Screen.height*0.125f,Screen.width*0.75f,Screen.height*0.75f) otherwise;

//Construction
FileBrowser fb = new FileBrowser(); //all defaults are kept
FileBrowser fb = new FileBrowser(int layoutStyle); //layout style, right now 0 or 1/default
FileBrowser fb = new FileBrowser(Rect guiRect); //the gui rect for the file browser
FileBrowser fb = new FileBrowser(string startingDirectory); //starting directory
FileBrowser fb = new FileBrowser(string directory,int layoutStyle);
FileBrowser fb = new FileBrowser(string directory,int layoutStyle,Rect guiRect);

//additional options
/*Theses additional components are optional and can be changed any time*/
fb.guiSkin = GUISkin;
fb.fileTexture = Texture2D; //optional texture
fb.directoryTexture = Texture2D; //optional texture
fb.backTexture = Texture2D; //optional texture
fb.driveTexture = Texture2D; //optional texture
fb.searchPattern = string; //optional search pattern used for finding files of various types, the format of the searchPattern for c#
fb.selectedColor = Color; //the background color of the selected file, the default is blue-ish
fb.backStyle,cancelStyle,selectStyle = GUIStyle; //custom style of for the back button, cancel button, or select button style
fb.showSearch = bool; //show the search bar or not
fb.searchRecursively = bool; //search the current directory and sub directories (this can greatly reduce performance)

fb.setDirectory(string dir); //set the directory
fb.setLayout(int l); //set the layout types
fb.setGUIRect(Rect r); //set the gui rect

You may comment the line #define thread to disable multi-threading. 

//Public Static
FileBrowser.searchDirectory(DirectoryInfo di,string sp,bool recursive); //This is a public function if you want to do your own search on a given directory, using a search pattern, and the option to search recursively.

//usage
/*In order to use the constructed file browser, call the draw function in a onGUI function.  The draw function returns true when a file is selected or cancel is hit.*/
void OnGUI(){
	if(fb.draw()){
		if(fb.outputFile == null){
			Debug.Log("Cancel hit");
		}else{
			Debug.Log("Ouput File = \""+fb.outputFile.ToString()+"\"");
			/*the outputFile variable is of type FileInfo from the .NET library "http://msdn.microsoft.com/en-us/library/system.io.fileinfo.aspx"*/
		}
	}
}