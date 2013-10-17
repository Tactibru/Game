using UnityEngine;
using System.Collections;

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
