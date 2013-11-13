using UnityEngine;
using System.Collections;

/// <summary>
/// Implements camera scrolling when the mouse is at the edge of the screen.
/// </summary>
[AddComponentMenu("Tactibru/Core Systems/Camera")]
public class CameraBehavior : MonoBehaviour 
{
	/// <summary>
	/// Tag used to mark objects as Screen Bounds objects.
	/// </summary>
	private const string SCREEN_BOUNDS_TAG = "ScreenBounds";

	/// <summary>
	/// Movement rate of the camera, in m/s.
	/// </summary>
	public float MovementRate = 2.0f;

	/// <summary>
	/// Minimum camera position.
	/// </summary>
	Vector3 minimumPosition = Vector3.zero;

	/// <summary>
	/// Maximum camera position.
	/// </summary>
	Vector3 maximumPosition = Vector3.zero;

	/// <summary>
	/// Iterates over all scene objects with the appropriate Screen Bounds tag, and determines the minimum and maximum coordinates.
	/// </summary>
	public void Start()
	{
		GameObject[] sceneBounds = GameObject.FindGameObjectsWithTag(SCREEN_BOUNDS_TAG);

		foreach (GameObject obj in sceneBounds)
		{
			minimumPosition.Set(
				(minimumPosition.x > obj.transform.position.x ? obj.transform.position.x : minimumPosition.x),
				(minimumPosition.y > obj.transform.position.y ? obj.transform.position.y : minimumPosition.y),
				(minimumPosition.z > obj.transform.position.z ? obj.transform.position.z : minimumPosition.z)
				);

			maximumPosition.Set(
				(maximumPosition.x < obj.transform.position.x ? obj.transform.position.x : maximumPosition.x),
				(maximumPosition.y < obj.transform.position.y ? obj.transform.position.y : maximumPosition.y),
				(maximumPosition.z < obj.transform.position.z ? obj.transform.position.z : maximumPosition.z)
				);
		}
	}

	/// <summary>
	/// Determines whether or not the camera needs to move, based on the location of the mouse
	/// relative to the edges of the screen.
	/// </summary>
	public void Update()
	{
		if (Camera.current != null && Camera.current != this.camera)
			return;

		float movementAmount = MovementRate * Time.deltaTime;
		
		// Horizontal - Right Edge
		if (Input.mousePosition.x > (Screen.width - (Screen.width / 10)))
			transform.Translate(Vector3.right * movementAmount, Space.World);

		// Horizontal - Left Edge
		if (Input.mousePosition.x < (Screen.width / 10))
			transform.Translate(Vector3.right * -movementAmount, Space.World);

		// Vertical - Top Edge
		if (Input.mousePosition.y > (Screen.height - (Screen.height / 10)))
			transform.Translate(Vector3.forward * movementAmount, Space.World);

		// Vertical - Bottom Edge
		if (Input.mousePosition.y < (Screen.height / 10))
			transform.Translate(Vector3.forward * -movementAmount, Space.World);

		// Camera Constraint
		ConstrainCamera();
	}

	/// <summary>
	/// Constraints the camera to within the X and Z coordinates in minimumPosition and maximumPosition.
	/// </summary>
	public void ConstrainCamera()
	{
		// Check the camera position
		if (transform.position.x < minimumPosition.x)
			transform.Translate(Vector3.right * (minimumPosition.x - transform.position.x), Space.World);

		if (transform.position.z < minimumPosition.z)
			transform.Translate(Vector3.forward * (minimumPosition.z - transform.position.z), Space.World);

		if (transform.position.x > maximumPosition.x)
			transform.Translate(Vector3.left * (transform.position.x - maximumPosition.x), Space.World);

		if (transform.position.z > maximumPosition.z)
			transform.Translate(Vector3.back * (transform.position.z - maximumPosition.z), Space.World);
	}
}
