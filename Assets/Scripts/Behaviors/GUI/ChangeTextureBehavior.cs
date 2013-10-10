using UnityEngine;
using System.Collections;
/// <summary>
/// Behavior handles changing the texture of the quads depending on whether or not the mouse is hovering over the object, clicking on the object or not clicking on the object.
/// 
/// Author: Karl Matthews
/// </summary>
public class ChangeTextureBehavior : MonoBehaviour 
{
	/// <summary>
	/// Public variables representing the three different textures that will appear on the quads depending on the mouse input.
	/// A public boolean variable that is true if the left mouse button has been pressed and false if it has been released. 
	/// </summary>
	public Texture2D normalTexture;
	public Texture2D pressedTexture;
	public Texture2D hoverTexture;
	public static bool pressed = false;
	
	/// <summary>
	/// Function that sets the texture to my hovering over texture if the the button has not been pressed and the mouse is hovering over the object.
	/// </summary>
	void OnMouseEnter()
	{
		if(!pressed)
		{
			renderer.material.SetTexture("_MainTex", hoverTexture);
		}
	}
	/// <summary>
	/// If the left mouse button has been pressed then change the texture of the quad to the pressed texture and set my pressed boolean to true.
	/// </summary>
	void OnMouseDown()
	{
		pressed = true;
		renderer.material.SetTexture("_MainTex", pressedTexture);
	}
	/// <summary>
	/// Once the mouse is not hovering over the object and the left mouse button is not being pressed then set the texture of the quad to my normal texture.
	/// </summary>
	void OnMouseExit()
	{
		if(!pressed)
		{
			renderer.material.SetTexture("_MainTex", normalTexture);
		}
	}
	/// <summary>
	/// If the left mouse button has been released then set the quad texture to my normal texture and turn pressed to false.
	/// </summary>
	void OnMouseUp()
	{
		renderer.material.SetTexture("_MainTex", normalTexture);
		pressed = false;
	}
	
	
	
}
