using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A base class that defines the basic AI behavior interface. Sub-classes of this should
/// be defined and applied to a specific AI-controlled squad in order to define its function.
/// </summary>
[RequireComponent(typeof(CombatSquadBehavior))]
[RequireComponent(typeof(ActorBehavior))]
public abstract class AIUnitBehavior : MonoBehaviour
{
	/// <summary>
	/// Used to retrieve the actor.
	/// </summary>
	public ActorBehavior Actor
	{
		get
		{
			if (actor == null)
				actor = GetComponent<ActorBehavior>();

			return actor;
		}
	}

	/// <summary>
	/// Stores the <see cref="ActorBehavior"/> on the game object.
	/// </summary>
	private ActorBehavior actor = null;

	/// <summary>
	/// Used by the <see cref="AIControllerBehavior"/> to set the game controller.
	/// </summary>
	public GameControllerBehaviour GameController
	{
		set { gameController = value; }
	}

	/// <summary>
	/// Used by the <see cref="AIControllerBehavior"/> to set the grid.
	/// </summary>
	public GridBehavior Grid
	{
		set { grid = value; }
	}

	/// <summary>
	/// References the level's <see cref="GameControllerBehavior"/>. This will be set by the
	/// <see cref="AIControllerBehavior"/>, not in the editor.
	/// </summary>
	protected GameControllerBehaviour gameController = null;

	/// <summary>
	/// References the level's <see cref="GridBehavior"/>. This will be set by the
	/// <see cref="AIControllerBehavior"/>, not in the editor.
	/// </summary>
	protected GridBehavior grid = null;

	/// <summary>
	/// Determine the grid point this squad should move to.
	/// </summary>
	/// <returns>AI state that the controller should move to.</returns>
	public abstract AIState DetermineMovePoint();

	/// <summary>
	/// Determine the target actor this squad should attack.
	/// </summary>
	public abstract AIState DetermineCombatTarget();

	/// <summary>
	/// Initiates combat between this squad and the target.
	/// </summary>
	/// <param name="target">Target.</param>
	protected void beginCombatWithTarget(ActorBehavior target)
	{
		// Capture the combat camera.
		CombatSystemBehavior combatSystem = GameObject.Find ("Combat Camera").GetComponent<CombatSystemBehavior>();
		if(combatSystem == null)
		{
			Debug.LogError ("Unable to find a valid combat system in scene!");
			return;
		}
		
		// Capture the offensive and defensive combat squads
		CombatSquadBehavior offensiveSquad = actor.GetComponent<CombatSquadBehavior>();
		
		if(!offensiveSquad)
		{
			Debug.LogError ("Attempted to enter combat with an invalid offensive squad!");
			return;
		}
		
		CombatSquadBehavior defensiveSquad = target.GetComponent<CombatSquadBehavior>();
		
		if(!defensiveSquad)
		{
			Debug.LogError ("Attempted to enter combat with an invalid defensive squad!");
			return;
		}
		
		// Hide the target movement points.
		grid.HideMovePoints();
		
		// Begin combat!
		combatSystem.BeginCombat (offensiveSquad, defensiveSquad, grid);
	}
}