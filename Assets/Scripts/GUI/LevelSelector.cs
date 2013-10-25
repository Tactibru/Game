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
	/// Represents the index of the button you are on (Example: Level 1 represents index 0.
	/// The scene index represents the index of the scene buttons.
	/// The number to display tells you the number of buttons shown at one time.
	/// An array of game objects that represent the scene buttons.
	/// Scene button holder holds all of the scene buttons.
	/// </summary>
	public int displayIndex = 0;
	public int sceneIndex = 0;
	public int numberToDisplay = 3;
	public GameObject[] sceneButtons;
	public GameObject sceneButtonHolder; 
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	protected override void Start()
	{
		base.Start();

		assetPath = Application.dataPath;
		gameLevelsPath = assetPath + "/Scenes/Game Levels/";

		/// Add all levels found in Game Levels to gameLevelsPath list
		addLevelsFromDirToList(gameLevelsPath);

		HideInvalidChoices();
	}


	/// <summary>
	/// Function that moves the button list up when the up button is clicked.
	/// If the display index is greater than zero then toggle the scene button, and move the buttons up by using the scene holder. 
	/// </summary>
	public void MoveListUp()
	{
		if (displayIndex > 0)
		{
			ToggleSceneButton(displayIndex + numberToDisplay - 1, false);
			displayIndex--;
			ToggleSceneButton(displayIndex, true);
			sceneButtonHolder.transform.position -= Vector3.up * 2;
		}
	}
	/// <summary>
	/// Moves the list down if the display index is less than the length of the array and number to display 
	/// </summary>
	public void MoveListDown()
	{
		if (displayIndex < sceneButtons.Length - numberToDisplay)
		{
			ToggleSceneButton(displayIndex, false);
			displayIndex++;
			ToggleSceneButton(displayIndex + numberToDisplay - 1, true);
			sceneButtonHolder.transform.position += Vector3.up * 2;
		}
	}
	/// <summary>
	/// Toggles the scene button.
	/// If the the index is greater than or equal to zero and the index is less than the array lenth then enable the collider and enable the text mesh.
	/// </summary>
	/// <param name='index'>
	/// Index.
	/// </param>
	/// <param name='isEnabled'>
	/// Is enabled.
	/// </param>
	void ToggleSceneButton(int index, bool isEnabled)
	{
		if (index >= 0 && index < sceneButtons.Length)
		{
			sceneButtons[index].collider.enabled = isEnabled;
			sceneButtons[index].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = isEnabled;
		}
	}
	/// <summary>
	/// Hides the invalid buttons if they are outside the three main buttons in focus.
	/// </summary>
	void HideInvalidChoices()
	{
		for (int index = sceneIndex + numberToDisplay; index < sceneButtons.Length; index++)
		{
			ToggleSceneButton(index, false);
		}
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
			case "Down Button":
				MoveListDown();
				break;
			case "Up Button":
				MoveListUp();
				break;
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
				base.ButtonPressed(buttonName);
				break;
		}

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
		string[] fileArray = Directory.GetFiles(dirPath, "*.unity");
		
		/// Foreach file in the fileArray, add each file to the gameLevelsList
		foreach(string file in fileArray)
		{
			/// Adds each file in the fileArray to the gameLevelsList, grabs only the file name, not the path or extension
			gameLevelsList.Add(Path.GetFileNameWithoutExtension(file));
		}
	}
}
