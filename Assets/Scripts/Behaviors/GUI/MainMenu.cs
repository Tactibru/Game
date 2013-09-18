using UnityEngine;
using System.Collections;

/// <summary>
/// A simple script that navigates the different scenes of the project through GUI buttons.
/// Also allows you to exit the game.
/// Author: Karl John Matthews
/// </summary>
public class MainMenu : MonoBehaviour 
{
    private GUIStyle menuTitle;
    private GUIStyle menuGUI;

	// Use this for initialization
    /// <summary>
    /// Initializes the main menu variables.
    /// Then I initialize the text alignment, the text color, and font size.
    /// </summary>
	void Start () 
    {
        menuTitle = new GUIStyle();
        menuTitle.alignment = TextAnchor.MiddleCenter;
        menuTitle.normal.textColor = Color.red;
        menuTitle.fontSize = 50;

        menuGUI = new GUIStyle();
        menuGUI.alignment = TextAnchor.MiddleCenter;
        menuGUI.normal.textColor = Color.white;
        menuGUI.fontSize = 25;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    /// <summary>
    /// Function that creates a GUI label for the title of the game and creates buttons that loads the different scenes and exit the game. 
    /// </summary>
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 150, 25, 300, 100), "Tactibru", menuTitle);

        if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 4 + 100, 300, 50), "Play", menuGUI))
        {
            Application.LoadLevel("GameSceneGUITest");
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 150, (Screen.height / 4) + 150, 300, 50), "Mode", menuGUI))
        {
            Application.LoadLevel("ModeSceneGUITest");
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 150, (Screen.height / 4) + 200, 300, 50), "Settings", menuGUI))
        {
            Application.LoadLevel("SettingsSceneGUITest");
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 150, (Screen.height / 4) + 250, 300, 50), "Credits", menuGUI))
        {
            Application.LoadLevel("CreditsSceneGUITest");
        }
        
        if (GUI.Button(new Rect(Screen.width / 2 - 150, (Screen.height / 4) + 300, 300, 50), "Exit", menuGUI))
        {
            Application.Quit();
        }
    }
}
