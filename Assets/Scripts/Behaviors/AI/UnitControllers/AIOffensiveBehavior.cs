using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Defines a defensive AI behavior. The squad will not move unless an enemy unit is
/// in its movement area.
/// </summary>
[AddComponentMenu("Tactibru/AI/Squad Behaviors/Offensive Behavior")]
public class AIOffensiveBehavior : AIUnitBehavior
{
	/// <summary>
	/// The actor being targeted by the AI squad.
	/// </summary>
	private ActorBehavior targetActor = null;

	/// <summary>
	/// Stores the distance from the AI Squad to the target.
	/// </summary>
	private float distanceToTarget = float.PositiveInfinity;

	/// <summary>
	/// Determines the target for this squad.
	/// </summary>
	/// <returns>AI state the controller should enter after determining the target.</returns>
	public override AIState DetermineMovePoint()
	{
		MovePointBehavior movePoint = Actor.currentMovePoint;

		// Get a list of spaces that are within attack range (movement + range).
		CombatSquadBehavior combatSquad = Actor.GetComponent<CombatSquadBehavior>();
		if(combatSquad == null)
		{
			Debug.LogError(string.Format("AI Squad ({0}) does not have a CombatSquadBehavior!", Actor.transform.name));
			Actor.actorHasMovedThisTurn = true;
			
			return AIState.PickingSquad;
		}

		int attackRange = combatSquad.Squad.Range;
		int moveDistance = combatSquad.Squad.Speed;

		List<MovePointBehavior> pointsInRange = new List<MovePointBehavior>();
		movePoint.BuildGraph(attackRange + moveDistance, 0, grid, ref pointsInRange, true);

		// Find the closest actor in attack range.
		foreach(MovePointBehavior node in pointsInRange)
		{
			ActorBehavior actorOnNode = gameController.GetActorOnNode(node);

			if(actorOnNode == null || (actorOnNode.theSide != GameControllerBehaviour.UnitSide.player))
				continue;

			// If the distance to the target is less than the distance to the selected target, target it instead.
			float distance = Vector3.Distance(Actor.transform.position, actorOnNode.transform.position);
			if(distance < distanceToTarget)
			{
				targetActor = actorOnNode;
				distanceToTarget = distance;
			}
		}

		// If a target was found, move toward it.
		if(targetActor != null)
		{
			// Build a path to the actor.
			List<MovePointBehavior> pathList = movePoint.FindPath(targetActor.currentMovePoint, (moveDistance + attackRange), grid, true);

			// Remove the target's move point from the grid.
			pathList.RemoveAt (pathList.Count - 1);

			// Determine the excess items in the path list.
			int excess = pathList.Count - moveDistance;

			if(excess > 0)
				pathList.RemoveRange (pathList.Count - excess, excess);

			MovePointBehavior targetPoint = pathList[pathList.Count - 1];

			// Determine the fastest path to the target point.
			pathList = movePoint.FindPath (targetPoint, moveDistance, grid);

			if(pathList != null)
			{
				Actor.pathList = pathList;
				Actor.canMove = true;

				grid.ignoreList.Remove (movePoint);

				grid.ignoreList.Add (targetPoint);

				Actor.actorHasMovedThisTurn = true;

				return AIState.WaitingForMove;
			}
		}
		else
		{
			// No target was found, so determine the closest target to move towards.
			foreach(ActorBehavior playerSquad in gameController.playerTeam)
			{
				float distance = Vector3.Distance(Actor.transform.position, playerSquad.transform.position);
				if(distance < distanceToTarget)
				{
					targetActor = playerSquad;
					distanceToTarget = distance;
				}
			}

			// Retrieve a list of all nodes within movement range.
			List<MovePointBehavior> nodesInRange = new List<MovePointBehavior>();
			movePoint.BuildGraph(moveDistance, 0, grid, ref nodesInRange);

			// Cast a ray to the target and retrieve the farthest one that is in our movement range.
			Ray ray = new Ray(Actor.transform.position, (targetActor.transform.position - Actor.transform.position).normalized);

			// Perform raycasting and store a list of all objects that have been selected.
			List<RaycastHit> hits = new List<RaycastHit>();
			hits.AddRange(Physics.RaycastAll (ray));
			
			// Iterate over the selection list to determine if the player has clicked on one of her squads.
			foreach(RaycastHit hitInfo in hits.OrderBy (l => l.distance).Reverse())
			{
				MovePointBehavior hitBehavior = hitInfo.transform.GetComponent<MovePointBehavior>();
				if(hitBehavior == null || !nodesInRange.Contains (hitBehavior))
					continue;

				// Target node has been found! Find a path to it.
				List<MovePointBehavior> pathList = movePoint.FindPath(hitBehavior, moveDistance, grid);

				if(pathList != null)
				{
					Actor.pathList = pathList;
					Actor.canMove = true;

					grid.ignoreList.Remove (movePoint);
					grid.ignoreList.Add (hitBehavior);

					Actor.actorHasMovedThisTurn = true;

					return AIState.WaitingForMove;
				}
			}
		}

		Actor.actorHasMovedThisTurn = true;
		return AIState.PickingSquad;
	}

	public override AIState DetermineCombatTarget()
	{
		targetActor = null;
		distanceToTarget = float.PositiveInfinity;

		MovePointBehavior movePoint = Actor.currentMovePoint;
		
		CombatSquadBehavior combatSquad = Actor.GetComponent<CombatSquadBehavior>();
		if(combatSquad == null)
		{
			Debug.LogError(string.Format("AI Squad ({0}) does not have a CombatSquadBehavior!", Actor.transform.name));
			Actor.actorHasMovedThisTurn = true;
			
			return AIState.PickingSquad;
		}
		
		int attackRange = combatSquad.Squad.Range;
		
		// Retrieve a list of nodes in range.
		List<MovePointBehavior> graph = new List<MovePointBehavior>();
		movePoint.BuildGraph (attackRange, 0, grid, ref graph, true);
		
		// Iterate over the nodes to determine if any node has an enemy.
		foreach(MovePointBehavior node in graph)
		{
			ActorBehavior actorOnNode = gameController.GetActorOnNode(node);
			
			if(actorOnNode == null || (actorOnNode.theSide != GameControllerBehaviour.UnitSide.player))
				continue;
			
			beginCombatWithTarget(actorOnNode);
			
			return AIState.WaitingForCombat;
		}
		
		return AIState.PickingSquad;
	}
}
