using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls the actions of the AI Player during the player's turn.
/// </summary>
[RequireComponent(typeof(GameControllerBehaviour))]
[RequireComponent(typeof(GridBehavior))]
[AddComponentMenu("Tactibru/AI/AI Controller")]
public class AIControllerBehavior : MonoBehaviour
{
	/// <summary>
	/// Tag that the main camera is marked as.
	/// </summary>
	private const string CAMERA_TAG = "MainCamera";

	/// <summary>
	/// Stores the current State.
	/// </summary>
	private AIState State = AIState.WaitingForPlayer;

	/// <summary>
	/// Stores a copy of the game controller behavior on the game object.
	/// </summary>
	private GameControllerBehaviour gameController;

	/// <summary>
	/// Stores a copy of the grid behavior on the game object.
	/// </summary>
	private GridBehavior grid;

	/// <summary>
	/// Stores the currently-selected actor that AI actions are being performed on.
	/// </summary>
	private ActorBehavior selectedActor = null;

	/// <summary>
	/// Stores the camera object so that tracking objects can be set.
	/// </summary>
	new private CameraBehavior camera = null;

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
		grid = GetComponent<GridBehavior>();

		// Locate a CameraBehavior if one is set.
		GameObject cameraObject = GameObject.FindGameObjectWithTag(CAMERA_TAG);
		if(cameraObject)
			camera = cameraObject.GetComponent<CameraBehavior>();
	}

	/// <summary>
	/// Calls the appropriate method for the AI state.
	/// </summary>
	public void Update()
	{
		switch (State)
		{
		case AIState.WaitingForPlayer:
			UpdateState_WaitingForPlayer();	
			break;

		case AIState.PickingSquad:
			UpdateState_PickingSquad();
			break;

		case AIState.DetermineMovePoint:
			UpdateState_DeterminingMovePoint();
			break;

		case AIState.WaitingForMove:
			UpdateState_WaitingForMove();
			break;

		case AIState.DetermineCombatTarget:
			UpdateState_DetermineCombatTarget();
			break;

		case AIState.WaitingForCombat:
			UpdateState_WaitingForCombat();
			break;
		}
	}

	/// <summary>
	/// Detects if the Enemy's turn has begun, then switches to the Picking Squad state.
	/// </summary>
	public void UpdateState_WaitingForPlayer()
	{
		if (gameController.currentTurn == GameControllerBehaviour.UnitSide.enemy)
			State = AIState.PickingSquad;
	}

	/// <summary>
	/// Iterates over the enemy's squads and selects the first active squad capable of moving.
	/// If all squads have moved, the turn is ended.
	/// </summary>
	public void UpdateState_PickingSquad()
	{
		foreach (ActorBehavior actor in gameController.enemyTeam)
		{
			if (!actor.actorHasMovedThisTurn)
			{
				selectedActor = actor;
				State = AIState.DetermineMovePoint;

				setCameraTarget(selectedActor.gameObject);

				return;
			}
		}

		State = AIState.WaitingForPlayer;
		setCameraTarget(null);
		gameController.EndTurn();
	}

	/// <summary>
	/// Sets the camera target.
	/// </summary>
	/// <param name="actor">Actor that will serve as the camera target.</param>
	private void setCameraTarget(GameObject actor)
	{
		if(camera == null)
			return;

		camera.trackingObject = actor;
	}

	/// <summary>
	/// Determines the movement point the AI actor should move to.
	/// </summary>
	public void UpdateState_DeterminingMovePoint()
	{
		AIUnitBehavior unitBehavior = selectedActor.GetComponent<AIUnitBehavior>();

		if (unitBehavior == null)
			unitBehavior = selectedActor.gameObject.AddComponent<AIDefensiveBehavior>();

		unitBehavior.Grid = grid;
		unitBehavior.GameController = gameController;
		State = unitBehavior.DetermineMovePoint();
	}

	/// <summary>
	/// Simply delays until the selected actor's movement has completed, then moves to the 
	/// Waiting For Combat state.
	/// </summary>
	public void UpdateState_WaitingForMove()
	{
		if (selectedActor.canMove)
			return;

		State = AIState.WaitingForCombat;
	}

	/// <summary>
	/// Determines the target in attack range the AI should attack.
	/// </summary>
	public void UpdateState_DetermineCombatTarget()
	{
		AIUnitBehavior unitBehavior = selectedActor.GetComponent<AIUnitBehavior>();
		
		if (unitBehavior == null)
			unitBehavior = selectedActor.gameObject.AddComponent<AIDefensiveBehavior>();
		
		unitBehavior.Grid = grid;
		unitBehavior.GameController = gameController;
		State = unitBehavior.DetermineCombatTarget();
	}

	/// <summary>
	/// Simply delays until the combat sequence has finished, then moves to the Picking Squad state.
	/// </summary>
	public void UpdateState_WaitingForCombat()
	{
		if (GridBehavior.inCombat)
			return;

		selectedActor = null;

		State = AIState.PickingSquad;
	}
}
