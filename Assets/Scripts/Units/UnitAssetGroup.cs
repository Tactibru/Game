using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
	/// <summary>
	/// Represents a group from which assets can be retrieved (e.g. "Heads")
	/// </summary>
	[Serializable]
	public class UnitAssetGroup
	{
		/// <summary>
		/// Represents the name of the asset group.
		/// </summary>
		public string Name = string.Empty;

		/// <summary>
		/// Stores a list of prefabs owned by this asset group.
		/// </summary>
		public List<UnitAssetBehavior> prefabs = new List<UnitAssetBehavior>();

		/// <summary>
		/// Retrieves a random prefab from the list of prefabs in the group.
		/// </summary>
		/// <returns>The random prefab.</returns>
		public UnitAssetBehavior getRandomPrefab()
		{
			if(prefabs == null)
			{
				Debug.LogError ("Attempted to retrieve a prefab from an empty asset group!");
				return null;
			}

			return null;
		}
	}
}
