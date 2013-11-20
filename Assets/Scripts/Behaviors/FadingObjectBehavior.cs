using UnityEngine;
using System.Collections;

/// <summary>
/// Represents an object that should fade when a squad is selected.
/// </summary>
[AddComponentMenu("Tactibru/Environment/Fading Object")]
public class FadingObjectBehavior : MonoBehaviour {
	/// <summary>
	/// Stores a reference to the level's grid object.
	/// </summary>
	private GridBehavior grid;

	/// <summary>
	/// Alpha value for the object whenever it is shaded.
	/// </summary>
	public float fadedAlpha = 0.5f;

	/// <summary>
	/// Populates the grid variable with the level's grid.
	/// </summary>
	public void Start()
	{
		GameObject gridObject = GameObject.FindGameObjectWithTag("Grid");
		if(gridObject != null)
			grid = gridObject.GetComponent<GridBehavior>();
	}

	/// <summary>
	/// Swaps materials if GridBehavior.currentActor is not null.
	/// </summary>
	public void Update()
	{
		if(grid == null)
			return;

		foreach(Renderer renderer in GetComponentsInChildren<MeshRenderer>())
		{
			Shader shader = Shader.Find("Transparent/Diffuse");
			renderer.material.shader = shader;

			Color color = renderer.material.color;
			color.a = (grid.currentActor != null ? fadedAlpha : 1.0f);

			renderer.material.color = color;
		}
	}
}
