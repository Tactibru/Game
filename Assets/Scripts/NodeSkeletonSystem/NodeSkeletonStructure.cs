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
	}
}