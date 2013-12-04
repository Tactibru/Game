using UnityEngine;
using System.Collections;

public class TempLevelSelect : MonoBehaviour
{
	private string[] levelNames = {
		"level01",
		"level02",
		"level03",
		"level04",
		"level05",
		"level06",
		"level07",
		"level08",
		"level09",
		"level10",
		"level11",
	};

	private float centerX;
	private float centerY;
	private int menuOffset = 0;

	public void Update()
	{
		centerX = Screen.width / 2.0f;
		centerY = Screen.width / 2.0f;
	}

	public void OnGUI()
	{
		float buttonWidth = Screen.width * 0.25f;
		float buttonHeight = Screen.height * 0.1f;
		float levelStart = buttonHeight * 3;

		// Back button.
		if(GUI.Button (new Rect(Screen.width * 0.1f, buttonHeight, Screen.width * 0.15f, buttonHeight), "Back"))
			Application.LoadLevel("MainMenuGUITest");

		// Up button.
		if (GUI.Button (new Rect (centerX - (buttonWidth / 2.0f), buttonHeight, buttonWidth, buttonHeight), "^"))
			menuOffset = Mathf.Max (menuOffset - 1, 0);

		// First Button
		if (GUI.Button (new Rect (centerX - (buttonWidth / 2.0f), levelStart, buttonWidth, buttonHeight), levelNames [menuOffset]))
			Application.LoadLevel (levelNames [menuOffset]);

		// Second Button
		if (GUI.Button (new Rect (centerX - (buttonWidth / 2.0f), levelStart + (buttonHeight * 1.2f), buttonWidth, buttonHeight), levelNames [menuOffset + 1]))
			Application.LoadLevel (levelNames [menuOffset + 1]);

		// Thid Button
		if (GUI.Button (new Rect (centerX - (buttonWidth / 2.0f), levelStart + (buttonHeight * 1.2f) * 2, buttonWidth, buttonHeight), levelNames[menuOffset + 2]))
			Application.LoadLevel (levelNames [menuOffset + 2]);

		// Down Button
		if(GUI.Button(new Rect(centerX - (buttonWidth / 2.0f), buttonHeight * 7, buttonWidth, buttonHeight), "v"))
			menuOffset = Mathf.Min (menuOffset + 1, levelNames.Length - 3);
	}
}
