using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Defines a defensive AI behavior. The squad will not move unless an enemy unit is
/// in its movement area.
/// </summary>
[AddComponentMenu("Tactibru/AI/Squad Behaviors/Offensive Behavior")]
public class AIOffensiveBehavior : AIUnitBehavior
{
	/// <summary>
	/// Determines the target for this squad.
	/// </summary>
	/// <returns>AI state the controller should enter after determining the target.</returns>
	public override AIState DetermineTarget()
	{
		MovePointBehavior movePoint = Actor.currentMovePoint;

		// Get the maximum movement distance.
		CombatSquadBehavior squadBehavior = Actor.GetComponent<CombatSquadBehavior>();
		if (squadBehavior == null)
		{
			Debug.LogError(string.Format("AI Squad ({0}) does not have a CombatSquadBehavior!", Actor.transform.name));
			Actor.actorHasMovedThisTurn = true;

			return AIState.PickingSquad;
		}
		int maxDistance = squadBehavior.Squad.Speed;

		// Retrieve a list of nodes within appropriate distance.
		List<MovePointBehavior> graph = new List<MovePointBehavior>();
		movePoint.BuildGraph(maxDistance, 0, grid, ref graph, true);

		Actor.actorHasMovedThisTurn = true;

		// Iterate over the nodes to determine if any node has an actor.
		foreach (MovePointBehavior node in graph)
		{
			ActorBehavior actorOnNode = gameController.GetActorOnNode(node);

			if (actorOnNode == null || (actorOnNode.theSide != GameControllerBehaviour.UnitSide.player))
				continue;

			Debug.Log("Found target actor!");

			grid.ignoreList.Remove(node);
			List<MovePointBehavior> path = movePoint.FindPath(node, maxDistance, grid);
			path.RemoveAt(path.Count - 1);

			Actor.pathList = path;
			Actor.canMove = true;

			GridBehavior.preCombat = true;
			grid.currentActor = Actor.gameObject;
			grid.targetActor = actorOnNode.gameObject;

			grid.ignoreList.Add(node);

			return AIState.WaitingForCombat;
		}

		int randomNode = Random.Range(0, graph.Count);
		List<MovePointBehavior> squadPath = movePoint.FindPath(graph[randomNode], maxDistance, grid);

		Actor.pathList = squadPath;
		Actor.canMove = true;

		return AIState.WaitingForMove;
	}
}
