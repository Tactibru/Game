using UnityEngine;
using System.Collections;

public class GridEditorBehavior : MonoBehaviour 
{
	//MOCK CLASS 
	//made to run editor. 
	//need to work with alex and use his back end code. 
	
	public int row; 
	public int column; 
	public bool booleanVariable; 
	

	// Use this for initialization
	
	// Update is called once per frame
	void Update () 
	{
		row = Mathf.Clamp(row, 1, 10); 
		column = Mathf.Clamp(column, 1, 10); 
	
	}
}
