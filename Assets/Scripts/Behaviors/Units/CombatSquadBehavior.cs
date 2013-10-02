using UnityEngine;
using System.Collections;
using Units;

/// <summary>
/// Denotes a Game Object that posseses a <see cref="Units.CombatSquad"/>
/// </summary>
public class CombatSquadBehavior : MonoBehaviour {
	/// <summary>
	/// Squad posessed by this game object.
	/// </summary>
	public CombatSquad Squad
	{
		get
		{
			return squad;
		}
	}

	/// <summary>
	/// Squad prefab used by this game object.
	/// </summary>
	public CombatSquad SquadPrefab;

	/// <summary>
	/// Squad instance actually used by this game object.
	/// </summary>
	private CombatSquad squad;

	/// <summary>
	/// Instantiates the squad prefab into the internally-held squad to avoid damaging the asset file.
	/// </summary>
	public void Start()
	{
		if (SquadPrefab == null)
		{
			Debug.LogError("CombatSquadBehavior does not have a squad attached to it!");
			return;
		}

		squad = (CombatSquad)Instantiate(SquadPrefab);
	}
}
