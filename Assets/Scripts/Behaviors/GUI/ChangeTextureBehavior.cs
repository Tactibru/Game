using UnityEngine;
using System.Collections;

/// <summary>
/// Behavior handles changing the color of the text depending on whether or not the mouse is hovering over the object, clicking on the object or not clicking on the object.
///
/// Author: Karl Matthews
/// </summary>
[AddComponentMenu("Tactibru/GUI/Texture Changer")]
public class ChangeTextureBehavior : MonoBehaviour
{
	/*
    /// <summary>
    /// Public variable representing the text mesh that will change color depending on the mouse state.
    /// A public boolean variable that is true if the left mouse button has been pressed and false if it has been released.
    /// </summary>
    public TextMesh text;
    public static bool pressed = false;

	
    /// <summary>
    /// Function that sets the color of the text mesh to yellow if the the button has not been pressed and the mouse is hovering over the object.
    /// </summary>
    void OnMouseEnter()
    {
        if(!pressed)
        {
            text.color = Color.yellow;
        }
    }
	
    /// <summary>
    /// If the left mouse button has been pressed then change the text mesh color to green and set my pressed boolean to true.
    /// </summary>
    void OnMouseDown()
    {
        text.color = Color.green;
        pressed = true;
    }
	
    /// <summary>
    /// Once the mouse is not hovering over the object and the left mouse button is not being pressed then set the test mesh color to red. 
    /// </summary>
    void OnMouseExit()
    {
        if(!pressed)
        {
            text.color = Color.red;
        }
		pressed = false;
    }
	
    /// <summary>
    /// If the left mouse button has been released then set the text mesh color to red and if pressed then called Pressed function.
    /// </summary>
    void OnMouseUp()
    {
        text.color = Color.red;
        if (pressed)
			Pressed();
    }
	
	/// <summary>
	/// Function that creates an Instance of a button manager and checks the name of the button pressed. 
	/// </summary>
	void Pressed()
	{
		ButtonManagerBehavior.Instance.ButtonPressed(name);	
	}
	*/
}