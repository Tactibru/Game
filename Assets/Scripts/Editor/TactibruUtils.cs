using UnityEngine;
using System.Collections;
using UEd = UnityEditor.Editor;
using UnityEditor;

namespace Editor
{
	public static class TactibruUtils
	{
		/// <summary>
		/// Swaps the material on the selected object for the Unit material.
		/// </summary>
		[UnityEditor.MenuItem("Tactibru/Set Unit Material Properties")]
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
	}
}
