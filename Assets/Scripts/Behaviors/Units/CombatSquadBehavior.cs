using UnityEngine;
using System.Collections.Generic;
using Units;
using NodeSkeletonSystem;

/// <summary>
/// Denotes a Game Object that possesses a <see cref="Units.CombatSquad"/>
/// </summary>
public class CombatSquadBehavior : MonoBehaviour {
	/// <summary>
	/// Squad possessed by this game object.
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
	/// Internally tracks the number of units in combat.
	/// </summary>
	private int unitCount;
	
	/// <summary>
	/// Unit skeleton.
	/// </summary>
	public NodeSkeletonBehavior unitSkeleton;
	
	/// <summary>
	/// Node used to allow a unit to be selected.
	/// </summary>
	public GameObject selectionNode;

	/// <summary>
	/// Color for the unit base.
	/// </summary>
	private Color baseColor;

	/// <summary>
	/// Stores a local copy of the instantiated selection node.
	/// </summary>
	private GameObject selNode;

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

		// Iterate over and instantiate each of the squad's units.
		for (int _i = 0; _i < squad.Units.Count; _i++)
			squad.Units[_i].Unit = (CombatUnit)Instantiate(squad.Units[_i].Unit);
			
		unitCount = squad.Units.Count;
		updateSquadVisuals();
	}
	
	/// <summary>
	/// Updates the unit prefabs, in case some units have been destroyed since the last update cycle.
	/// </summary>
	public void Update ()
	{
		if(unitCount != squad.Units.Count)
			updateSquadVisuals();

		ActorBehavior actor = GetComponent<ActorBehavior>();
		if (actor == null)
			return;

		selNode.renderer.material.color = (actor.actorHasMovedThisTurn ? Color.gray : baseColor);
	}
	/// <summary>
	/// Updates the overworld visuals for the squad.
	/// </summary>
	private void updateSquadVisuals ()
	{
		// Retrieve the actor to determine whether or not to flip the object.
		ActorBehavior actor = GetComponent<ActorBehavior>();
		if (actor == null)
			return;

		bool flippedHorizontally = (actor == null ? false : actor.theSide == GameControllerBehaviour.UnitSide.player);
		
		// Hide any mesh renderer on this object.
		gameObject.renderer.enabled = false;
		
		// Clear the squad's sub units.
		List<MonoBehaviour> children = new List<MonoBehaviour>();
		children.AddRange(GetComponentsInChildren<MonoBehaviour>());
		for(int _i = (children.Count - 1); _i >= 0; _i--)
		{
			if(children[_i].gameObject == gameObject)
				continue;
				
			DestroyImmediate(children[_i].gameObject);
			children.RemoveAt(_i);
		}
		
		// Add the selector
		GameObject selNode = (GameObject)Instantiate(selectionNode);
		selNode.name = "Selection Node";
		selNode.transform.parent = transform;
		selNode.transform.localPosition = Vector3.zero;
		this.selNode = selNode;

		// Determine the side of the material based on its Actor component.
		baseColor = (actor.theSide == GameControllerBehaviour.UnitSide.player ? Color.blue : Color.red);
		
		// Create the new sub-units.
		unitCount = squad.Units.Count;
		
		foreach(UnitData data in squad.Units)
		{
			float x = -0.1f + (0.2f * data.Position.Row) + (data.Position.Column % 2 == 0 ? 0.05f : 0.0f);
			float z = 0.25f - (0.1f * data.Position.Column);
			float y = 0.5f;

			NodeSkeletonBehavior skele = (NodeSkeletonBehavior)Instantiate(unitSkeleton);

			// Load body parts for the unit.
			foreach (NSSNode node in skele.SkeletonStructure.Nodes)
			{
				GameObject prefab = (GameObject)Resources.Load(string.Format("Prefabs/UnitParts/{0}/{1}", node.Name, data.Unit.Name));
				prefab = (prefab ?? (GameObject)Resources.Load(string.Format("Prefabs/UnitParts/{0}/001", node.Name)));

				if (prefab == null)
				{
					Debug.LogWarning(string.Format("Could not find prefab for 'Prefabs/UnitParts/{0}/001'", node.Name));
					continue;
				}

				skele.AttachToNode(node.Name, prefab);
			}

			skele.transform.parent = transform;
			Vector3 scale = Vector3.one;
			if (flippedHorizontally)
				scale.x = -1.0f;
			scale.y = 0.5f;
			skele.transform.localScale = scale;
			skele.transform.localPosition = Vector3.zero;
			
			skele.transform.Translate(x, y, z);
			skele.transform.Rotate(Vector3.right, 45.0f, Space.World);
		}
	}
}
