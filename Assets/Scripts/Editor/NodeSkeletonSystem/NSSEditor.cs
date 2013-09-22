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
		/// <summary>
		/// Texture used for the preview window.
		/// </summary>
		private Texture2D previewBaseTexture;

		private Texture2D blankTexture = new Texture2D(200, 200);

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
			// Draw the preview area.
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();

				// Render the selected preview texture (or a blank texture if it's null).
				Rect previewRect = EditorGUILayout.BeginVertical(GUILayout.Height(200), GUILayout.Width(200));
				{
					GUILayout.FlexibleSpace();

					// Draw the actual texture.
					if (previewBaseTexture != null)
						EditorGUI.DrawTextureTransparent(previewRect, previewBaseTexture);
					else
						EditorGUI.DrawPreviewTexture(previewRect, blankTexture);

					// Draw the origin.
				}
				EditorGUILayout.EndVertical();

				GUILayout.FlexibleSpace();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label("Preview Texture:");
				previewBaseTexture = (Texture2D)EditorGUILayout.ObjectField(previewBaseTexture, typeof(Texture2D), false);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Separator();

			// Set the target as dirty if the GUI values have changed.
			if (GUI.changed)
				EditorUtility.SetDirty(Target);
		}
	}
}