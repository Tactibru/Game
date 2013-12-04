using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units
{
	/// <summary>
	/// Represents a group from which assets can be retrieved (e.g. "Heads")
	/// </summary>
	[System.Serializable]
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
		/// Stores the asset group.
		/// </summary>
		/// <param name="name">Name.</param>
		public UnitAssetGroup(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Retrieves a random prefab from the list of prefabs in the group.
		/// </summary>
		/// <returns>The random prefab.</returns>
		public UnitAssetBehavior getRandomPrefab()
		{
			if(prefabs.Count == 0)
			{
				Debug.LogError ("Attempted to retrieve a prefab from an empty asset group!");
				return null;
			}

			return prefabs[Random.Range (0, prefabs.Count)];
		}

		/// <summary>
		/// Retrieves a prefab from the list of prefabs in the group, matching a specific name.
		/// </summary>
		/// <returns>The prefab by name.</returns>
		/// <param name="name">Name.</param>
		public UnitAssetBehavior getPrefabByName(string name)
		{
			if(prefabs.Count == 0)
			{
				Debug.LogError ("Attempted to retrieve a prefab from an empty asset group!");
				return null;
			}

			IEnumerable<UnitAssetBehavior> assets = prefabs.Where (l => l.Name == name);

			if(assets.Count () == 0)
				return getRandomPrefab();

			return assets.First ();
		}
	}
}
