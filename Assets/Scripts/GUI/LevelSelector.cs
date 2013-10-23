using UnityEngine;
using System.Collections;
using System.IO;


/// <summary>
/// Level Selector
/// Author: Ryan Taite & Karl Matthews
/// 
/// Script to handle the level selection scene's buttons presses.
/// </summary>
public class LevelSelector : ButtonManagerBehavior
{
	/// <summary>
	/// The Application.dataPath stored for easier use
	/// </summary>
	public static string assetPath = Application.dataPath;
	/// <summary>
	/// The Game Levels directory path.
	/// </summary>
	public static string gameLevelsPath = assetPath + "\\Scenes\\Game Levels\\";
	/// <summary>
	/// Number of levels in the Game Levels folder
	/// </summary>
	public static int levelCount = 0;
 
	/// <summary>
	/// Array that holds the levels from GameLevels
	/// Currently a work in progress. 
	/// Set to a size of 20, but only until I know how many levels are actually in the Game Levels folder.
	/// </summary>
	public GameScene[] levelArray = new GameScene[levelCount];
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	public void Start()
	{
		CheckIfDirectoryExists_TEST();
		scanDirectoryForFiles(gameLevelsPath);
	}
	
	/// <summary>
	/// Activate the button that will be pressed.
	/// </summary>
	/// <param name='buttonName'>
	/// Name of the button in the scene being pressed.
	/// </param>
	public override void ButtonPressed (string buttonName)
	{
		
		
		/// Switch statement that loads the approiate level based on the level name passed in.
		switch(buttonName)
		{
			/// Load level03 scene
		case "level3Button":
			Application.LoadLevel("level03");
			break;
			/// Load level04 scene
		case "level4Button":
			Application.LoadLevel("level04");
			break;
			/// Load level05 scene
		case "level5Button":
			Application.LoadLevel("level05");
			break;
			/// Go back to the main menu
		case "backToMenu":
			Application.LoadLevel("MainMenuGUITest");
			break;
		default:
			/// Attempt to load level scene with the same name as the buttonName param passed in
			base.ButtonPressed(buttonName);
			break;
		}
	}
	
	/// <summary>
	/// Checks if directory and certain files exist in the project.
	/// This is for testing purposes.
	/// </summary>
	public void CheckIfDirectoryExists_TEST()
	{
		/// Check if the Game Levels dir exists in the Assest path
		if(Directory.Exists(gameLevelsPath))
		{
			//Debug.Log(assetPath + "\\Scenes\\Game Levels\\");
			Debug.Log("Game Levels Dir exists!");
		}
		else
		{
			Debug.Log("Game Levels Dir does NOT exist!");
		}
		
		/// Check if the levels in Game Levels exist
		if(File.Exists(gameLevelsPath + "level03.unity"))
		{
			Debug.Log("level03.unity does exist!");
		}
		else
		{
			Debug.Log("level03.unity does NOT exist!");
		}
	}
	
	/// <summary>
	/// Scans the directory for files.
	/// </summary>
	/// <param name='dirPath'>
	/// Directory path we want to scan for files
	/// </param>
	private void scanDirectoryForFiles(string dirPath)
	{
		string[] fileArray = Directory.GetFiles(dirPath);
		
		foreach(string fileName in fileArray)
		{
			Debug.Log(fileName);
		}
	}
}
