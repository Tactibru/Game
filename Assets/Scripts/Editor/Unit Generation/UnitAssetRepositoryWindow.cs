using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Editor.Util;
using NodeSkeletonSystem;

namespace Editor.UnitGeneration
{
	/// <summary>
	/// Provides utility methods for interacting with the unit asset repository.
	/// </summary>
	public static class UnitAssetRepositoryWindow
	{
		/// <summary>
		/// Resource file used to save unit asset repository information.
		/// </summary>
		public const string RESOURCE_FILE = @"UNIT_ASSET_REPOSITORY";
	}
}