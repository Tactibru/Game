using UnityEngine;
using System.Collections;

public class NSSTestBehavior : MonoBehaviour {

	public GameObject headPrefab;
	public GameObject handPrefab;

	// Use this for initialization
	void Start () {
		NodeSkeletonBehavior behavior = gameObject.GetComponent<NodeSkeletonBehavior>();
		if (behavior != null)
		{
			behavior.AttachToNode("head", headPrefab);
			behavior.AttachToNode("hand", handPrefab);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
