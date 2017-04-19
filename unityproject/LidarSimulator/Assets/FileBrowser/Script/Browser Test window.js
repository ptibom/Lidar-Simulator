import System;		// Needed for .Net which is needed to save a temp file
import System.IO;	// for the "Browser.SaveFile" demo.

var BrowsersParent : GameObject; // The gameObject that you place the "Browser.cs" in.
private var Browser : Object; // A refrence to the Browser you are trying to control, will be found automatically on Awake.

function Awake() // Runs once before Start, best used to get refrences to other scripts.
{
	Browser = BrowsersParent.GetComponent("Browser");
}

function OnGUI() // Runs once at Runtime when object is loaded.
{
	// To Open a file with the browser run:  Browser.OpenFile( The_Path_To_Your_MainSaves_Folder );
	// The following is an example that will open the root directory of your project. If you run this in
	// Unity, without compiling, then the file browser will be a duplicate of your "Project" tab.
	if(GUI.Button(Rect(5,5,230,30), "Test Browser's Open File function"))
	{
		Browser.OpenFile( Application.dataPath);
	}
	
	// Since opening a file is reliant on the user making multiple actions before the file they wish to open is actually
	// known to the computer, I am unsure of how to pass references from "Browser" to any potential code "Browser" could be used with.
	// As such I can't make it as simple for you to do things like: var FileToOpen = Browser.OpenFile( SaveFiles );
	// because "Browser" has to wait for the user to choose the file they wish to open before "FileToOpen" could even be set.
	// This means that when you are setting up "Browser" to actually be used in a game, you must add your own file opening code to "Browser" first.
	// I have added a large comment block in "Browser" to explain how to do that. You however do not need to do add anything for the purpose
	// of this demo, instead "Browser" will Debug the path the user opens in the console so that you can verify that it is working.
	// 
	// The code that you add to open your file will be completely dependent on what you need to load. By using "Browser" I'm assuming
	// that you already have some method of saving and loading data to some form of file on disk (Player Prefs won't work with "Browser")
	// and if you do all you should need to do is call your "Load File" function where I commented in "Browser" and pass it the directory 
	// that "Browser" will supply.


	// To Save a file with Browser you must first create a file on the disk in a temporary location. Then when you call "Browser.SaveFile"
	// "Browser" simply moves the file you pass it to where ever the user selects, if the user cancels, then the file is deleted. In any 
	// case you must first save the file somewhere before calling "Browser.SaveFile", this is simply to make integrating "Browser" easier,
	// eventually I hope to make the OpenFile function just as "plug'n'play" and then "Browser" would be usable in multiple scenarios in one 
	// game without needing modification.
	//
	// To test "Browser.SaveFile" I'm just going to display a text field on the screen, save it via .Net, and pass it to "Browser". If you 
	// don't know any thing about .Net, don't worry, all you need to use "Browser.SaveFile" is the directory of a file to save, and a directory
	// that you want to restrict the user to (so they don't rome the whole dang hard drive, although you could restrict them to the hard drive 
	// it self, and then "Browser" would basically be a second file browser for their whole computer). However, for the purpose of this demo, 
	// the user will be restricted to the folder I've supplied "Demo Directory Tree".
	//
	// Note: You probably shouldn't save the temporary file inside the directory your restricting the user to, doing so would mean that they'd
	// be able to see the temporary file, it won't prevent any thing from working, it would just be awkward for the user.

	if(GUI.Button(Rect(5,50,230,30), "Test Browser's Save File function"))
	{		
			// All of this is just to create an example text file, if you want to know more about how I created it with System.IO and .Net 
			// feal free contact me with any questions on the forums (I know from experiance that there isn't exaclty much documentation 
			// on saving and loading files for beginers out there), or do what I did and comb through the .Net library's refrence on System.IO 
			// at:   http://msdn.microsoft.com/en-us/library/system.io.aspx
			var FileName = Application.dataPath + "/TempSaveFile";
			var Sw = new StreamWriter(FileName);
			Sw.WriteLine("This is an example save file.");
			Sw.WriteLine("");
			Sw.WriteLine("It was righten to show the ''Browser.SaveFile'' function in action.");
			Sw.WriteLine("...and apparently...");
			Sw.WriteLine("It works!!!");
			Sw.WriteLine("");
			Sw.WriteLine("YAY!!!!");
			Sw.Close();
		// This is the actual save file comand that you need to know about, the first:
		Browser.SaveFile( FileName, Application.dataPath+"/Demo Directory Tree");
	}
}