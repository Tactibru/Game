using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
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
		private Texture markerTexture;

		/// <summary>
		/// Offset of the label from the point it is labeling.
		/// </summary>
		private const int LABEL_OFFSET = 8;

		/// <summary>
		/// List of states for each individual node.
		/// </summary>
		private List<bool> nodesShown;

		/// <summary>
		/// List of custom colors for each individual node.
		/// </summary>
		private List<Color> nodeColors;

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
			markerTexture = (Texture)Resources.Load("Editor/Marker");

			// Node Lists
			nodesShown = new List<bool>();
			nodeColors = new List<Color>();
		}

		/// <summary>
		/// Deletes the blank texture whenever the editor is disabled.
		/// </summary>
		public void OnDisable()
		{
			if (markerTexture != null)
			{
				Destroy(markerTexture);
				markerTexture = null;
			}

			if (blankTexture != null)
			{
				Destroy(blankTexture);
				blankTexture = null;
			}
		}
		
		/// <summary>
		/// Implements the GUI displayed in the inspector, so that items can be entered by value.
		/// </summary>
		public override void OnInspectorGUI()
		{
			while (nodesShown.Count < Target.Nodes.Count)
				nodesShown.Add(true);

			while (nodeColors.Count < Target.Nodes.Count)
				nodeColors.Add(Color.black);

			// Draw the preview area.
			GUILayout.Space(10);
			EditorGUILayout.BeginHorizontal(GUILayout.Height(215));
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
					for(int _i = 0; _i < Target.Nodes.Count; _i++)
						drawMarkerWithLabel(previewRect, Target.Nodes[_i].Offset, Target.Nodes[_i].Name, nodeColors[_i]);
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

			GUILayout.Space(20);

			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.LabelField("Node Count:", Target.Nodes.Count.ToString());
			}
			EditorGUILayout.EndHorizontal();

			// Begin editing the nodes.
			for (int _i = 0; _i < Target.Nodes.Count; _i++)
				insertNodeEditor(_i);

			GUILayout.Space(20);

			// Add a new node.
			if (GUILayout.Button("Add Node"))
			{
				Target.Nodes.Add(new NSSNode("Node " + Target.Nodes.Count));
			}

			// Set the target as dirty if the GUI values have changed.
			if (GUI.changed)
				EditorUtility.SetDirty(Target);
		}

		/// <summary>
		/// Inserts an editor for a node with the specified index.
		/// </summary>
		/// <param name="nodeIndex"></param>
		private void insertNodeEditor(int nodeIndex)
		{
			bool markedForRemoval = false;

			EditorGUILayout.BeginHorizontal();
			{
				nodesShown[nodeIndex] = EditorGUILayout.Foldout(nodesShown[nodeIndex], Target.Nodes[nodeIndex].Name);
				GUILayout.FlexibleSpace();

				// Delete the node if the X button is pressed.
				if (GUILayout.Button("X"))
				{
					markedForRemoval = true;
				}
			}
			EditorGUILayout.EndHorizontal();

			if (nodesShown[nodeIndex])
			{
				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.Label("Name");
					Target.Nodes[nodeIndex].Name = GUILayout.TextField(Target.Nodes[nodeIndex].Name);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.Label("Preview Color");
					nodeColors[nodeIndex] = EditorGUILayout.ColorField(nodeColors[nodeIndex]);
				}
				EditorGUILayout.EndHorizontal();

				Target.Nodes[nodeIndex].Offset = EditorGUILayout.Vector3Field("Offset", Target.Nodes[nodeIndex].Offset);
			}

			if(markedForRemoval)
				Target.Nodes.RemoveAt(nodeIndex);
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