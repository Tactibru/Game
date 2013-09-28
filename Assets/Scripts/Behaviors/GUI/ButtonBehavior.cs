using UnityEngine;
using System.Collections;
/// <summary>
/// Script that handles the functionality of my Button Controller, such as Mouse Up, Mouse Down, and Mouse Over
/// 
/// Author: Karl John Matthews
/// </summary>
public class ButtonBehavior : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
<<<<<<< HEAD

    /// <summary>
    /// False = Up, True = Down
    /// </summary>
    public bool lastState = false;
=======
>>>>>>> 3f2a510240e82851d813ffacd130b1b96816a164
	
	// Update is called once per frame
	void Update () 
    {
       
	}
<<<<<<< HEAD

=======
>>>>>>> 3f2a510240e82851d813ffacd130b1b96816a164
    /// <summary>
    /// Function that prints a statement saying that my current mouse position is over the cube
    /// </summary>
    public void MouseOver()
    {
        Debug.Log("Mouse is over Cube");
    }

<<<<<<< HEAD
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
=======
    public void LeftButtonDown()
    {
>>>>>>> 3f2a510240e82851d813ffacd130b1b96816a164
        Debug.Log("Left Mouse button is down on cube");
        renderer.material.color = Color.red;
    }
    public void LeftButtonUp()
    {
<<<<<<< HEAD
        if (lastState == true)
            LMBReleased();

        lastState = false;

=======
>>>>>>> 3f2a510240e82851d813ffacd130b1b96816a164
        Debug.Log("Left Mouse button has been released");
        renderer.material.color = Color.green;
    }
}
