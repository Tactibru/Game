using UnityEngine;
using UnityEditor;
using System.Collections;
using Editor.Util;
using NodeSkeletonSystem;

namespace Editor.NodeSkeletonSystem
{
	/// <summary>
	/// Unity editor window that allows for the creation and modification of Node Skeleton Structure (NSS) files.
	/// 
	/// Author: Ken Murray
	/// </summary>
	public class NSSEditor : EditorWindow
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
		
		/// <summary>
		/// Menu item (Tactibru -> Node Skeleton System -> NSS Editor) that the NSS Editor window.
		/// </summary>
		[MenuItem("Tactibru/Node Skeleton System/NSS Editor", false, 20)]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow (typeof(NSSEditor));
		}
		/** END MENU ITEMS **/
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Editor.NodeSkeletonSystem.NSSEditor"/> class.
		/// </summary>
		public NSSEditor()
		{
			title = "NSS Editor";
		}
	}
}