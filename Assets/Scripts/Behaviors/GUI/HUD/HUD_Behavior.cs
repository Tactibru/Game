using UnityEngine;
using System.Collections;

public class HUD_Behavior : MonoBehaviour {
	
	// Use this for initialization
	void Start () {

		Vector3 anchorPosition = new Vector3(0.0f, 1.0f, 0.25f);
		transform.position = gameObject.camera.ViewportToWorldPoint(anchorPosition);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

