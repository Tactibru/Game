using System;
using UnityEngine;

namespace Units
{
	/// <summary>
	/// Provides information regarding unit position and size within a squad.
	/// </summary>
	[Serializable]
	public class UnitData
	{
		/// <summary>
		/// Contains the actual unit information.
		/// </summary>
		public CombatUnit Unit;

		/// <summary>
		/// Unit's position within the squad.
		/// </summary>
		public UnitPosition Position;
	}
}
