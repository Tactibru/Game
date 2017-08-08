using UnityEngine;
using System.Collections;

public class GA_ExampleBall : MonoBehaviour
{
	public float Speed = 1.0f;
	
	void Start ()
	{
		GetComponent<Rigidbody>().velocity = new Vector3(Random.value * 0.2f - 0.1f, -1, 0) * Speed;
	}
	
	void Update()
	{
		GetComponent<Rigidbody>().AddForce(Vector3.down * 0.0001f);
		GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * Speed;
	}
}
