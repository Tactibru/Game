using UnityEngine;
using System.Collections;

public class GA_ExampleWall : MonoBehaviour
{
	void Start ()
	{
		GetComponent<Renderer>().material.color = Color.gray;
	}
}
