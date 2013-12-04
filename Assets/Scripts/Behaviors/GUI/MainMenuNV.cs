using UnityEngine;
using System.Collections;

public class MainMenuNV : MonoBehaviour 
{
	Rect Box = new Rect(0.0f,0.0f, 600.0f, 450.0f); 
    /// <summary>
    /// This is the main menu script.
    /// Creates buttons for the menus
    /// and directs them to the correct scenes.
    /// Neal Valiant
    /// </summary>

	void OnGUI()
	{
		GUI.Box(Box, "Tactibru"); 

		if(GUI.Button(new Rect(Box.center.x-50, Box.center.y-200, 120.0f, 20.0f), "Play"))
		{
			Application.LoadLevel("LevelSelectSceneGUITest"); 
		}

		if(GUI.Button(new Rect(Box.center.x-50, Box.center.y-180, 120.0f, 20.0f), "Settings"))
		{
			Application.LoadLevel("SettingsSceneGUITest"); 
		}

		if(GUI.Button(new Rect(Box.center.x-50, Box.center.y-160, 120.0f, 20.0f), "Credits"))
		{
			Application.LoadLevel("CreditsSceneGUITest"); 
		}

		if(GUI.Button(new Rect(Box.center.x-50, Box.center.y-140, 120.0f, 20.0f), "Exit"))
		{
			Application.Quit(); 
		}



	}
}
