using UnityEngine;
using System.Collections;
using NodeSkeletonSystem;

/// <summary>
/// Represents a quad that utilises a Node Skeleton Structure.
/// 
/// Author: Ken Murray
/// </summary>
public class NodeSkeletonBehavior : MonoBehaviour
{
	/// <summary>
	/// Node skeleton structure defining this object's attachment nodes.
	/// </summary>
	public NodeSkeletonStructure SkeletonStructure;

	/// <summary>
	/// Attaches an object to the specified node.
	/// </summary>
	/// <param name="nodeName">Name of the node to attach to.</param>
	/// <param name="prefab">Prefab for the object to be attached.</param>
	/// <returns>Whether or not the attachment was a success - failure indicates that the node does not exist.</returns>
	public bool AttachToNode(string nodeName, GameObject prefab)
	{
		// Verify that the prefab is set.
		if (prefab == null)
		{
			Debug.LogWarning("Warning: Attempted to attach a null prefab to a NodeSkeletonBehavior.");
			return false;
		}

		// Verify that the skeleton structure is set.
		if (SkeletonStructure == null)
		{
			Debug.LogWarning("Warning: Attempted to attach a node to a NodeSkeletonBehavior with no skeleton structure defined!");
			return false;
		}

		// Check for the node name.
		if (!SkeletonStructure.ContainsNode(nodeName))
		{
			Debug.LogWarning("Warning: Attempted to retrieve a node (" + nodeName + ") that does not exist!");
			return false;
		}

		// Retrieve the node, then get its location.
		NSSNode node = SkeletonStructure.GetNode(nodeName);

		// Instantiate the object, then set its local position, scale and rotation.
		GameObject subObject = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
		subObject.transform.parent = transform;
		subObject.transform.localPosition = node.Offset;
		//subObject.transform.localScale = Vector3.one;
		//subObject.transform.localRotation = Quaternion.identity;

		return true;
	}
}
