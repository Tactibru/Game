using UnityEngine;
using System.Collections;

/// <summary>
/// Script that creates one instance of a button manager that holds all the buttons for an individual scene.
/// 
/// Author: Karl Matthews
/// </summary>
public class ButtonManagerBehavior : MonoBehaviour
{
	/// <summary>
	/// Creates one instance of the button manager
	/// 
	/// The function then creates a new instance of the button if not found, but if it is found then get the component of the button manager.
	/// </summary>
	static ButtonManagerBehavior instance;

	/// <summary>
	/// Creates one instance of the button manager
	/// 
	/// The function then creates a new instance of the button if not found, but if it is found then get the component of the button manager.
	/// </summary>
	public static ButtonManagerBehavior Instance
	{
		get
		{
			if (instance == null)
			{ //In case the start function is overridden, and base is not called
				GameObject manager = GameObject.Find("Button Manager") as GameObject;
				if (manager != null)
					instance = manager.GetComponent<ButtonManagerBehavior>();
			}
			if (instance == null)
			{
				GameObject newManager = new GameObject("Button Manager");
				instance = newManager.AddComponent<ButtonManagerBehavior>();
			}
			return instance;
		}
	}
	
	
	/// <summary>
	/// An overidable start function that sets an instance to this game object.
	/// </summary>
	protected virtual void Start()
	{
		instance = this;
	}
	
	/// <summary>
	/// Virtual button pressed function that takes in the button name that exits the game if the button pressed is the exit button.
	/// Then if I am inside the unity editor then I just shut off the play button if the exit button is pressed.
	/// </summary>
	public virtual void ButtonPressed(string buttonName)
	{
		/*switch (buttonName) 
		{
		case "Exit Button":
			Application.Quit();
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			break;
		default:
			Debug.LogWarning(string.Format("No handler found for {0}!", buttonName));
			break;
		}*/	
	}
}
