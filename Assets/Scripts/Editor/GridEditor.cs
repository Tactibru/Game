using Tactibru.Editor.Util;
using UnityEditor; 
using UnityEngine; 
using System.Collections; 

/// <summary>
/// Grid editor.
/// This class, takes variables from another class that you want to edit. 
/// </summary>
/// n
namespace Tactibru.Editor.Grid
{
[CustomEditor(typeof(GridBehavior))]
public class GridEditor:EditorBase<GridBehavior>
{
	public override void OnInspectorGUI()
	{
		//This is where you create your target, by script. 
		GridBehavior targetScript = (GridBehavior)target; 
		//To create a GUI element for each variable. 
        GUI.changed = false;
		targetScript.theMapLength = EditorGUILayout.IntSlider("Map Length",targetScript.theMapLength, 1, 30); 
		targetScript.theMapWidth = EditorGUILayout.IntSlider("Map Width", targetScript.theMapWidth, 1, 30); 
		targetScript.isFenced = EditorGUILayout.Toggle("Is Fenced", targetScript.isFenced); 
		targetScript.theMovePointPrehab = EditorGUILayout.ObjectField("Movepoint", targetScript.theMovePointPrehab, typeof(MovePointBehavior), true) as MovePointBehavior;
        targetScript.theAltMovePointPrehab = EditorGUILayout.ObjectField("AltMovePoint", targetScript.theAltMovePointPrehab, typeof(MovePointBehavior), true) as MovePointBehavior;
        targetScript.theFencePointPrehab = EditorGUILayout.ObjectField("Fence", targetScript.theFencePointPrehab, typeof(FenceBehavour), true) as FenceBehavour;

        if (GUI.changed)
            EditorUtility.SetDirty(targetScript);
		
		if(GUILayout.Button("Create Grid"))
			targetScript.CreateGrid(); 
	}
}
}
