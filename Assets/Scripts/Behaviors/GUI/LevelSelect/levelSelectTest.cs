using UnityEngine;
using System.Collections;

public class levelSelectTest : MonoBehaviour {
	/// <summary>
	/// Author: Ryan Taite
	/// 
	/// Script to make a level selection menu.
	/// </summary>

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Raises the GU event.
	/// Following the Unity gui-Basics Tutorial
	/// URL: docs.unity3d.com/Documentation/Components/gui-Basics.html
	/// </summary>
	void OnGUI()
	{
		// Level select menu group
		GUI.BeginGroup(new Rect(10, 10, 300, 200));
		
		// All rectangles are now adjusted to the group. (0, 0) is the topleft corner of the group.
		
		// Level Select title
		GUI.Box (new Rect(10,10,100,90), "Level Select");
		
		// Level 1 button
		if(GUI.Button (new Rect(20,40,80,20), "Level 1"))
		{
			Application.LoadLevel(1);	// Currently this loads what seems to be the Tactibru main menu.
		}
		
		// Level 2 button
		if(GUI.Button(new Rect(20,70,80,20), "Level 2"))
		{
			Application.LoadLevel(2);	// Currently this loads the "Game Scene"
		}
		
		GUI.EndGroup();
		
	}
}
