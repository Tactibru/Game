using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Units;

/// <summary>
/// Represents a grid point on the map that units can move to.
/// </summary>
[AddComponentMenu("Tactibru/Movement/Move Point")]
public class MovePointBehavior : MonoBehaviour 
{

    public int index;
	/// <summary>
	/// Stores a list of nodes that this move point is connected to.
	/// </summary>
	public MovePointBehavior[] neighborList = new MovePointBehavior[4];

	/// <summary>
	/// Disables the renderer.
	/// </summary>
	public void Start()
	{
		renderer.enabled = false;
	}

	/// <summary>
	/// Attempts to find a path to the target node.
	/// </summary>
	/// <param name="targetNode">Final node the unit should move to.</param>
	/// <param name="maxDistance">Maximum distnance the unit can move.</param>
	/// <param name="grid">Grid that the pathfinding is occurring on.</param>
	/// <returns></returns>
	public List<MovePointBehavior> FindPath(MovePointBehavior targetNode, int maxDistance, GridBehavior grid)
	{
		// Build the Dijkstra's Graph
		List<MovePointBehavior> graph = new List<MovePointBehavior>();
		List<MovePointBehavior> tGraph = new List<MovePointBehavior>();
		
		BuildGraph(maxDistance, 0, grid, ref graph);

		if (!graph.Contains(targetNode))
			return null;

		Dictionary<MovePointBehavior, int> distance = new Dictionary<MovePointBehavior, int>();
		Dictionary<MovePointBehavior, MovePointBehavior> previous = new Dictionary<MovePointBehavior, MovePointBehavior>();

		foreach (MovePointBehavior node in graph)
		{
			distance.Add(node, int.MaxValue);
			previous.Add(node, null);

			tGraph.Add(node);
		}

		distance[this] = 0;
		previous[this] = null;

		while (tGraph.Count > 0)
		{
			// Find the item with the smallest distance.
			MovePointBehavior node = tGraph[0];
			for (int _i = 0; _i < tGraph.Count; _i++)
			{
				MovePointBehavior _node = tGraph[_i];

				if (grid.ignoreList.Contains(_node))
					continue;

				if (distance[_node] < distance[node])
				{
					node = _node;
					MovePointBehavior tNode = tGraph[0];
					tGraph[0] = _node;
					tGraph[_i] = tNode;
				}
			}

			tGraph.RemoveAt(0);

			if (distance[node] == int.MaxValue)
				break;

			foreach (MovePointBehavior neighbor in node.neighborList)
			{
				if (neighbor == null || grid.ignoreList.Contains(neighbor) || !graph.Contains(neighbor))
					continue;

				int alt = distance[node] + 1;

				if(alt < distance[neighbor])
				{
					distance[neighbor] = alt;
					previous[neighbor] = node;
				}
			}
		}

		List<MovePointBehavior> path = new List<MovePointBehavior>();
		MovePointBehavior u = targetNode;

		while (previous[u] != null)
		{
			path.Add(u);
			u = previous[u];
		}

		path.Reverse();

		return path;
	}

	/// <summary>
	/// Enables the renderer on any nodes the unit can move to.
	/// </summary>
	/// <param name="actor">Actor associated with this movement attempt.</param>
	/// <param name="grid">Grid associated with the movement.</param>
	/// <param name="range">Maximum distance (in grid squares) to highlight.</param>
    public void HighlightValidNodes(ActorBehavior actor, GridBehavior grid, int range = -1)
    {
		int depth = 0;

		if(actor.currentMovePoint == null)
		{
			Debug.LogError("Current move point is null!");
			return;
		}

		//int maxDistance = 0;
		CombatSquadBehavior csb = actor.GetComponent<CombatSquadBehavior>();
		if (csb == null)
			Debug.LogWarning("Attempting to move a unit that does not have a squad associated!");

		bool skipIgnoreList = false;

		if(range == -1)
			range = (csb == null ? 1 : csb.Squad.Speed);
		else
			skipIgnoreList = true;

		List<MovePointBehavior> moveGraph = new List<MovePointBehavior>();

		actor.currentMovePoint.BuildGraph(range, depth, grid, ref moveGraph, skipIgnoreList);
		moveGraph.RemoveAt(0);

		foreach (MovePointBehavior node in moveGraph)
			node.renderer.enabled = true;
    }

	/// <summary>
	/// Performs the logic behind the depth-first search.
	/// </summary>
	/// <param name="maxDepth">Maximum depth to perform checking to.</param>
	/// <param name="currentDepth">Current depth within the search.</param>
	/// <param name="grid">Grid the graph is being built on.</param>
	/// <param name="path">List of move points representing the constructed path.</param>
	public void BuildGraph(int maxDepth, int currentDepth, GridBehavior grid, ref List<MovePointBehavior> path)
	{
		BuildGraph(maxDepth, currentDepth, grid, ref path, false);
	}

	/// <summary>
	/// Performs the logic behind the depth-first search.
	/// </summary>
	/// <param name="maxDepth">Maximum depth to perform checking to.</param>
	/// <param name="currentDepth">Current depth within the search.</param>
	/// <param name="grid">Grid the graph is being built on.</param>
	/// <param name="path">List of move points representing the constructed path.</param>
	/// <param name="skipIgnoreList">Whether or not the ignore list check should be skipped.</param>
	public void BuildGraph(int maxDepth, int currentDepth, GridBehavior grid, ref List<MovePointBehavior> path, bool skipIgnoreList)
	{
		if (currentDepth >= maxDepth || neighborList.Length == 0)
			return;

		if (!path.Contains(this))
			path.Add(this);

		currentDepth++;

		foreach (MovePointBehavior neighbor in neighborList)
		{
			if (neighbor == null)
				continue;

			if (!skipIgnoreList && grid.ignoreList != null && grid.ignoreList.Contains(neighbor))
				continue;

			//neighbor.renderer.enabled = true;
			if (!path.Contains(neighbor))
				path.Add(neighbor);

			neighbor.BuildGraph(maxDepth, currentDepth, grid, ref path, skipIgnoreList);
		}
	}
}
