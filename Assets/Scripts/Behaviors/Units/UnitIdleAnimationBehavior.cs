using UnityEngine;
using System.Collections;

/// <summary>
/// Implements an idle animation by making the game object bob vertically along the Y axis.
/// 
/// Author: Ken 'Minalien' Murray
/// </summary>
public class UnitIdleAnimationBehavior : MonoBehaviour
{
	/// <summary>
	/// Distance (in meters) the game object will move.
	/// </summary>
	public float bobDistance = 0.02f;

	/// <summary>
	/// Whether or not the idle bob animation should be active.
	/// </summary>
	public bool Active = true;

	/// <summary>
	/// Stores the initial position of the object.
	/// </summary>
	private float initialY = 0.0f;

	/// <summary>
	/// Direction the animation is bobbing in.
	/// </summary>
	private float bobDirection = 1.0f;

	/// <summary>
	/// Initializes the initialY value to the Y coordinate of the game object.
	/// </summary>
	public void Start()
	{
		initialY = gameObject.transform.position.y;
	}

	/// <summary>
	/// Performs the animated bobbing.
	/// </summary>
	public void Update()
	{
		if (!Active)
			return;

		float moveDistance = bobDirection * Time.deltaTime * 0.1f;
		gameObject.transform.Translate(Vector3.up * moveDistance);

		float y = gameObject.transform.position.y;
		
		if ((y >= (initialY + bobDistance)) || (y <= (initialY - bobDistance)))
			bobDirection *= -1;
	}
}
