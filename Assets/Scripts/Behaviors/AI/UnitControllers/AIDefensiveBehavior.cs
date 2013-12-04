using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Defines a defensive AI behavior. The squad will not move at all, and will only attack enemies who come in range.
/// </summary>
[AddComponentMenu("Tactibru/AI/Squad Behaviors/Defensive Behavior")]
public class AIDefensiveBehavior : AIUnitBehavior
{
	/// <summary>
	/// Determines the target for this squad.
	/// </summary>
	/// <returns>AI state the controller should enter after determining the target.</returns>
	public override AIState DetermineMovePoint()
	{
		Actor.actorHasMovedThisTurn = true;

		return AIState.DetermineCombatTarget;
	}

	public override AIState DetermineCombatTarget()
	{
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
