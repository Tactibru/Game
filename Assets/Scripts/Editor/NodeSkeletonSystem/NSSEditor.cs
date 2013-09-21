using UnityEngine;
using UnityEditor;
using System.Collections;
using Editor.Util;
using NodeSkeletonSystem;

namespace Editor.NodeSkeletonSystem
{
	/// <summary>
	/// Unity editor that allows for the modification of Node Skeleton Structure (NSS) files.
	/// 
	/// Author: Ken Murray
	/// </summary>
	/// 
	[CustomEditor(typeof(NodeSkeletonStructure))]
	public class NSSEditor : EditorBase<NodeSkeletonStructure>
	{
		/** MENU ITEMS **/
		/// <summary>
		/// Creates a new NSS file.
		/// </summary>
		[MenuItem("Assets/Create/Tactibru/Node Skeleton Structure", false, 0)]
		public static void CreateNewNSSFile()
		{
			CustomAssetUtility.CreateAsset<NodeSkeletonStructure>();
		}
		/** END MENU ITEMS **/
		
		/// <summary>
		/// Implements the custom editor's Inspector window GUI.
		/// </summary>
		public override void OnInspectorGUI()
		{
		}
	}
}