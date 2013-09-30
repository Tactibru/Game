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
		/// Stores the position of a unit in the squad.
		/// </summary>
		/// <remarks>
		/// This is laid out assuming two rows of five columns each.
		/// </remarks>
		public struct UnitPosition
		{
			/// <summary>
			/// Column the unit sits in.
			/// </summary>
			/// <remarks>
			/// If the unit occupies 2 columns, this will be the leftmost column.
			/// </remarks>
			public int Column;

			/// <summary>
			/// Row the unit sits in.
			/// </summary>
			/// <remarks>
			/// If the unit occupies 2 rows, this will be ignored.
			/// </remarks>
			public int Row;
		}

		/// <summary>
		/// Provides information regarding unit position and size within a squad.
		/// </summary>
		public struct UnitData
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

		/// <summary>
		/// Maximum number of combat units per squad.
		/// </summary>
		public const int MAX_UNITS_PER_SQUAD = 5;
		
		/// <summary>
		/// Flags the squad as "dirty", forcing the speed to be recalculated.
		/// </summary>
		public bool IsDirty = true;
		
		/// <summary>
		/// Units contained within the squad.
		/// </summary>
		public List<UnitData> Units = new List<UnitData>(MAX_UNITS_PER_SQUAD);

		/// <summary>
		/// Maintains which positions within the squad are occupied.
		/// </summary>
		private bool[,] occupied = new bool[5, 2];
		
		/// <summary>
		/// Caches the unit's speed, to prevent unnecessary iteration.
		/// </summary>
		private int cachedSpeed = 0;

		/// <summary>
		/// Caches the unit's size, to prevent unnecessary iteration.
		/// </summary>
		private int cachedSize = 0;
		
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

					foreach (UnitData unitData in Units)
						cachedSpeed += unitData.Unit.Speed;
					
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
				if (!IsDirty || cachedSize <= 0)
				{
					cachedSize = 0;

					foreach (UnitData unitData in Units)
						cachedSize += unitData.Unit.UnitSize;
				}

				return cachedSize;
			}
		}

		/// <summary>
		/// Initializes the occupied space.
		/// </summary>
		public CombatSquad()
		{
			for (int _x = 0; _x < 5; _x++)
				for (int _y = 0; _y < 2; _y++)
					occupied[_x, _y] = false;
		}

		/// <summary>
		/// Attempts to add the specified unit to the squad.
		/// </summary>
		/// <param name="unit">Unit being added to the squad.</param>
		/// <param name="position">Position within the squad to place the unit.</param>
		/// <returns>
		/// Whether or not the unit was successfully added.
		/// </returns>
		public bool AddUnit(CombatUnit unit, UnitPosition position)
		{
			// Verify that the unit will fit within the squad.
			if((Size + unit.UnitSize) >= MAX_UNITS_PER_SQUAD)
				return false;

			if (!IsPositionValid(unit, position))
				return false;

			UnitData unitData;
			unitData.Unit = unit;
			unitData.Position = position;

			Units.Add(unitData);
			
			return true;
		}

		/// <summary>
		/// Verifies that the unit can be placed in the specified position.
		/// </summary>
		/// <param name="unit">Unit being added to the squad.</param>
		/// <param name="position">Position within the squad to place the unit.</param>
		/// <returns>
		/// Whether or not the unit will be able to be added to the squad in this position.
		/// </returns>
		public bool IsPositionValid(CombatUnit unit, UnitPosition position)
		{
			bool twoRows = ((unit.Space == CombatUnit.UnitSpace.OneByTwo) || (unit.Space == CombatUnit.UnitSpace.TwoByTwo));
			bool twoColumns = ((unit.Space == CombatUnit.UnitSpace.TwoByOne) || (unit.Space == CombatUnit.UnitSpace.TwoByTwo));

			// Verify that the position data is valid.
			if (position.Row < 0 ||
				(twoRows && (position.Row > 0)) ||
				(!twoRows && (position.Row > 1)))
				return false;

			if (position.Column < 0 ||
				(twoColumns && (position.Column > 3)) ||
				(!twoColumns && (position.Column > 4)))
				return false;

			// Verify that the proposed space is not occupied.
			if (occupied[position.Row, position.Column] ||
				(twoRows && occupied[position.Row + 1, position.Column]) ||
				(twoColumns && occupied[position.Row, position.Column + 1]) ||
				(twoRows && twoColumns && occupied[position.Row + 1, position.Column + 1]))
				return false;

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
				
				return Units[idx].Unit;
			}
		}
	}
}