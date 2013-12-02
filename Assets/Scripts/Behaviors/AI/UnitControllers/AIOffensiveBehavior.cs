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
	public override AIState DetermineMovePoint()
	{
		Actor.actorHasMovedThisTurn = true;

		return AIState.PickingSquad;
	}

	public override AIState DetermineCombatTarget()
	{
		return AIState.PickingSquad;
	}
}
