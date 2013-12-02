using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Stores the current state of the AI controller.
/// </summary>
public enum AIState
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
	/// AI Controller is determining the target movement point.
	/// </summary>
	DetermineMovePoint,

	/// <summary>
	/// AI Controller is waiting for its unit to complete its movement.
	/// </summary>
	WaitingForMove,

	/// <summary>
	/// AI Controller is determining the combat target.
	/// </summary>
	DetermineCombatTarget,

	/// <summary>
	/// AI Controller is waiting for a combat sequence to complete.
	/// </summary>
	WaitingForCombat,
}