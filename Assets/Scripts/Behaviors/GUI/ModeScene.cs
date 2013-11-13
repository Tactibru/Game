using UnityEngine;
using System.Collections;

/// <summary>
/// Mode scene script that creates four different buttons and each button changes the color of the text.
/// Author: Karl John Matthews
/// </summary>
[AddComponentMenu("Tactibru/GUI/Scenes/Mode Scene")]
public class ModeScene : MonoBehaviour 
{
    private GUIStyle menuTitle;
    private GUIStyle menuGUI;

    // Use this for initialization
    /// <summary>
    /// Initializes the mode scene variables.
    /// Then I initialize the text alignment, the text color, and font size.
    /// </summary>
    void Start()
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
    void Update()
    {

    }

    /// <summary>
    /// Function that creates a title of the scene and four buttons that when you click them change the color of the text. 
    /// </summary>
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 150, 25, 300, 100), "Mode Scene", menuTitle);

        if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 4 + 100, 300, 50), "Mode 1", menuGUI))
        {
            menuGUI.normal.textColor = Color.green;
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 150, (Screen.height / 4) + 150, 300, 50), "Mode 2", menuGUI))
        {
            menuGUI.normal.textColor = Color.magenta;
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 150, (Screen.height / 4) + 200, 300, 50), "Mode 3", menuGUI))
        {
            menuGUI.normal.textColor = Color.yellow;
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 150, (Screen.height / 4) + 250, 300, 50), "Mode 4", menuGUI))
        {
            menuGUI.normal.textColor = Color.cyan;
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 150, (Screen.height / 4) + 450, 300, 50), "Return To Main Menu", menuGUI))
        {
            Application.LoadLevel("MainMenuGUITest");
        }

    }
}
