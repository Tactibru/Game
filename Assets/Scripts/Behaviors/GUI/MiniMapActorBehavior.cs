using UnityEngine;
using System.Collections;

public class MiniMapActorBehavior : MonoBehaviour {

    public MiniMapPointBehaviour currentPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (currentPosition)
        {
            transform.position = currentPosition.transform.position + new Vector3(0.0f, 0.0f, -0.01f);
        }
	}
}
