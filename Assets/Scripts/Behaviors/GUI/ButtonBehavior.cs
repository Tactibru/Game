using UnityEngine;
using System.Collections;
/// <summary>
/// Script that handles the functionality of my Button Controller, such as Mouse Up, Mouse Down, and Mouse Over
/// 
/// Author: Karl John Matthews
/// </summary>
[AddComponentMenu("Tactibru/GUI/Button")]
public class ButtonBehavior : MonoBehaviour 
{
    /// <summary>
    /// False = Up, True = Down
    /// </summary>
    public bool lastState = false;
	
	 /// <summary>
    /// Function that prints a statement saying that my current mouse position is over the cube
    /// </summary>
    public void MouseOver()
    {
        Debug.Log("Mouse is over Cube");
    }
	/// <summary>
	/// If the left mouse button has been pressed then print a statement to the console
	/// </summary>
    public void LMBPressed()
    {
		Debug.Log("The left mouse button is being pressed");
    }
	/// <summary>
	/// If the left mouse button has been Released then print a statement to the console
	/// </summary>
    public void LMBReleased()
    {
		Debug.Log("The left mouse button is being released");
    }
	/// <summary>
	/// If the left mouse button has been held down then print a statement to the console
	/// </summary>
    public void LMBHeld()
    {
		Debug.Log("The left mouse button is being held");
    }
	/// <summary>
	/// If the left mouse button is down and last state is false then call LMB pressed otherwise call LMB held 
	/// Then set last state to true
	/// </summary>
    public void LeftButtonDown()
    {
        if (lastState == false)
            LMBPressed();
        else
            LMBHeld();

        lastState = true;
    }
	/// <summary>
	/// If the left mouse button is up then call LMB released and set last state to false
	/// </summary>
    public void LeftButtonUp()
    {
        if (lastState == true)
            LMBReleased();

        lastState = false;
    }
}
