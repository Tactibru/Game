using UnityEngine;
using System.Collections;

/// <summary>
/// A simple Game Scene script that displays the title of the scene and has a return to main menu button.
/// Author: Karl John Matthews
/// </summary>
[AddComponentMenu("Tactibru/GUI/Scenes/Game Scene")]
public class GameScene : MonoBehaviour 
{
    /// <summary>
    /// Private members variables for the settings scene, the menu title and menuGUI
    /// </summary>
    private GUIStyle menuTitle;
    private GUIStyle menuGUI;

	// Use this for initialization
    /// <summary>
    /// Initializes the game scene variables.
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
    /// Creates a GUI label for the title of the scene.
    /// Then I create a GUI button that returns the user to main menu.
    /// </summary>
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 150, 25, 300, 100), "Game Scene", menuTitle);

        if (GUI.Button(new Rect(Screen.width / 2 - 150, (Screen.height / 4) + 450, 300, 50), "Return To Main Menu", menuGUI))
        {
            Application.LoadLevel("MainMenuGUITest");
        }

    }

}
