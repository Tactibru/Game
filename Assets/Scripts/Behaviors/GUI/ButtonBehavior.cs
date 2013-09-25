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
	
	// Update is called once per frame
	void Update () 
    {
       
	}
    /// <summary>
    /// Function that prints a statement saying that my current mouse position is over the cube
    /// </summary>
    public void MouseOver()
    {
        Debug.Log("Mouse is over Cube");
    }

    public void LeftButtonDown()
    {
        Debug.Log("Left Mouse button is down on cube");
        renderer.material.color = Color.red;
    }
    public void LeftButtonUp()
    {
        Debug.Log("Left Mouse button has been released");
        renderer.material.color = Color.green;
    }
}
