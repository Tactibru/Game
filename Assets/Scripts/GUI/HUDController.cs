using UnityEngine;
using System.Collections;

[AddComponentMenu("Tactibru/GUI/HUD Controller")]
public class HUDController : ButtonManagerBehavior
{
	GameObject menuGroup;
	bool isEnabled;
	GameControllerBehaviour gameController;
	[System.NonSerialized]
	public TextMesh turnCount;
	[System.NonSerialized]
	public TextMesh whoseTurn;
	/// <summary>
	/// Start this instance.
	/// </summary>
	protected override void Start()
	{
		base.Start();
		isEnabled = false;
		ToggleMenuGroup();
		gameController = GameObject.FindGameObjectWithTag("Grid").GetComponent<GameControllerBehaviour>();
		menuGroup = GameObject.Find("Menu Group HUD").gameObject;
		turnCount = GameObject.Find("turnCountText").GetComponent<TextMesh>();
		whoseTurn = GameObject.Find("whoseTurnText").GetComponent<TextMesh>();
	}
	
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
		
		if(isEnabled)
			isEnabled = false;
		else
			isEnabled = true;

		
		
	}
	
	
	
}
