using UnityEngine;
using System.Collections;
/// <summary>
/// Script that anchors the HUD to certain parts of the screen based off of anchor position.
/// 
/// Author: Karl Matthews
/// </summary>
[AddComponentMenu("Tactibru/GUI/Anchor Behavior")]
/// <summary>
/// Enum for all of the different Anchor positons 
/// </summary>
public enum AnchorPosition
{
	TopRight,
	Top,
	TopLeft,
	Right,
	Center,
	Left,
	BottomRight,
	Bottom,
	BottomLeft,
}

[ExecuteInEditMode]
public class AnchorBehavior : MonoBehaviour 
{
	/// <summary>
	/// The previous postion of the of the anchor position 
	/// </summary>
	AnchorPosition oldPosition;
	/// <summary>
	/// Variable that represents the current anchor position 
	/// </summary>
	public AnchorPosition anchorPosition;
	/// <summary>
	/// Camera object that represents the main camera
	/// </summary>
	static Camera hudCamera;
	
	void Start()
	{
		RepositionAnchor();
	}
	
	void Update()
	{
		RepositionAnchor();
	}
	
	/// <summary>
	/// Funnctions that repositions the anchor based the postion chosen from the enum, through the screen to world point function.
	/// </summary>
	void RepositionAnchor()
	{
		// If the previous postion of the anchor is equal to the current position of the anchor, return out of the function.
		if (oldPosition == anchorPosition)
			return;
		if (!hudCamera)
			hudCamera = transform.parent.camera;
		switch (anchorPosition) 
		{
		case AnchorPosition.TopLeft:
			transform.position = hudCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
			break;
		case AnchorPosition.Top:
			transform.position = hudCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 0));
			break;
		case AnchorPosition.TopRight:
			transform.position = hudCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
			break;
		case AnchorPosition.Left:
			transform.position = hudCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));
			break;
		case AnchorPosition.Center:
			transform.position = hudCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
			break;
		case AnchorPosition.Right:
			transform.position = hudCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));
			break;
		case AnchorPosition.BottomLeft:
			transform.position = hudCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
			break;
		case AnchorPosition.Bottom:
			transform.position = hudCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
			break;
		case AnchorPosition.BottomRight:
			transform.position = hudCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
			break;			
		default:
			break;		
		}
		// Set the name of the object to Anchor and what ever the anchor postion is.
		name = "Anchor - " + anchorPosition.ToString();
		// Then set old postion to the new position.
		oldPosition = anchorPosition;
		
	}
}
