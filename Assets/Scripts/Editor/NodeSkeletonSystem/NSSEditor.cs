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
		/// Implements the GUI displayed in the inspector, so that items can be entered by value.
		/// </summary>
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			// Set the target as dirty if the GUI values have changed.
			if(GUI.changed)
				EditorUtility.SetDirty(Target);
		}
		
		/// <summary>
		/// Mark this editor has having a preview GUI.
		/// </summary>
		public override bool HasPreviewGUI()
		{
			return true;
		}
		
		/// <summary>
		/// Renders a quad, potentially with a preview texture, with the individual points and names representing the nodes.
		/// </summary>
		public void OnPreviewGUI()
		{
		}
	}
}