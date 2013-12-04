using UnityEngine;
using System.Collections.Generic;
using UEd = UnityEditor.Editor;
using UnityEditor;

namespace Tactibru.Editor
{
	public static class TactibruUtils
	{
		/// <summary>
		/// Swaps the material on the selected object for the Unit material.
		/// </summary>
		[MenuItem("Tactibru/Set Unit Material Properties")]
		public static void SetUnitMaterialProperties()
		{
			Material unitMat = (Material)GameObject.Instantiate(Resources.Load ("Materials/UnitMaterial"));
			if(unitMat == null)
			{
				Debug.LogError("UnitMaterial is null.");
				return;
			}

			if(Selection.activeGameObject == null)
				return;

			Selection.activeGameObject.renderer.sharedMaterial.SetColor ("_TargetHighlightColor", new Color((224.0f / 255.0f), (41.0f / 255.0f), (230.0f / 255.0f)));
			Selection.activeGameObject.renderer.sharedMaterial.SetColor ("_TargetBaseColor", new Color((200.0f / 255.0f), (34.0f / 255.0f), (206.0f / 255.0f)));
			Selection.activeGameObject.renderer.sharedMaterial.SetColor ("_TargetShadeColor", new Color((169.0f / 255.0f), (29.0f / 255.0f), (175.0f / 255.0f)));
		}

		/// <summary>
		/// Swaps the selected camera for the appropriate camera in the asset database.
		/// </summary>
		[MenuItem("Tactibru/Fix Camera")]
		public static void FixCamera()
		{
			GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

			// Destroy the camera in preparation for adding the new one.
			GameObject.DestroyImmediate(mainCamera);

			// Retrieve the MainCamera prefab from the asset database.
			GameObject newCamera = (GameObject)GameObject.Instantiate(AssetDatabase.LoadAssetAtPath(
				"Assets/Prefabs/Main Camera.prefab", typeof(GameObject)));

			newCamera.transform.name = "Main Camera";

			// Update the main camera property of the combat camera.
			CombatSystemBehavior combatSystem = (CombatSystemBehavior)GameObject.Find("Combat Camera").GetComponent<CombatSystemBehavior>();
			combatSystem.mainCamera = newCamera.camera;

			EditorUtility.SetDirty (combatSystem);

			// Disable all text renderers on the camera.
			MeshRenderer[] renderers = newCamera.GetComponentsInChildren<MeshRenderer>();
			foreach(MeshRenderer renderer in renderers)
			{
				if(renderer.gameObject.name == "MiniMapBG")
					continue;

				renderer.enabled = false;
			}

			// Update the grid for the minimap.
			GridBehavior grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridBehavior>();
			MiniMapGridBehaviour minimapGrid = newCamera.GetComponentInChildren<MiniMapGridBehaviour>();
			minimapGrid.theGrid = grid;
			minimapGrid.gameController = grid.GetComponent<GameControllerBehaviour>();

			EditorUtility.SetDirty (newCamera);

			Selection.activeGameObject = newCamera;

			Debug.Log ("Camera fixed.");
		}
	}
}