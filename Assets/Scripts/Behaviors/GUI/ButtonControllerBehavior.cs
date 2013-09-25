using UnityEngine;
using System.Collections;

/// <summary>
/// Script that constructs a ray from the current mouse coordinates. Then calls a MouseOver function if the ray hits an object.
/// 
/// Author: Karl John Matthews
/// </summary>
public class ButtonControllerBehavior : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    /// <summary>
    /// Function that constructs a ray from the current mouse position.
    /// Checks to see if the ray hits an object, if it does then create a ButtonBehavior object and get the ButtonBehavior component
    /// If the object has been created then call MouseOver function
    /// </summary>
    void Update()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            // Create a new ButtonBehavior in Scripts/Behaviors/GUI
            // Instead of checking for ButtonControllerBehavior, check for ButtonBehavior
            // This scripts goes on either Camera or other empty object
            // ButtonBehavior goes on button object!
            ButtonBehavior buttonControl = hitInfo.collider.GetComponent<ButtonBehavior>();
            if (buttonControl != null)
                buttonControl.MouseOver();
  
        }
    }

}