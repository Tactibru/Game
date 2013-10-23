using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Level Selector
/// Author: Ryan Taite & Karl Matthews
/// 
/// Script to handle the level selection scene's buttons presses.
/// </summary>
public class LevelSelector : ButtonManagerBehavior
{
	/// <summary>
	/// The display index repsents the index of the button you are on (Example: Level 1 represents index 0.
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
	 /// Overiding the start function and calling hide invalid choices 
	 /// </summary>
	protected override void Start()
	{
		base.Start();
		HideInvalidChoices();
	}
	/// <summary>
	/// Function that moves the button list up when the up button is clicked.
	 /// If the display index is greater than zero then toggle the scene button, and move the buttons up by using the scene holder. 
	/// </summary>
	public void MoveListUp()
	{	
		if(displayIndex > 0)
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
	void HideInvalidChoices ()
	{
		for(int index = sceneIndex + numberToDisplay; index < sceneButtons.Length; index++)
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
	public override void ButtonPressed (string buttonName)
	{
		/// Switch statement that loads the approiate level based on the level name passed in..
		/// Attempt to load level scene with the same name as the buttonName param passed in
		switch(buttonName)
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
	
	
}
