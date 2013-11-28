using Tactibru.Editor.Util;
using Units;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Tactibru.Editor.Units
{
	/// <summary>
	/// Unity editor that allows for the modification of Combat Squads.
	/// </summary>
	[CustomEditor(typeof(CombatSquad))]
	public class SquadEditor : EditorBase<CombatSquad>
	{
		#region Menu Items
		/// <summary>
		/// Creates a new Squad asset.
		/// </summary>
		[MenuItem("Assets/Create/Tactibru/Combat/Squad", false, 0)]
		public static void CreateSquad()
		{
			CustomAssetUtility.CreateAsset<CombatSquad>();
		}
		#endregion

		/// <summary>
		/// Stores whether or not the foldouts for the units in the squad are expanded or not.
		/// </summary>
		private bool[] showUnit = new bool[CombatSquad.MAX_UNITS_PER_SQUAD];

		/// <summary>
		/// Unit prefab to be added to the squad.
		/// </summary>
		private CombatUnit unitPrefab = null;

		/// <summary>
		/// Position within the squad to add the potential unit.
		/// </summary>
		private UnitPosition unitPosition = new UnitPosition();

		/// <summary>
		/// Row in which the unit appears.
		/// </summary>
		private enum UnitRow
		{
			Front = 0,
			Back = 1,
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private void unitPositionEditor(ref UnitPosition position)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.BeginVertical();
				{
					GUILayout.Label("Column");
					//position.Column = EditorGUILayout.IntField(position.Column);
					position.Column = EditorGUILayout.IntSlider (position.Column, 0, 4);
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();
				{
					UnitRow row = (UnitRow)position.Row;
					GUILayout.Label("Row");
					position.Row = (int)((UnitRow)EditorGUILayout.EnumPopup(row));

				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}
		
		/// <summary>
		/// Implements the GUI displayed in the inspector, so that items can be entered by value.
		/// </summary>
		public override void OnInspectorGUI()
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Unit Count:");
				GUILayout.FlexibleSpace();
				EditorGUILayout.SelectableLabel(Target.Units.Count.ToString(), GUILayout.Width(50));
			}
			GUILayout.EndHorizontal();

			for (int _i = 0; _i < Target.Units.Count; _i++)
			{
				GUILayout.BeginHorizontal();
				{
					string foldoutName = string.Format("{0} ({1}, {2})", Target.Units[_i].Unit.Name, Target.Units[_i].Position.Row, Target.Units[_i].Position.Column);
					showUnit[_i] = EditorGUILayout.Foldout(showUnit[_i], foldoutName);
					GUILayout.FlexibleSpace();

					if (GUILayout.Button("X"))
						Target.Units.RemoveAt(_i);
				}
				GUILayout.EndHorizontal();

				if (showUnit[_i])
				{
					Target.Units[_i].Unit = (CombatUnit)EditorGUILayout.ObjectField(Target.Units[_i].Unit, typeof(CombatUnit), false);
					unitPositionEditor(ref Target.Units[_i].Position);
				}
			}

			EditorGUILayout.HelpBox("Set the unit asset and position here, then click Add Unit to add the unit to the squad.", MessageType.Info);

			unitPrefab = (CombatUnit)EditorGUILayout.ObjectField(unitPrefab, typeof(CombatUnit), false);
			unitPositionEditor(ref unitPosition);

			if (GUILayout.Button("Add Unit"))
				if (unitPrefab != null && Target.IsPositionValid(unitPrefab, unitPosition))
				{
					if (!Target.AddUnit(unitPrefab, unitPosition))
						Debug.Log("Couldn't add unit.");
				}
				else
					Debug.LogWarning("Attempted to add invalid unit.");

			// Set the target as dirty if the GUI values have changed.
			if (GUI.changed)
			{
				EditorUtility.SetDirty(Target);
				Debug.Log("Squad saved.");
			}
		}
	}
}