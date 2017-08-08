using UnityEngine;
using System.Collections;

public class TrayBackGroundBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        transform.GetComponent<Renderer>().enabled = false;
	}
	
	
}
