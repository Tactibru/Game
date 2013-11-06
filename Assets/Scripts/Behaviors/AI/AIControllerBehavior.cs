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
	/// Stores the current state of the AI controller.
	/// </summary>
	private enum AIState
	{
		/// <summary>
		/// AI Controller is waiting for the player to end their turn.
		/// </summary>
		WaitingForPlayer,

		/// <summary>
		/// AI Controller is currently picking a Squad to move.
		/// </summary>
		PickingSquad,

		/// <summary>
		/// AI Controller is determining the target unit or grid node.
		/// </summary>
		DeterminingTarget,

		/// <summary>
		/// AI Controller is waiting for its unit to complete its movement.
		/// </summary>
		WaitingForMove,

		/// <summary>
		/// AI Controller is waiting for a combat sequence to complete.
		/// </summary>
		WaitingForCombat,
	}

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
	/// Stores the current movement target for the selected actor.
	/// </summary>
	private MovePointBehavior targetPoint = null;

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

			case AIState.DeterminingTarget:
				UpdateState_DeterminingTarget();
				break;

			case AIState.WaitingForMove:
				UpdateState_WaitingForMove();
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
				State = AIState.DeterminingTarget;
				return;
			}
		}

		State = AIState.WaitingForPlayer;
		gameController.EndTurn();
	}

	/// <summary>
	/// Determines whether any targets are in the AI's movement radius, and moves to that unit and attacks.
	/// If no unit is in range, moves toward the nearest target.
	/// 
	/// If (for some reason) the path is invalid, the squad is idled and the AI moves to the next squad.
	/// </summary>
	public void UpdateState_DeterminingTarget()
	{
		MovePointBehavior movePoint = selectedActor.currentMovePoint;

		// Get the maximum movement distance.
		CombatSquadBehavior squadBehavior = selectedActor.GetComponent<CombatSquadBehavior>();
		if (squadBehavior == null)
		{
			Debug.LogError(string.Format("AI Squad ({0}) does not have a CombatSquadBehavior!", selectedActor.transform.name));
			selectedActor.actorHasMovedThisTurn = true;
			State = AIState.PickingSquad;
			return;
		}
		int maxDistance = squadBehavior.Squad.Speed;

		// Retrieve a list of nodes within appropriate distance.
		List<MovePointBehavior> graph = new List<MovePointBehavior>();
		movePoint.BuildGraph(maxDistance, 0, grid, ref graph, true);

		selectedActor.actorHasMovedThisTurn = true;

		// Iterate over the nodes to determine if any node has an actor.
		foreach (MovePointBehavior node in graph)
		{
			ActorBehavior actorOnNode = gameController.GetActorOnNode(node);

			if (actorOnNode == null || (actorOnNode.theSide != GameControllerBehaviour.UnitSide.player))
				continue;

			Debug.Log("Found target actor!");

			grid.ignoreList.Remove(node);
			targetPoint = node;
			List<MovePointBehavior> path = movePoint.FindPath(targetPoint, maxDistance, grid);
			path.RemoveAt(path.Count - 1);

			selectedActor.pathList = path;
			selectedActor.canMove = true;

			GridBehavior.preCombat = true;
			grid.currentActor = selectedActor.gameObject;
			grid.targetActor = actorOnNode.gameObject;

			State = AIState.WaitingForMove;

			grid.ignoreList.Add(node);

			return;
		}

		// Determine if the squad has a Defensive squad type.
		// If so, this unit sits and does not move unless a unit enters its movement range.
		AISquadTypeBehavior squadType = selectedActor.GetComponent<AISquadTypeBehavior>();
		if (squadType != null && squadType.SquadType == AISquadTypeBehavior.AISquadType.Defensive)
		{
			State = AIState.PickingSquad;
			return;
		}

		int randomNode = Random.Range(0, graph.Count);
		List<MovePointBehavior> squadPath = movePoint.FindPath(graph[randomNode], maxDistance, grid);

		selectedActor.pathList = squadPath;
		selectedActor.canMove = true;

		State = AIState.WaitingForMove;
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
	/// Simply delays until the combat sequence has finished, then moves to the Picking Squad state.
	/// </summary>
	public void UpdateState_WaitingForCombat()
	{
		if (GridBehavior.inCombat)
			return;

		selectedActor = null;
		targetPoint = null;

		State = AIState.PickingSquad;
	}
}
