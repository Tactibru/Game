using UnityEditor; 
using System.Collections; 
/// <summary>
/// Grid editor.
/// This class, takes variables from another class that you want to edit. 
/// </summary>
[CustomEditor(typeof(GridEditorBehavior))]
public class GridEditor:Editor
{
	public override void OnInspectorGUI()
	{
		//This is where you create your target, by script. 
		GridEditorBehavior targetScript = (GridEditorBehavior)target; 
		//To create a GUI element for each variable. 
		targetScript.row = EditorGUILayout.IntSlider("Row",targetScript.row, 1, 10); 
		targetScript.column = EditorGUILayout.IntSlider("Column", targetScript.column, 1, 10); 
		targetScript.booleanVariable = EditorGUILayout.Toggle("Bool", targetScript.booleanVariable); 
	}
}