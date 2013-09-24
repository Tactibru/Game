using UnityEngine;
using System.Collections;

// Rename to ButtonControllerBehavior
public class ButtonControllerScript : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out hitInfo))
        {
            // Create a new ButtonBehavior in Scripts/Behaviors/GUI
            // Instead of checking for ButtonControllerBehavior, check for ButtonBehavior
            // This scripts goes on either Camera or other empty object
            // ButtonBehavior goes on button object!
            ButtonControllerScript buttonControl = hitInfo.collider.GetComponent<ButtonControllerScript>();
            if (buttonControl != null)
                buttonControl.MouseOver();
        }
	}

    // This should be moved to ButtonBehavior
    void MouseOver()
    {
       Debug.Log("Mouse is Over Button");
    }
}