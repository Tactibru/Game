using UnityEngine;
using System.Collections;


/// <summary>
/// Level Selector
/// Author: Ryan Taite & Karl Matthews
/// 
/// Script to handle the level selection scene's buttons presses.
/// </summary>
public class LevelSelector : ButtonManagerBehavior
{
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
}
