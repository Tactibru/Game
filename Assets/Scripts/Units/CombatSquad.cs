using UnityEngine;
using System.Collections.Generic;

namespace Units
{
	/// <summary>
	/// Represents a Squad within the game, composed of up to five <see cref="Units.CombatUnit" />s.
	/// </summary>
	public class CombatSquad : ScriptableObject
	{
		/// <summary>
		/// Maximum number of combat units per squad.
		/// </summary>
		private const int MAX_UNITS_PER_SQUAD = 5;
		
		/// <summary>
		/// Flags the squad as "dirty", forcing the speed to be recalculated.
		/// </summary>
		public bool IsDirty = true;
		
		/// <summary>
		/// Units contained within the squad.
		/// </summary>
		public List<CombatUnit> Units = new List<CombatUnit>(MAX_UNITS_PER_SQUAD);
		
		/// <summary>
		/// Caches the unit's speed, to prevent unnecessary iteration.
		/// </summary>
		private int cachedSpeed = 0;
		
		/// <summary>
		/// Retrieves the speed of the squad, either through the cache or by recalculating if dirty.
		/// </summary>
		public int Speed
		{
			get
			{
				// Recalculate the speed if the cache is invalid or we are dirty.
				if(!IsDirty || cachedSpeed <= 0)
				{
					cachedSpeed = 0;
					
					foreach(CombatUnit unit in Units)
						cachedSpeed += unit.Speed;
					
					cachedSpeed /= Units.Count;
				}
				
				return cachedSpeed;
			}
		}
		
		/// <summary>
		/// Retrieves the number of units currently assigned to this squad.
		/// </summary>
		public int Size
		{
			get
			{
				return Units.Count;
			}
		}
		
		/// <summary>
		/// Attempts to add the specified unit to the squad.
		/// </summary>
		/// <returns>
		/// Whether or not the unit was successfully added.
		/// </returns>
		public bool AddUnit(CombatUnit unit)
		{
			if((Units.Count + 1) >= MAX_UNITS_PER_SQUAD)
				return false;
			
			Units.Add (unit);
			
			return true;
		}
		
		/// <summary>
		/// Allows a combat unit to be retrieved based on its index.
		/// </summary>
		public CombatUnit this[int idx]
		{
			get
			{
				if(idx < 0 || idx >= Units.Count)
					return null;
				
				return Units[idx];
			}
		}
	}
}