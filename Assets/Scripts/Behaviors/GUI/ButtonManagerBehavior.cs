using UnityEngine;
using System.Collections;

public class ButtonManagerBehavior : MonoBehaviour
{
	static ButtonManagerBehavior instance;
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
	
	protected virtual void Start()
	{
		instance = this;
	}
	
	public virtual void ButtonPressed(string buttonName)
	{
		switch (buttonName) 
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
		}	
	}
}
