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
[AddComponentMenu("Tactibru/Core Systems/Combat System")]
public class CombatSystemBehavior : MonoBehaviour 
{
	/// <summary>
	/// Internally tracks the primary scene camera, used in the tactical view.
	/// </summary>
	public Camera mainCamera;

	/// <summary>
	/// Internally tracks the camera used to display the combat window.
	/// </summary>
	public Camera combatCamera;

	/// <summary>
	/// Marks which row in the combat sequence is the next active attacker.
	/// </summary>
	public enum CurrentAttacker
	{
		/// <summary>
		/// Default, blanket value that shouldn't be set.
		/// </summary>
		None,

		/// <summary>
		/// Marks the offensive side's front row as the current attacker.
		/// </summary>
		OffensiveFront,

		/// <summary>
		/// Marks the defensive side's front row as the current attacker.
		/// </summary>
		DefensiveFront,

		/// <summary>
		/// Marks the offensive side's back row as the current attacker.
		/// </summary>
		OffensiveBack,

		/// <summary>
		/// Marks the defensive side's back row as the current attacker.
		/// </summary>
		DefensiveBack
	}

	/// <summary>
	/// Indicates the prefab to be used for the Node Skeleton that units will be built from.
	/// </summary>
	public NodeSkeletonBehavior unitSkeleton;

	/// <summary>
	/// Material used to handle font rendering (set on <see cref="CombatSystemUnitBehavior"/> instances).
	/// </summary>
	public Material fontMaterial;

	/// <summary>
	/// Font used for unit text rendering.
	/// </summary>
	public Font font;

	/// <summary>
	/// Indicates which attacker is next in the combat sequence.
	/// </summary>
	private CurrentAttacker currentAttacker;

	/// <summary>
	/// Represents the offensive squad in this combat.
	/// </summary>
	public CombatSquadBehavior offensiveSquad { get; set; }

	/// <summary>
	/// Represents the defensive squad in this combat.
	/// </summary>
	public CombatSquadBehavior defensiveSquad { get; set; }

	/// <summary>
	/// A list of NodeSkeletonBehavior instances used to render the combat units for the combat screen.
	/// </summary>
	private List<NodeSkeletonBehavior> unitPrefabs;

	/// <summary>
	/// HACK: Temporarily pads the combat scene out.
	/// </summary>
	private float hackTimeImpl;

	/// <summary>
	/// Internally tracks the grid combat is occurring on.
	/// </summary>
	private GridBehavior grid;

	// Use this for initialization
	void Start () 
	{
		mainCamera.enabled = true;
		combatCamera.enabled = false;
	}
	
	// Update is called once per frame
	/// <summary>
	/// Function that checks whether your in combat and enables the proper camera. 
	/// </summary>
	void Update () 
	{
		if (GridBehavior.inCombat && !combatCamera.enabled)
			combatCamera.enabled = true;
		else if (!GridBehavior.inCombat && combatCamera.enabled)
			combatCamera.enabled = false;

		if (!GridBehavior.inCombat)
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
							int damageReceived = (unit.Toughness != 0 ? (int)Mathf.Ceil((float)damagePerUnit * (1.0f - (1.0f / (float)unit.Toughness))) : damagePerUnit);
							unit.CurrentHealth -= Mathf.Max(damageReceived, 0);
                            if (unit.CurrentHealth <= 0)
                            {
                                HonorSystemBehavior.inCombat = true;
                                HonorSystemBehavior.honorPenalty = unit.HonorMod;
                                HonorSystemBehavior.offensiveHonor++;
                            }
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
							int damageReceived = (unit.Toughness != 0 ? (int)Mathf.Ceil((float)damagePerUnit * (1.0f - (1.0f / (float)unit.Toughness))) : damagePerUnit);
							unit.CurrentHealth -= Mathf.Max(damageReceived, 0);

                            if (unit.CurrentHealth <= 0)
                            {
                                HonorSystemBehavior.inCombat = true;
                                HonorSystemBehavior.honorPenalty = unit.HonorMod;
                                HonorSystemBehavior.defensiveHonor++;
                            }
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
							int damageReceived = (unit.Toughness != 0 ? (int)Mathf.Ceil((float)damagePerUnit * (1.0f - (1.0f / (float)unit.Toughness))) : damagePerUnit);
							unit.CurrentHealth -= Mathf.Max(damageReceived, 0);
                            if (unit.CurrentHealth <= 0)
                            {
                                HonorSystemBehavior.inCombat = true;
                                HonorSystemBehavior.honorPenalty = unit.HonorMod;
                                HonorSystemBehavior.offensiveHonor++;
                            }
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
							int damageReceived = (unit.Toughness != 0 ? (int)Mathf.Ceil((float)damagePerUnit * (1.0f - (1.0f / (float)unit.Toughness))) : damagePerUnit);
							unit.CurrentHealth -= Mathf.Max(damageReceived, 0);
                            if (unit.CurrentHealth <= 0)
                            {
                                HonorSystemBehavior.inCombat = true;
                                HonorSystemBehavior.honorPenalty = unit.HonorMod;
                                HonorSystemBehavior.defensiveHonor++;
                            }
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
			offensiveSquad.Squad.Units.RemoveAll(l => l.Unit.CurrentHealth <= 0);

			if (offensiveSquad.Squad.Units.Count == 0)
				endCombat(offensiveSquad);
		}

