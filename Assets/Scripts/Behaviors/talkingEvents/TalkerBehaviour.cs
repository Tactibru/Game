using UnityEngine;
using System.Collections;

public class TalkerBehaviour : MonoBehaviour 
{
    /// <summary>
    /// Used set the talkers render off.
    /// </summary>
	
	void Start () 
    {
        //transform.localScale = 
        transform.renderer.enabled = false;
	}
	
	
}
