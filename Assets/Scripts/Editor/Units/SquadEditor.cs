using Editor.Util;
using Units;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Editor.Units
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
		/// Implements the GUI displayed in the inspector, so that items can be entered by value.
		/// </summary>
		public override void OnInspectorGUI()
		{
			
		}
	}
}