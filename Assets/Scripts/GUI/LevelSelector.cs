using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Level Selector
/// Author: Ryan Taite & Karl Matthews
/// 
/// Script to handle the level selection scene's buttons presses.
/// </summary>
[AddComponentMenu("Tactibru/GUI/Level Selector")]
public class LevelSelector : ButtonManagerBehavior
{
	/// <summary>
	/// The Application.dataPath stored for easier use
	/// </summary>
	public static string assetPath;
	/// <summary>
	/// The Game Levels directory path.
	/// </summary>
	/// <remarks>
	/// Change this if the location of where the playable levels changes
	/// Otherwise, do not change, it is used in code below
	/// </remarks>
	public static string gameLevelsPath;
	/// <summary>
	/// List that holds the unity levels from Game Levels folder.
	/// </summary>
	public List<string> gameLevelsList = new List<string>();
	/// <summary>
	/// A list of the three textMeshes on the three level selection buttons.
	/// </summary>
	public List<TextMesh> levelButtons = new List<TextMesh>();
	/// <summary>
	/// The start index of the level list displayed.
	/// </summary>
	/// <remarks>
	/// Used to "move" the level list up and down without going out of bounds.
	/// </remarks>
	int startIndex;
	/// <summary>
	/// The top button of the three level select buttons.
	/// </summary>
	public TextMesh topButton;
	/// <summary>
	/// The middle button of the three level select buttons.
	/// </summary>
	public TextMesh middleButton;
	/// <summary>
	/// The bottom button of the three level select buttons.
	/// </summary>
	public TextMesh bottomButton;
	
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	protected override void Start()
	{
		base.Start();
		
		/// The current directory path to the asset folder of the game, this will be the correct path no matter what environment the game is ran on.
		assetPath = Application.dataPath;
		/// This is the directory path to the Game Levels folder where all of the current playable levels are kept.
		gameLevelsPath = assetPath + "/Scenes/Game Levels/";

		/// Add all levels found in Game Levels to gameLevelsPath list
		addLevelsFromDirToList(gameLevelsPath);
		
		/// startIndex is set to 0 becuase we want the level select buttons to start at the top or first level available.
		startIndex = 0;
		
		/// Sets the topButton to the first level button's TextMesh in the levelButtons array 
		topButton = levelButtons[0].transform.GetComponentInChildren<TextMesh>();
		/// Sets the middletopButton to the first level button's TextMesh in the levelButtons array
		middleButton = levelButtons[1].transform.GetComponentInChildren<TextMesh>();
		/// Sets the bottomButton to the first level button's TextMesh in the levelButtons array
		bottomButton = levelButtons[2].transform.GetComponentInChildren<TextMesh>();
		
		/// Called on Start so that the level button's text is changed from their defualt values to the approriate level names
		updateButtonText();
	}

	/// <summary>
	/// Activate the button that will be pressed.
	/// </summary>
	/// <param name='buttonName'>
	/// Name of the button in the scene being pressed.
	/// </param>
	public override void ButtonPressed(string buttonName)
	{
		/// Switch statement that loads the approiate level based on the level name passed in..
		/// Attempt to load level scene with the same name as the buttonName param passed in
		switch (buttonName)
		{
			/// When the down button is pressed, call the downButtonPressed function to move the level list down
		case "Down Button":
			downButtonPressed();
			break;
			/// When the up button is pressed, call the downButtonPressed function to move the level list up
		case "Up Button":
			upButtonPressed();
			break;
			/// When the top button is pressed, load the level it is currently displaying
		case "topButtonQuad":
			Application.LoadLevel(gameLevelsList[startIndex]);
			break;
			/// When the middle button is pressed, load the level it is currently displaying
		case "middleButtonQuad":
			Application.LoadLevel(gameLevelsList[startIndex + 1]);
			break;
			/// When the bottom button is pressed, load the level it is currently displaying
		case "bottomButtonQuad":
			Application.LoadLevel(gameLevelsList[startIndex + 2]);
			break;
			/// When the Menu button is pressed, go back to the Main Menu
		case "backToMenu":
			Application.LoadLevel("MainMenuGUITest");
			break;
		default:
			base.ButtonPressed(buttonName);
			break;
		}
	}
	
	
	/// <summary>
	/// When the up button is pressed, it moves the level list up by one so long as it does not go out of bounds.
	/// </summary>
	public void upButtonPressed()
	{
		/// Check to make sure we don't go out of bounds.
		if(startIndex - 1 >= 0)
		{
			/// Decrease the startIndex to move up the list by one
			startIndex--;
			/// Update the button's text to show the correct level names
			updateButtonText();
		}
	}
	
	/// <summary>
	/// When the down button is pressed, it moves the level list down by one so long as it does not go out of bounds.
	/// </summary>
	public void downButtonPressed()
	{
		if(startIndex + 3 < gameLevelsList.Count)
		{
			/// Increase the startIndex to move down the list by one
			startIndex++;
			/// Update the button's text to show the correct level names
			updateButtonText();
		}
	}
	
	/// <summary>
	/// Updates the button text to show the correct level names.
	/// </summary>
	/// <remarks>
	/// This is called each time the Up or Down button is pressed.
	/// </remarks>
	public void updateButtonText()
	{
		topButton.text = gameLevelsList[startIndex];
		middleButton.text = gameLevelsList[startIndex + 1];
		bottomButton.text = gameLevelsList[startIndex + 2];
	}
	
	/// <summary>
	/// Scans the directory for files.
	/// </summary>
	/// <param name='dirPath'>
	/// Directory path we want to scan for files
	/// </param>
	private void addLevelsFromDirToList(string dirPath)
	{
		/// Creates an array for all the files found in the directory passed in.
		/// Only adds files that end in ".unity", ignores ".unity.meta" or anything else
		string[] fileArray;

		if (Application.isEditor)
			fileArray = Directory.GetFiles(dirPath, "*.unity");
		else
		{
			fileArray = new string[] {
				"level03",
				"level04",
				"level05",
				"level06",
				"level07",
				"level08",
				"level09",
				"level10",
				"level11",
			};
		}
		
		/// Foreach file in the fileArray, add each file to the gameLevelsList
		foreach(string file in fileArray)
		{
			/// Adds each file in the fileArray to the gameLevelsList, grabs only the file name, not the path or extension
			gameLevelsList.Add(Path.GetFileNameWithoutExtension(file));
		}
	}
}
