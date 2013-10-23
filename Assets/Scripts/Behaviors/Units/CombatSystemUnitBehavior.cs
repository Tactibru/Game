using UnityEngine;
using System.Collections;

/// <summary>
/// Implements custom rendering for a unit's health within the combat system.
/// </summary>
public class CombatSystemUnitBehavior : MonoBehaviour {
	/// <summary>
	/// Stores the mesh that will be used to render the unit's health.
	/// </summary>
	private TextMesh textMesh;

	/// <summary>
	/// Unit associated with this object.
	/// </summary>
	public Units.CombatUnit unit;

	/// <summary>
	/// Tacks the font used to render the text.
	/// </summary>
	private Font font;

	/// <summary>
	/// Adds a <see cref="UnityEngine.TextMesh"/> component to the game object to handle rendering.
	/// </summary>
	public void Start()
	{
		textMesh = gameObject.AddComponent<TextMesh>();
		textMesh.name = "Name";

		textMesh.font = font;
		textMesh.alignment = TextAlignment.Center;
	}

	/// <summary>
	/// Ensures that the appropriate text is set for the TextMesh component.
	/// </summary>
	public void Update()
	{
		if (unit == null)
			return;

		textMesh.text = string.Format("{0}/{1}", unit.CurrentHealth, unit.Health);
	}

	/// <summary>
	/// Sets the font used on the text mesh.
	/// </summary>
	/// <param name="font">Font to use for rendering.</param>
	public void SetFont(Font font)
	{
		this.font = font;
	}
}
