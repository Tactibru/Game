using UnityEngine;
using System.Collections;

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
	AnchorPosition oldPosition;
	public AnchorPosition anchorPosition;
	static Camera hudCamera;
	
	void Start()
	{
		RepositionAnchor();
	}
	
	void Update()
	{
		RepositionAnchor();
		//If allowing mide game resolution change,
		//Check if resolution changed, call reposition
	}
	
	void RepositionAnchor()
	{
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
		name = "Anchor - " + anchorPosition.ToString();
		oldPosition = anchorPosition;
	}
}
