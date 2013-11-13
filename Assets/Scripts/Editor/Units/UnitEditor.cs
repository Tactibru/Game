using Editor.Util;
using Units;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Editor.Units
{
	/// <summary>
	/// Unity editor that allows for the modification of Combat Units.
	/// </summary>
	[CustomEditor(typeof(CombatUnit))]
	public class UnitEditor : EditorBase<CombatUnit>
	{
		#region Menu Items
		/// <summary>
		/// Creates a new Unit asset.
		/// </summary>
		[MenuItem("Assets/Create/Tactibru/Combat/Unit", false, 0)]
		public static void CreateUnit()
		{
			CustomAssetUtility.CreateAsset<CombatUnit>();
		}
		#endregion
		
		/// <summary>
		/// Whether or not the General category should be shown.
		/// </summary>
		private bool showGeneralCategory = true;
		
		/// <summary>
		/// Whether or not the Stats category should be shown.
		/// </summary>
		private bool showStatsCategory = true;
		
		/// <summary>
		/// Implements the GUI displayed in the inspector, so that items can be entered by value.
		/// </summary>
		public override void OnInspectorGUI()
		{
			// [General] -> General, non-stat data for the Unit.
			showGeneralCategory = EditorGUILayout.Foldout(showGeneralCategory, "General");
			
			if(showGeneralCategory)
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("Name");
					Target.Name = GUILayout.TextField(Target.Name);
				}
				GUILayout.EndHorizontal();
				
				Target.HonorMod = EditorGUILayout.IntSlider (new GUIContent("Honor Mod", "Honor modification value."), Target.HonorMod, -10, 10);
				labeledIntField("Upkeep Cost", ref Target.Cost);
				Target.UnitSize = EditorGUILayout.IntSlider (new GUIContent("Unit Size", "Number of 'slots' this unit occupies in a squad."), Target.UnitSize, 1, CombatSquad.MAX_UNITS_PER_SQUAD);
				Target.Space = (CombatUnit.UnitSpace)EditorGUILayout.EnumPopup(new GUIContent("Unit Space", "Amount of space on the 5x2 Grid occupied by this unit."), Target.Space);
				Target.Weapon = (CombatUnit.UnitWeapon)EditorGUILayout.EnumPopup (new GUIContent("Weapon", "Weapon the unit wields in combat."), Target.Weapon);
			}
			
			// [Stats] -> Unit Statistics.
			showStatsCategory = EditorGUILayout.Foldout(showStatsCategory, "Stats");
			
			if(showStatsCategory)
			{
				labeledIntField("Health", ref Target.Health);
				labeledIntField("Strength", ref Target.Strength);
				labeledIntField("Toughness", ref Target.Toughness);
				labeledIntField("Speed", ref Target.Speed);
				labeledIntField("Range", ref Target.Range);
			}

			// Set the target as dirty if the GUI values have changed.
			if (GUI.changed)
			{
				Target.CurrentHealth = Target.Health;
				EditorUtility.SetDirty(Target);
			}
		}
		
		/// <summary>
		/// Utility method that displays an integer field in the editor.
		/// </summary>
		private void labeledIntField(string label, ref int value)
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(label);
				value = EditorGUILayout.IntField(value, GUILayout.Width(50));
			}
			GUILayout.EndHorizontal();
		}
	}
}