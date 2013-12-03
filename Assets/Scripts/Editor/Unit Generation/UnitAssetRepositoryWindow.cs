using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Tactibru.Editor.Util;
using NodeSkeletonSystem;
using Units;

namespace Tactibru.Editor.UnitGeneration
{
	/// <summary>
	/// Provides utility methods for interacting with the unit asset repository.
	/// </summary>
	public class UnitAssetRepositoryWindow : EditorWindow
	{
		/// <summary>
		/// Stores a list of booleans to determine whether or not the foldouts are shown.
		/// </summary>
		public List<bool> foldoutShown = new List<bool>();
		
		/// <summary>
		/// Represents the name of a new group to be created.
		/// </summary>
		private string newGroupName = "New Asset Group";

		/// <summary>
		/// Opens the unit asset repository window.
		/// </summary>
		[MenuItem("Tactibru/Unit Asset Repository")]
		public static void ShowAssetRepository()
		{
			EditorWindow.GetWindow<UnitAssetRepositoryWindow>();
		}

		/// <summary>
		/// Sets the title for the repository window.
		/// </summary>
		public UnitAssetRepositoryWindow()
		{
			title = "Unit Assets";
		}
		
		/// <summary>
		/// Renders the GUI controls for editing repositories.
		/// </summary>
		public void OnGUI()
		{
			while(foldoutShown.Count < UnitAssetRepository.Instance.assetGroups.Count)
				foldoutShown.Add(true);

			// Display the "Create New Asset Group" button.
			EditorGUILayout.BeginHorizontal();
			{
				newGroupName = EditorGUILayout.TextField (newGroupName);
				
				if(GUILayout.Button ("Create new Asset Group"))
				{
					UnitAssetRepository.Instance.assetGroups.Add (new UnitAssetGroup(newGroupName));
					foldoutShown.Add (true);
					
					newGroupName = "New Asset Group";
				}
			}
			EditorGUILayout.EndHorizontal();
			
			// Display all of the asset groups.
			for(int _i = 0; _i < UnitAssetRepository.Instance.assetGroups.Count; _i++)
			{
				UnitAssetGroup group = UnitAssetRepository.Instance.assetGroups[_i];

				EditorGUILayout.BeginHorizontal();
				{
					foldoutShown[_i] = EditorGUILayout.Foldout (foldoutShown[_i], group.Name);
					if(GUILayout.Button ("X", GUILayout.Width (20)))
					{
						UnitAssetRepository.Instance.assetGroups.RemoveAt (_i);
						foldoutShown.RemoveAt (_i);
						continue;
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginVertical();
				{
					if(foldoutShown[_i])
					{
						if(GUILayout.Button ("Add Asset"))
							group.prefabs.Add (null);
						
						for(int _j = 0; _j < group.prefabs.Count; _j++)
						{
							EditorGUILayout.BeginHorizontal();
							{
								group.prefabs[_j] = (UnitAssetBehavior)EditorGUILayout.ObjectField(group.prefabs[_j], typeof(UnitAssetBehavior), false);

								if(GUILayout.Button ("X", GUILayout.Width (20)))
									group.prefabs.RemoveAt (_j);
							}
							EditorGUILayout.EndHorizontal();
						}
					}
				}
				EditorGUILayout.EndVertical();
			}

			// Set the target as dirty if the GUI values have changed.
			if (GUI.changed)
				EditorUtility.SetDirty(UnitAssetRepository.Instance);
		}
	}
}