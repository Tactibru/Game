using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units
{
	/// <summary>
	/// Provides a repository from which unit assets can be assigned.
	/// </summary>
	public class UnitAssetRepository : ScriptableObject
	{
		/// <summary>
		/// Provides singleton access to the unit asset repository.
		/// </summary>
		/// <value>The instance.</value>
		public static UnitAssetRepository Instance
		{
			get
			{
				if(instance == null)
					instance = (UnitAssetRepository)Resources.Load (RESOURCE_FILE);

				return instance;
			}
		}

		private static UnitAssetRepository instance = null;

		/// <summary>
		/// Resource file used to save unit asset repository information.
		/// </summary>
		public const string RESOURCE_FILE = @"UNIT_ASSET_REPOSITORY";

		/// <summary>
		/// Stores a list of all contained asset groups.
		/// </summary>
		public List<UnitAssetGroup> assetGroups = new List<UnitAssetGroup>();

		/// <summary>
		/// Retrieves an asset group by name.
		/// </summary>
		/// <returns>The asset group by name.</returns>
		/// <param name="name">Name.</param>
		public UnitAssetGroup getAssetGroupByName(string name)
		{
			IEnumerable<UnitAssetGroup> groups = assetGroups.Where(l => l.Name == name);
			if(groups.Count() == 0)
				return null;

			return groups.First ();
		}
	}
}
