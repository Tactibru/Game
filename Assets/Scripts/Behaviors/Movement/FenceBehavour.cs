using UnityEngine;
using System.Collections;

public class FenceBehavour : MonoBehaviour 
{

   /// <summary>
   /// Being to set the fences to be invisible.
   /// 
   /// Alex Reiss
   /// </summary>

	
	void Start () 
    {
        transform.renderer.enabled = false;
	}
	
	
}
