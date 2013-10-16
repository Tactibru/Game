using UnityEngine;
using UnityEditor;
using System.Collections;
using NodeSkeletonSystem;

namespace Units
{
	/// <summary>
	/// Represents a Unit asset within the game.
	/// </summary>
	public class CombatUnit : ScriptableObject
	{
		/// <summary>
		/// Squad space occupied by this unit.
		/// </summary>
		public enum UnitSpace
		{
			OneByOne,
			OneByTwo,
			TwoByOne,
			TwoByTwo
		}

		/// <summary>
		/// Represents the <see cref="NodeSkeletonSystem.NodeSkeletonStructure"/> that will be used for this unit.
		/// </summary>
		public NodeSkeletonStructure Skeleton;

		/// <summary>
		/// Represents the directory (within the Unit Art Assets directory) where the unit's art assets will be located.
		/// </summary>
		/// <example>
		/// Assuming a "Ruffian" is set, the assets should be:
		/// Textures\Units\Heads\Ruffian_001.png
		/// Textures\Units\Heads\Ruffian_002.png
		/// etc.
		/// </example>
		public string AssetDirectory = "Ruffian";
		
		/// <summary>
		/// Unit name.
		/// </summary>
		public string Name = "Combat Unit";
		
		/// <summary>
		/// Unit's total health value.
		/// </summary>
		public int Health = 10;
		
		/// <summary>
		/// Unit Attack Strength.
		/// </summary>
		public int Strength = 5;
		
		/// <summary>
		/// Unit Attack Resistance.
		/// </summary>
		public int Toughness = 5;
		
		/// <summary>
		/// Unit's contribution to the <see cref="Units.CombatSquad" /> movement speed.
		/// </summary>
		public int Speed = 5;
		
		/// <summary>
		/// Unit's attack range (contributes to <see cref="Units.CombatSquad" /> attack range.
		/// </summary>
		public int Range = 1;
		
		/// <summary>
		/// Unit's Upkeep Cost.
		/// </summary>
		public int Cost = 0;
		
		/// <summary>
		/// Unit's adjustment to the player's starting honor.
		/// </summary>
		public int HonorMod = 0;
		
		/// <summary>
		/// Amount of space occupied by the unit.
		/// </summary>
		public UnitSpace Space = UnitSpace.OneByOne;
		
		/// <summary>
		/// Number of Squad seats taken up by this unit.
		/// </summary>
		public int UnitSize = 1;
	}
}