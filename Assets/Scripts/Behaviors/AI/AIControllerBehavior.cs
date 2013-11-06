using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the actions of the AI Player during the player's turn.
/// </summary>
[RequireComponent(typeof(GameControllerBehaviour))]
[AddComponentMenu("Tactibru/AI/AI Controller")]
public class AIControllerBehavior : MonoBehaviour
{
	/// <summary>
	/// Stores a copy of the game controller behavior on the game object.
	/// </summary>
	private GameControllerBehaviour gameController;

	/// <summary>
	/// Captures an instance of the game controller behavior from the game object.
	/// </summary>
	/// <remarks>
	/// Because GameControllerBehavior is a required component (see class attributes),
	/// this will never be null.
	/// </remarks>
	public void Start()
	{
		gameController = GetComponent<GameControllerBehaviour>();
	}

	public void Update()
	{
	}
}
