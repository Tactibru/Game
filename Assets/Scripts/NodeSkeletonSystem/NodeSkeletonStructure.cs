using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NodeSkeletonSystem
{
	/// <summary>
	/// Defines the structure of a node skeleton that can be attached to a game object via the
	/// <see cref="NodeSkeletonBehavior" /> behavior script.
	/// </summary>
	public class NodeSkeletonStructure : ScriptableObject
	{
		/// <summary>
		/// Contains a list of nodes associated with this skeleton structure.
		/// </summary>
		public List<NSSNode> Nodes;

		/// <summary>
		/// Iterates over the nodes to determine if a specific node exists.
		/// </summary>
		/// <param name="nodeName">Name of the node to locate.</param>
		/// <returns>Whether or not the node exists.</returns>
		public bool ContainsNode(string nodeName)
		{
			foreach (NSSNode node in Nodes)
				if (node.Name == nodeName)
					return true;

			return false;
		}

		/// <summary>
		/// Retrieves a node based on the given name.
		/// </summary>
		/// <param name="nodeName">Name of the node to retrieve.</param>
		/// <returns>The node, if it exists. null, otherwise.</returns>
		public NSSNode GetNode(string nodeName)
		{
			foreach (NSSNode node in Nodes)
				if (node.Name == nodeName)
					return node;

			return null;
		}
	}
}