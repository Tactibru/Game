using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Units;

/// <summary>
/// Script that switches between the main camera and the combat when the InCombat boolean is true
/// 
/// Author: Karl John Matthews
/// </summary>
public class CombatSystemBehavior : MonoBehaviour 
{
	public bool InCombat;
	public Camera mainCamera;
	public Camera combatCamera;

	/// <summary>
	/// Represents the offensive squad in this combat.
	/// </summary>
	private CombatSquadBehavior offensiveSquad;

	/// <summary>
	/// Represents the defensive squad in this combat.
	/// </summary>
	private CombatSquadBehavior defensiveSquad;

	// Use this for initialization
	void Start () 
	{
		InCombat = false;
		mainCamera.enabled = true;
		combatCamera.enabled = false;
	}
	
	// Update is called once per frame
	/// <summary>
	/// Function that checks whether your in combat aand enambles the proper camera. 
	/// </summary>
	void Update () 
	{
		if (InCombat && !combatCamera.enabled)
			combatCamera.enabled = true;
		else if (!InCombat && combatCamera.enabled)
			combatCamera.enabled = false;

		if (!InCombat)
			return;

		// Perform combat logic.
		IEnumerable<CombatUnit> offFirstRow = offensiveSquad.Squad.Units.Where(l => l.Position.Row == 0).Select(l => l.Unit);
		IEnumerable<CombatUnit> offSecondRow = offensiveSquad.Squad.Units.Where(l => l.Position.Row == 1).Select(l => l.Unit);
		IEnumerable<CombatUnit> defFirstRow = defensiveSquad.Squad.Units.Where(l => l.Position.Row == 0).Select(l => l.Unit);
		IEnumerable<CombatUnit> defSecondRow = defensiveSquad.Squad.Units.Where(l => l.Position.Row == 1).Select(l => l.Unit);

		int totalStrength = offFirstRow.Sum(l => l.Strength);
		int totalToughness = defFirstRow.Sum(l => l.Toughness);

		int damage = Mathf.Max(totalStrength - totalToughness, 0);

		Debug.Log(string.Format("Row 1 attacked for {0} damage.", damage));

		foreach (CombatUnit unit in defFirstRow)
		{
			int dmg = (damage / defFirstRow.Count());
			unit.Health -= Mathf.Max(dmg, 0);

			Debug.Log(string.Format("{0} took {1} damage.", unit.Name, dmg));

			if (unit.Health == 0)
				Debug.Log(string.Format("{0} was destroyed!", unit.Name));
		}

		defensiveSquad.Squad.Units.RemoveAll(l => (l.Position.Row == 0 && l.Unit.Health == 0));

		int totalRemainingUnits = defensiveSquad.Squad.Units.Count;
		if (totalRemainingUnits <= 0)
			endCombat();
	}

	/// <summary>
	/// Initializes the combat system.
	/// </summary>
	/// <param name="offensiveSquad">GameObject for the squad performing the attack.</param>
	/// <param name="defensiveSquad">GameObject for the squad on defense.</param>
	public void BeginCombat(CombatSquadBehavior offensiveSquad, CombatSquadBehavior defensiveSquad)
	{
		if (offensiveSquad.Squad == null || defensiveSquad.Squad == null)
		{
			Debug.LogError("Combat was started with either the offensive or defense squad being null!");
			return;
		}

		GridBehavior.inCombat = true;
		InCombat = true;

		this.offensiveSquad = offensiveSquad;
		this.defensiveSquad = defensiveSquad;

		Debug.Log("Combat between " + offensiveSquad.ToString() + " and " + defensiveSquad.ToString() + " begin.");

		Debug.Log("Offensive size: " + offensiveSquad.Squad.Units.Count);
		Debug.Log("Defensive size: " + defensiveSquad.Squad.Units.Count);
	}

	/// <summary>
	/// Completes the combat, 
	/// </summary>
	private void endCombat()
	{
		Debug.Log("Combat between " + offensiveSquad.ToString() + " and " + defensiveSquad.ToString() + " end.");

		Destroy(defensiveSquad.gameObject);

		this.offensiveSquad = null;
		this.defensiveSquad = null;

		GridBehavior.inCombat = false;
		InCombat = false;
	}
}
