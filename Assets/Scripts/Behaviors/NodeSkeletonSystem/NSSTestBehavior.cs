using UnityEngine;
using System.Collections;

public class NSSTestBehavior : MonoBehaviour {

	public GameObject prefab;

	// Use this for initialization
	void Start () {
		NodeSkeletonBehavior behavior = gameObject.GetComponent<NodeSkeletonBehavior>();
		if (behavior != null)
		{
			behavior.AttachToNode("head", prefab);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
