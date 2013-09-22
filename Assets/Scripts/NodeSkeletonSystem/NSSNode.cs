using UnityEngine;

namespace NodeSkeletonSystem
{
	/// <summary>
	/// Represents a "node" to which an animated quad can be attached.
	/// </summary>
	[System.Serializable]
	public class NSSNode
	{
		/// <summary>
		/// Unique identifier ("name") for the individual node, such as 'head' or 'r-arm'.
		/// </summary>
		public string Name;
		
		/// <summary>
		/// Offset of the node based on the Skeleton's origin.
		/// </summary>
		public Vector3 Offset;

		/// <summary>
		/// Default constructor, blank.
		/// </summary>
		public NSSNode()
			: base()
		{
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="name">Name of the node.</param>
		public NSSNode(string name)
			: base()
		{
			Name = name;
		}
	}
}

