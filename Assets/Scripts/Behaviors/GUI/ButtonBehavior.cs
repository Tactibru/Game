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
}
