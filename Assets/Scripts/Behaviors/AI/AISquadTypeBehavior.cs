using UnityEngine;
using System.Collections;

/// <summary>
/// Provides a squad type to combat squads that are being used by the enemy.
/// </summary>
[RequireComponent(typeof(CombatSquadBehavior))]
[AddComponentMenu("Tactibru/AI/AI Squad Type Behavior")]
public class AISquadTypeBehavior : MonoBehaviour
{
	/// <summary>
	/// Specifies the available squad types.
	/// </summary>
	public enum AISquadType
	{
		/// <summary>
		/// A primarily offensive melee squad type.
		/// </summary>
		Offensive,

		/// <summary>
		/// A primarily defensive melee squad type.
		/// </summary>
		Defensive,
	}

	/// <summary>
	/// Squad type associated with this AI squad.
	/// </summary>
	public AISquadType SquadType;
}
