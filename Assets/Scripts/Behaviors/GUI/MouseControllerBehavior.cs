using UnityEngine;
using System.Collections;

public class MouseControllerBehavior : MonoBehaviour 
{
	public enum ButtonState
	{
		Up,
		Down,
		NUM_OF_STATES
	}
	
	public enum MouseButton
	{
		Left,
		Middle, 
		Right,
		NUM_OF_STATES
	}
	
	public ButtonState State;
	public MouseButton Buton;
	
	delegate void UpdateButtonStates();
	UpdateButtonStates[] buttonStates = new UpdateButtonStates[(int)ButtonState.NUM_OF_STATES];
	
	delegate void UpdateMouseButtonState();
	UpdateMouseButtonState[] mouseButtonStates = new UpdateMouseButtonState[(int)MouseButton.NUM_OF_STATES];
	// Update is called once per frame
	void Update () 
	{
		RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
		{
			Debug.Log("Hi I do nothing yet");
		}
	}
}
