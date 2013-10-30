using UnityEngine;
using System.Collections;

public class HUDController : ButtonManagerBehavior
{
	public GameObject menuButtonGroup;
	public bool isEnabled;
	/// <summary>
	/// Start this instance.
	/// </summary>
	protected override void Start()
	{
		base.Start();
		isEnabled = false;
	}
	
	public override void ButtonPressed(string buttonName)
	{
		switch(buttonName)
		{
		case "Menu Button":
			ToggleMenuGroup();
			break;
		case "End Turn Button":
			break;
		default:
			break;
		}
	}
	
	void ToggleMenuGroup()
	{
		if(isEnabled)
			isEnabled = false;
		else
			isEnabled = true;
		
		menuButtonGroup.transform.GetComponent<MeshRenderer>().enabled = isEnabled;
		// Exit button rendering
		menuButtonGroup.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = isEnabled;
		menuButtonGroup.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = isEnabled;
		// Load button rendering
		menuButtonGroup.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = isEnabled;
		menuButtonGroup.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = isEnabled;
		// Save button rendering
		menuButtonGroup.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = isEnabled;
		menuButtonGroup.transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>().enabled = isEnabled;
	}
	
	
	
}
