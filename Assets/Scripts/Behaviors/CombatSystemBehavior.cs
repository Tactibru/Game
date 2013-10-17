using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Units;
using NodeSkeletonSystem;

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
	/// Indicates the prefab to be used for the Node Skeleton that units will be built from.
	/// </summary>
	public NodeSkeletonBehavior unitSkeleton;

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

	/// <summary>
	/// A list of NodeSkeletonBehavior instances used to render the combat units for the combat screen.
	/// </summary>
	private List<NodeSkeletonBehavior> unitPrefabs;

	/// <summary>
	/// HACK: Temporarily pads the combat scene out.
	/// </summary>
	private float hackTimeImpl;

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

		// TODO: REPLACE HACK, CRAP CODE.
		hackTimeImpl += Time.deltaTime;
		if (hackTimeImpl <= 1.0f)
			return;
		hackTimeImpl = 0.0f;

		// Perform combat logic.
		IEnumerable<CombatUnit> offFirstRow = offensiveSquad.Squad.Units.Where(l => l.Position.Row == 0).Select(l => l.Unit);
		IEnumerable<CombatUnit> offSecondRow = offensiveSquad.Squad.Units.Where(l => l.Position.Row == 1).Select(l => l.Unit);
		IEnumerable<CombatUnit> defFirstRow = defensiveSquad.Squad.Units.Where(l => l.Position.Row == 0).Select(l => l.Unit);
		IEnumerable<CombatUnit> defSecondRow = defensiveSquad.Squad.Units.Where(l => l.Position.Row == 1).Select(l => l.Unit);

		// Ensure there is actually somebody remaining!
		if (offensiveSquad.Squad.Units.Count == 0)
			endCombat(offensiveSquad);

		if (defensiveSquad.Squad.Units.Count == 0)
			endCombat(defensiveSquad);

		switch (currentAttacker)
		{
			case CurrentAttacker.OffensiveFront:
				{
					if (offFirstRow.Count() > 0)
					{
						int totalStrength = offFirstRow.Sum(l => l.Strength);

						int damagePerUnit = totalStrength / (defFirstRow.Count() > 0 ? defFirstRow.Count() : defSecondRow.Count());
						foreach (CombatUnit unit in (defFirstRow.Count() > 0 ? defFirstRow : defSecondRow))
						{
							unit.Health -= Mathf.Max(damagePerUnit - unit.Toughness, 0);

							Debug.Log(string.Format("{0}:{1} took {2} damage, {3} remaining.", defensiveSquad.ToString(), unit.Name, damagePerUnit, unit.Health));

							if (unit.Health <= 0)
								Debug.Log(string.Format("{0} was destroyed! {1} units remaining in squad.", unit.Name, defensiveSquad.Squad.Units.Count));
						}

						removeDeadUnits();
					}

					currentAttacker = CurrentAttacker.DefensiveFront;
				} break;

			case CurrentAttacker.DefensiveFront:
				{
					if (defFirstRow.Count() > 0)
					{
						int totalStrength = defFirstRow.Sum(l => l.Strength);

						int damagePerUnit = totalStrength / (offFirstRow.Count() > 0 ? offFirstRow.Count() : offSecondRow.Count());
						foreach (CombatUnit unit in (offFirstRow.Count() > 0 ? offFirstRow : offSecondRow))
						{
							unit.Health -= Mathf.Max(damagePerUnit - unit.Toughness, 0);

							Debug.Log(string.Format("{0}:{1} took {2} damage, {3} remaining.", offensiveSquad.ToString(), unit.Name, damagePerUnit, unit.Health));

							if (unit.Health <= 0)
								Debug.Log(string.Format("{0} was destroyed! {1} units remaining in squad.", unit.Name, offensiveSquad.Squad.Units.Count));
						}

						removeDeadUnits();
					}

					currentAttacker = CurrentAttacker.OffensiveBack;
				} break;

			case CurrentAttacker.OffensiveBack:
				{
					if (offSecondRow.Count() > 0)
					{
						int totalStrength = offSecondRow.Sum(l => l.Strength);

						int damagePerUnit = totalStrength / (defFirstRow.Count() > 0 ? defFirstRow.Count() : defSecondRow.Count());
						foreach (CombatUnit unit in (defFirstRow.Count() > 0 ? defFirstRow : defSecondRow))
						{
							unit.Health -= Mathf.Max(damagePerUnit - unit.Toughness, 0);

							Debug.Log(string.Format("{0}:{1} took {2} damage, {3} remaining.", defensiveSquad.ToString(), unit.Name, damagePerUnit, unit.Health));

							if (unit.Health <= 0)
								Debug.Log(string.Format("{0} was destroyed! {1} units remaining in squad.", unit.Name, defensiveSquad.Squad.Units.Count));
						}

						removeDeadUnits();
					}

					currentAttacker = CurrentAttacker.DefensiveBack;
				} break;

			case CurrentAttacker.DefensiveBack:
				{
					if (defSecondRow.Count() > 0)
					{
						int totalStrength = defSecondRow.Sum(l => l.Strength);

						int damagePerUnit = totalStrength / (offFirstRow.Count() > 0 ? offFirstRow.Count() : offSecondRow.Count());
						foreach (CombatUnit unit in (offFirstRow.Count() > 0 ? offFirstRow : offSecondRow))
						{
							unit.Health -= Mathf.Max(damagePerUnit - unit.Toughness, 0);

							Debug.Log(string.Format("{0}:{1} took {2} damage, {3} remaining.", offensiveSquad.ToString(), unit.Name, damagePerUnit, unit.Health));

							if (unit.Health <= 0)
								Debug.Log(string.Format("{0} was destroyed! {1} units remaining in squad.", unit.Name, offensiveSquad.Squad.Units.Count));
						}

						removeDeadUnits();
					}

					currentAttacker = CurrentAttacker.None;
				} break;

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
		if (offensiveSquad != null)
		{
			offensiveSquad.Squad.Units.RemoveAll(l => l.Unit.Health <= 0);

			if (offensiveSquad.Squad.Units.Count == 0)
				endCombat(offensiveSquad);
		}

		if (defensiveSquad != null)
		{
			defensiveSquad.Squad.Units.RemoveAll(l => l.Unit.Health <= 0);

			if (defensiveSquad.Squad.Units.Count == 0)
				endCombat(defensiveSquad);
		}
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

		int unitCount = offensiveSquad.Squad.Units.Count + defensiveSquad.Squad.Units.Count;
		unitPrefabs = new List<NodeSkeletonBehavior>(unitCount);

		createUnits(offensiveSquad.Squad.Units, true, 0.0f);
		createUnits (defensiveSquad.Squad.Units, false, 1.0f);

		currentAttacker = CurrentAttacker.OffensiveFront;
	}
	
	private void createUnits (IEnumerable<UnitData> units, bool flipHorizontally, float offset)
	{
		// Create a base object
		GameObject unitBase = (GameObject)Instantiate(new GameObject());
		unitBase.name = "__UNITBASE__";
		unitBase.transform.parent = transform;
		unitBase.transform.localPosition = Vector3.zero;
		unitBase.AddComponent<MonoBehaviour>();
		
		foreach(UnitData data in units)
		{
			float x = (flipHorizontally ? (-1.0f + (0.33f * data.Position.Row)) : 1.0f - (0.33f * data.Position.Row));
			float y = 0.7f - (0.33f * data.Position.Column);
			float z = 0.9f - (0.05f * data.Position.Column);

			NodeSkeletonBehavior skele = (NodeSkeletonBehavior)Instantiate(unitSkeleton);

			// Load body parts for the unit.
			foreach (NSSNode node in skele.SkeletonStructure.Nodes)
			{
				GameObject prefab = (GameObject)Resources.Load (string.Format ("Prefabs/UnitParts/{0}/{1}", node.Name, data.Unit.Name));
				prefab = (prefab ?? (GameObject)Resources.Load (string.Format ("Prefabs/UnitParts/{0}/001", node.Name)));
				
				if(prefab == null)
				{
					Debug.LogWarning(string.Format ("Could not find prefab for 'Prefabs/UnitParts/{0}/001'", node.Name));
					continue;
				}
				
				skele.AttachToNode(node.Name, prefab);
			}

			skele.transform.parent = unitBase.transform;
			Vector3 scale = (Vector3.one / 2.0f);
			if(flipHorizontally)
				scale.x *= -1.0f;
			skele.transform.localScale = scale;
			skele.transform.localPosition = Vector3.zero;

			skele.transform.Translate(x, y, z);
		}
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
		
		//foreach(GameObject obj in transform.GetComponentsInChildren<MonoBehaviour>())
		MonoBehaviour[] objects = GetComponentsInChildren<MonoBehaviour>();
		for(int _i = (objects.Count() - 1); _i >= 0; _i--)
		{
			if(objects[_i].name == "__UNITBASE__")
				DestroyImmediate(objects[_i].gameObject);
		}
		
		if(losingSquad != null)
			Destroy(losingSquad.gameObject);

		foreach (NodeSkeletonBehavior node in unitPrefabs)
			DestroyImmediate(node.gameObject);

		this.offensiveSquad = null;
		this.defensiveSquad = null;

		GridBehavior.preCombat = false;
		GridBehavior.inCombat = false;
		InCombat = false;
	}
}
