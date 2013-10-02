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
	/// Marks which row in the combat sequence is the next active attacker.
	/// </summary>
	public enum CurrentAttacker
	{
		None,
		OffensiveFront,
		DefensiveFront,
		OffensiveBack,
		DefensiveBack
	}

	/// <summary>
	/// Indiciates which attacker is next in the combat sequence.
	/// </summary>
	private CurrentAttacker currentAttacker;

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

		/*int totalStrength = offFirstRow.Sum(l => l.Strength);
		int totalToughness = defFirstRow.Sum(l => l.Toughness);

		int damage = Mathf.Max(totalStrength - totalToughness, 0);

		Debug.Log(string.Format("Row 1 (Size: {1}) attacked for {0} damage.", damage, defFirstRow.Count()));

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
			endCombat();*/

		// Ensure there is actually somebody remaining!
		if (offensiveSquad.Squad.Units.Count == 0)
			endCombat(offensiveSquad);

		if (defensiveSquad.Squad.Units.Count == 0)
			endCombat(defensiveSquad);

		switch (currentAttacker)
		{
			case CurrentAttacker.OffensiveFront:
				{
					if (offFirstRow.Count() <= 0)
					{
						currentAttacker = CurrentAttacker.DefensiveFront;
						break;
					}

					int totalStrength = offFirstRow.Sum(l => l.Strength);
					int totalToughness = (defFirstRow.Count() > 0 ? defFirstRow.Sum(l => l.Toughness) : defSecondRow.Sum(l => l.Toughness));
					int damage = Mathf.Max(totalStrength - totalToughness, 0);

					int damagePerUnit = damage / (defFirstRow.Count() > 0 ? defFirstRow.Count() : defSecondRow.Count());
					foreach (CombatUnit unit in (defFirstRow.Count() > 0 ? defFirstRow : defSecondRow))
					{
						unit.Health -= damagePerUnit;

						Debug.Log(string.Format("{0} took {1} damage.", unit.Name, damagePerUnit));

						if (unit.Health <= 0)
							Debug.Log(string.Format("{0} was destroyed!", unit.Name));
					}

					removeDeadUnits();

					currentAttacker = CurrentAttacker.DefensiveFront;
				} break;

			case CurrentAttacker.DefensiveFront:
				currentAttacker = CurrentAttacker.OffensiveBack;
				break;

			case CurrentAttacker.OffensiveBack:
				currentAttacker = CurrentAttacker.DefensiveBack;
				break;

			case CurrentAttacker.DefensiveBack:
				currentAttacker = CurrentAttacker.None;
				break;

			case CurrentAttacker.None:
				endCombat(null);
				break;
		}
	}

	/// <summary>
	/// Removes all dead units from both the offensive and defensive squads.
	/// </summary>
	private void removeDeadUnits()
	{
		if(offensiveSquad != null)
			offensiveSquad.Squad.Units.RemoveAll(l => l.Unit.Health == 0);

		if(defensiveSquad != null)
			defensiveSquad.Squad.Units.RemoveAll(l => l.Unit.Health == 0);
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

		currentAttacker = CurrentAttacker.OffensiveFront;
	}

	/// <summary>
	/// Completes the combat, 
	/// </summary>
	/// <param name="losingSquad">
	/// Reference to the squad that lost the combat.
	/// </param>
	private void endCombat(CombatSquadBehavior losingSquad)
	{
		Debug.Log("Combat between " + offensiveSquad.ToString() + " and " + defensiveSquad.ToString() + " end.");
		
		if(losingSquad != null)
			Destroy(losingSquad.gameObject);

		this.offensiveSquad = null;
		this.defensiveSquad = null;

		GridBehavior.inCombat = false;
		InCombat = false;
	}
}
