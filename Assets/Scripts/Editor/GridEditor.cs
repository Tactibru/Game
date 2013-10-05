using Editor.Util;
using UnityEditor; 
using UnityEngine; 
using System.Collections; 

/// <summary>
/// Grid editor.
/// This class, takes variables from another class that you want to edit. 
/// </summary>
/// n
namespace Editor.Grid
{
[CustomEditor(typeof(GridEditorBehavior))]
public class GridEditor:EditorBase<GridEditorBehavior>
{
	public override void OnInspectorGUI()
	{
		//This is where you create your target, by script. 
		GridEditorBehavior targetScript = (GridEditorBehavior)target; 
		//To create a GUI element for each variable. 
		targetScript.row = EditorGUILayout.IntSlider("Row",targetScript.row, 1, 10); 
		targetScript.column = EditorGUILayout.IntSlider("Column", targetScript.column, 1, 10); 
		targetScript.booleanVariable = EditorGUILayout.Toggle("Bool", targetScript.booleanVariable); 
		targetScript.prefab = EditorGUILayout.ObjectField("Prefab", targetScript.prefab, typeof(GameObject), true) as GameObject; 
		
		if(GUILayout.Button("Call Function"))
			targetScript.FunctionCall(); 
	}
}
}