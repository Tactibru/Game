using UnityEngine;
using System.Collections;

/// <summary>
/// Implements an idle animation by making the game object bob vertically along the Y axis.
/// 
/// Author: Ken 'Minalien' Murray
/// </summary>
[AddComponentMenu("Tactibru/Combat/Animations/Unit Idle Animation")]
public class UnitIdleAnimationBehavior : MonoBehaviour
{
	/// <summary>
	/// Distance (in meters) the game object will move.
	/// </summary>
	public float bobDistance = 0.04f;

	/// <summary>
	/// Whether or not the idle bob animation should be active.
	/// </summary>
	public bool Active
	{
		get { return idleActive; }
		set
		{
			idleActive = value;
			gameObject.transform.Translate(Vector3.down * (gameObject.transform.position.y - initialY));
		}
	}

	private bool idleActive = true;

	/// <summary>
	/// Whether or not the bob should be allowed to go lower than the initial Y coordinate.
	/// </summary>
	public bool belowInitial = false;

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
	public void FixedUpdate()
	{
		if (!Active)
			return;
		
		Vector3 pos = gameObject.transform.position;
		pos.y = initialY + (Mathf.Sin (2.0f * Time.time) * bobDistance);
		
		gameObject.transform.position = pos;
	}
}

