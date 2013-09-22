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

		/// <summary>
		/// Texture used as a "blank" texture if the preview texture is not set.
		/// </summary>
		private Texture2D blankTexture = new Texture2D(200, 200);

		/// <summary>
		/// Marker texture, used to draw the "points".
		/// </summary>
		private static Texture markerTexture;

		/// <summary>
		/// Offset of the label from the point it is labeling.
		/// </summary>
		private const int LABEL_OFFSET = 8;

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
		/// Loads the marker texture when the editor is enabled.
		/// </summary>
		public void OnEnable()
		{
			// Load the Marker rect.
			if (markerTexture == null)
				markerTexture = (Texture)Resources.Load("Editor/Marker");
		}
		
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
					drawMarkerWithLabel(previewRect, Vector2.zero, "Origin", Color.red);

					// Draw the remaining nodes.
					foreach(NSSNode node in Target.Nodes)
						drawMarkerWithLabel(previewRect, node.Offset, node.Name, Color.black);
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

			base.OnInspectorGUI();

			// Set the target as dirty if the GUI values have changed.
			if (GUI.changed)
				EditorUtility.SetDirty(Target);
		}

		/// <summary>
		/// Draws a marker at the specified point within baseRect, with a label for it.
		/// </summary>
		/// <param name="baseRect">Rect in which the label was drawn.</param>
		/// <param name="point">Point within the rect to draw the marker.</param>
		/// <param name="label">Label to apply to the marker.</param>
		/// <param name="color">Color to use for the marker.</param>
		private void drawMarkerWithLabel(Rect baseRect, Vector2 point, string label, Color color)
		{
			// Store the current GUI color.
			Color guiColor = GUI.color;
			GUI.color = color;

			// Draw the marker.
			Rect markerRect = new Rect(baseRect.x + ((baseRect.width - markerTexture.width) / 2) + (point.x * baseRect.width),
				baseRect.y + ((baseRect.height - markerTexture.height) / 2) - (point.y * baseRect.height),
				markerTexture.width,
				markerTexture.height);
			EditorGUI.DrawTextureTransparent(markerRect, markerTexture);

			// Draw the label.
			Vector2 labelSize = GUI.skin.label.CalcSize(new GUIContent(label));
			Rect labelRect = new Rect(baseRect.x + ((baseRect.width - labelSize.x) / 2) + (point.x * baseRect.width),
				baseRect.y + ((baseRect.height - labelSize.y) / 2) + (labelSize.y - LABEL_OFFSET) - (point.y * baseRect.height),
				labelSize.x,
				labelSize.y);
			GUI.Label(labelRect, label);

			// Restore the GUI color
			GUI.color = guiColor;
		}
	}
}