using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Editor.NodeSkeletonSystem
{
	/// <summary>
	/// Unity editor window that allows for the creation and modification of Node Skeleton Structure (NSS) files.
	/// 
	/// Author: Ken Murray
	/// </summary>
	public class NSSEditor : EditorWindow
	{
		/// <summary>
		/// Menu item (Tactibru -> Node Skeleton System -> NSS Editor) that the NSS Editor window.
		/// </summary>
		[MenuItem("Tactibru/Node Skeleton System/NSS Editor")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow (typeof(NSSEditor));
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Editor.NodeSkeletonSystem.NSSEditor"/> class.
		/// </summary>
		public NSSEditor()
		{
			title = "NSS Editor";
		}
	}
}