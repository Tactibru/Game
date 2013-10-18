using UnityEngine;
using System.Collections;

public class LevelSelector : ButtonManagerBehavior
{
	public override void ButtonPressed (string buttonName)
	{
		switch(buttonName)
		{
		case "level3Button":
			Application.LoadLevel("level03");
			break;
		case "level4Button":
			Application.LoadLevel("level04");
			break;
		case "level5Button":
			Application.LoadLevel("level05");
			break;
		case "backToMenu":
			Application.LoadLevel("MainMenuGUITest");
			break;
		default:
			base.ButtonPressed(buttonName);
			break;
		}
		
		
	}
}
