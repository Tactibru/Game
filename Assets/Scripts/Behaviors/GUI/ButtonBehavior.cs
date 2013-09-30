using UnityEngine;
using System.Collections;
/// <summary>
/// Script that handles the functionality of my Button Controller, such as Mouse Up, Mouse Down, and Mouse Over
/// 
/// Author: Karl John Matthews
/// </summary>
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

    public void LMBPressed()
    {
    }

    public void LMBReleased()
    {
    }

    public void LMBHeld()
    {
    }

    public void LeftButtonDown()
    {
        if (lastState == false)
            LMBPressed();
        else
            LMBHeld();

        lastState = true;
    }

    public void LeftButtonUp()
    {
        if (lastState == true)
            LMBReleased();

        lastState = false;
    }
}
