using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a group from which assets can be retrieved (e.g. "Heads")
/// </summary>
[AddComponentMenu("Tactibru/Assets/Unit Asset")]
public class UnitAssetBehavior : MonoBehaviour
{
	/// <summary>
	/// Represents the name of the asset group.
	/// </summary>
	public string Name = string.Empty;
}