		if (defensiveSquad != null)
		{
			defensiveSquad.Squad.Units.RemoveAll(l => l.Unit.CurrentHealth <= 0);

			if (defensiveSquad.Squad.Units.Count == 0)
				endCombat(defensiveSquad);
		}
	}

	/// <summary>
	/// Initializes the combat system.
	/// </summary>
	/// <param name="offensiveSquad">GameObject for the squad performing the attack.</param>
	/// <param name="defensiveSquad">GameObject for the squad on defense.</param>
	/// <param name="grid">Grid behavior on which the combat is taking place.</param>
	public void BeginCombat(CombatSquadBehavior offensiveSquad, CombatSquadBehavior defensiveSquad, GridBehavior grid)
	{
		if (offensiveSquad.Squad == null || defensiveSquad.Squad == null)
		{
			Debug.LogError("Combat was started with either the offensive or defense squad being null!");
			return;
		}

		this.grid = grid;

		GridBehavior.inCombat = true;
        AudioBehavior.inCombat = true;

		this.offensiveSquad = offensiveSquad;
		this.defensiveSquad = defensiveSquad;

		int unitCount = offensiveSquad.Squad.Units.Count + defensiveSquad.Squad.Units.Count;
		unitPrefabs = new List<NodeSkeletonBehavior>(unitCount);

		createUnits(offensiveSquad.Squad.Units, true, 0.0f);
		createUnits (defensiveSquad.Squad.Units, false, 1.0f);

		currentAttacker = CurrentAttacker.OffensiveFront;
	}
	
	/// <summary>
	/// Creates the sub-objects that display the individual units in a squad.
	/// </summary>
	/// <param name="units"></param>
	/// <param name="flipHorizontally"></param>
	/// <param name="offset"></param>
	private void createUnits (IEnumerable<UnitData> units, bool flipHorizontally, float offset)
	{
		if (fontMaterial == null || font == null)
			Debug.LogWarning("Font material is not set on the Combat System Behavior! Ensure you are using the prefab to create the combat system!");

		// Create a base object
		GameObject unitBase = new GameObject();
		unitBase.name = "__UNITBASE__";
		unitBase.transform.parent = transform;
		unitBase.transform.localPosition = Vector3.zero;
		unitBase.AddComponent<MonoBehaviour>();
		
		foreach(UnitData data in units)
		{
			float x = (flipHorizontally ? (-1.0f + (0.33f * (1 - data.Position.Row))) : 1.0f - (0.33f * (1 - data.Position.Row))) + (data.Position.Column % 2 == 0 ? 0.1f : 0.0f);
			float y = 0.7f - (0.33f * data.Position.Column);
			float z = 0.9f - (0.05f * data.Position.Column);

			NodeSkeletonBehavior skele = (NodeSkeletonBehavior)Instantiate(unitSkeleton);
			skele.gameObject.AddComponent<UnitIdleAnimationBehavior>();

			GameObject obj = new GameObject();
			obj.AddComponent<MeshRenderer>();
			obj.transform.parent = skele.transform;
			
			CombatSystemUnitBehavior unitBehavior = obj.gameObject.AddComponent<CombatSystemUnitBehavior>();
			unitBehavior.unit = data.Unit;
			unitBehavior.transform.localPosition = Vector3.zero;
			unitBehavior.transform.localScale = Vector3.one / 5.0f;
			if (font != null)
				unitBehavior.SetFont(font);

			if(fontMaterial != null)
				unitBehavior.renderer.material = fontMaterial;

			// Load body parts for the unit.
			foreach (NSSNode node in skele.SkeletonStructure.Nodes)
			{
				UnitAssetBehavior prefab = (UnitAssetRepository.Instance.getAssetGroupByName(node.Name).getPrefabByName(node.Name == "Weapon" ? data.Unit.Weapon.ToString() : data.Unit.Name));

				if(prefab == null)
				{
					Debug.LogWarning(string.Format ("Could not find prefab for 'Prefabs/UnitParts/{0}/001'", node.Name));
					continue;
				}
				
				skele.AttachToNode(node.Name, prefab.gameObject);
			}

			skele.transform.parent = unitBase.transform;
			Vector3 scale = (Vector3.one / 2.0f);
			if(flipHorizontally)
			{
				scale.x *= -1.0f;
				Vector3 lScale = unitBehavior.transform.localScale;
				lScale.x *= -1.0f;
				unitBehavior.transform.localScale = lScale;
			}
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
		MonoBehaviour[] objects = GetComponentsInChildren<MonoBehaviour>();
		for(int _i = (objects.Count() - 1); _i >= 0; _i--)
		{
			if(objects[_i].name == "__UNITBASE__")
				DestroyImmediate(objects[_i].gameObject);
		}
		
		if(losingSquad != null)
		{
			Destroy(losingSquad.gameObject);

			ActorBehavior actor = losingSquad.GetComponent<ActorBehavior>();
			if (actor != null && grid.ignoreList.Contains(actor.currentMovePoint))
				grid.ignoreList.Remove(actor.currentMovePoint);
		}

		foreach (NodeSkeletonBehavior node in unitPrefabs)
			DestroyImmediate(node.gameObject);

		this.offensiveSquad = null;
		this.defensiveSquad = null;

		GridBehavior.preCombat = false;
        HonorSystemBehavior.inCombat = true;
		GridBehavior.inCombat = false;
        AudioBehavior.inCombat = false;
	}
}
