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

		float aspectRatio = (float)Screen.width / (float)Screen.height;
		float height = hudCamera.orthographicSize;
		float width = height * aspectRatio;

		switch (anchorPosition)
		{
		case AnchorPosition.TopLeft:
			transform.localPosition = (Vector3.left * width) + (Vector3.up * height);
			break;
		case AnchorPosition.Top:
			transform.localPosition = (Vector3.up * height);
			break;
		case AnchorPosition.TopRight:
			transform.localPosition = (Vector3.right * width) + (Vector3.up * height);
			break;
		case AnchorPosition.Left:
			transform.localPosition = (Vector3.left * width);
			break;
		case AnchorPosition.Center:
			transform.localPosition = Vector3.zero;
			break;
		case AnchorPosition.Right:
			transform.localPosition = (Vector3.right * width);
			break;
		case AnchorPosition.BottomLeft:
			transform.localPosition = (Vector3.left * width) + (Vector3.down * height);
			break;
		case AnchorPosition.Bottom:
			transform.localPosition = (Vector3.down * height);
			break;
		case AnchorPosition.BottomRight:
			transform.localPosition = (Vector3.right * width) + (Vector3.down * height);
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
