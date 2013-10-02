using UnityEngine;
using System.Collections;

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
		if (InCombat) 
			combatCamera.enabled = true;
		else
			combatCamera.enabled = false;
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

		Debug.Log("Combat between " + offensiveSquad.ToString() + " and " + defensiveSquad.ToString() + " begin.");
	}

	/// <summary>
	/// Completes the combat, 
	/// </summary>
	private void endCombat(CombatSquadBehavior offensiveSquad, CombatSquadBehavior defensiveSquad)
	{
		Debug.Log("Combat between " + offensiveSquad.ToString() + " and " + defensiveSquad.ToString() + " end.");

		Destroy(defensiveSquad.gameObject);

		GridBehavior.inCombat = false;
		InCombat = false;
	}
}
