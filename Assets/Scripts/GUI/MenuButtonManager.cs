using UnityEngine;
using System.Collections;
/// <summary>
/// Button Manager for the main menu that loads the different scenes depending on what button has been pressed.
/// 
/// Author: Karl Matthews
/// </summary>
public class MenuButtonManager : ButtonManagerBehavior 
{
	public override void ButtonPressed(string buttonName)
	{
		switch (buttonName) 
		{
		case "Play Button":
			Application.LoadLevel("LevelSelectSceneGUITest");
			break;
		case "Settings Button":
			Application.LoadLevel("SettingsSceneGUITest");
			break;
		case "Credits Button":
			Application.LoadLevel("CreditsSceneGUITest");
			break;
		default:
			base.ButtonPressed(buttonName);
			break;
		}	
	}
}
