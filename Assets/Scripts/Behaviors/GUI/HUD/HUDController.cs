using UnityEngine;
using System.Collections;

[AddComponentMenu("Tactibru/GUI/HUD Controller")]
public class HUDController : ButtonManagerBehavior
{
	//GameObject menuGroup;
	bool isEnabled;
	GameControllerBehaviour gameController;
	public TextMesh turnCount;
	public TextMesh whoseTurn;

	public GUIStyle style;

	/// <summary>
	/// Start this instance.
	/// </summary>
	//protected override void Start()
	void Start()
	{
		//base.Start();
		isEnabled = false;
		// Commented out until we have art assests
		//ToggleMenuGroup();
		gameController = GameObject.FindGameObjectWithTag("Grid").GetComponent<GameControllerBehaviour>();
		//menuGroup = GameObject.Find("Menu Group HUD").gameObject;
	}

	void OnGUI()
	{
		// Display for Whose turn it is and what the turn count is
		GUI.Box(new Rect(Screen.width * 0.0f, Screen.height * 0.85f, 190, 100), ("Turn: " + WhoseTurn() + "\n" + "Turn #: " + TurnCount()), style);

		// Button for Menu
		if(GUI.Button(new Rect(Screen.width * 0f, Screen.height * 0f,100,30), "Menu"))
		{
			if(isEnabled)
				isEnabled = false;
			else
				isEnabled = true;
		}

		// Toggle Menu Group
		if(isEnabled)
		{
			// Menu Group
			GUI.BeginGroup(new Rect(Screen.width * 0f, Screen.height * 0.05f, 100, 250));

			// Save button
			if(GUI.Button(new Rect(0, 10,90,30), "Save"))
			{
				Debug.Log ("Save Button, not implemented.");
			}

			// Load button
			if(GUI.Button(new Rect(0, 50,90,30), "Load"))
			{
				Debug.Log ("Load Button, not implemented.");
			}

			// Exit button
			if(GUI.Button(new Rect(0, 90,90,30), "Exit"))
			{
				Application.LoadLevel("MainMenuGUITest");
			}

			// End Main Menu Group
			GUI.EndGroup();
		}

		if(GUI.Button(new Rect(Screen.width * 0f, Screen.height * 0.75f, 100, 50), "End Turn"))
		{
			if (gameController.AllowPlayerControlledEnemies || gameController.currentTurn == GameControllerBehaviour.UnitSide.player)
				gameController.EndTurn();
		}


	}

	/// <summary>
	/// Whose turn is it right now?
	/// </summary>
	/// <returns>The string of who's turn it is.</returns>
	public string WhoseTurn()
	{
		return gameController.currentTurn.ToString();
	}

	/// <summary>
	/// How many turns have passed?
	/// </summary>
	/// <returns>Returns the number of turns as a string</returns>
	public string TurnCount()
	{
		return gameController.numberOfTurns.ToString();
	}

	/*
	public override void ButtonPressed(string buttonName)
	{
		switch(buttonName)
		{
		case "Menu Button HUD":
			ToggleMenuGroup();
			break;
		case "End Turn Button":
			if (gameController.AllowPlayerControlledEnemies || gameController.currentTurn == GameControllerBehaviour.UnitSide.player)
				gameController.EndTurn();
			break;
		case "Exit Button":
			Application.LoadLevel("MainMenuGUITest");
			break;
		default:
			break;
		}
	}
	*/
	
	/// <summary>
	/// Turns the MeshRenderer on and off for items in the menuGroup.
	/// </summary>
	/*
	void ToggleMenuGroup()
	{
		if(menuGroup != null)
		{
			menuGroup.transform.GetComponent<MeshRenderer>().enabled = isEnabled;
			// Exit button rendering
			menuGroup.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = isEnabled;
			menuGroup.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = isEnabled;
			// Load button rendering
			menuGroup.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = isEnabled;
			menuGroup.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = isEnabled;
			// Save button rendering
			menuGroup.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = isEnabled;
			menuGroup.transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>().enabled = isEnabled;
		}
		
		// Flip isEnabled bool
		if(isEnabled)
			isEnabled = false;
		else
			isEnabled = true;
	}
	*/
}
