using UnityEngine;
using System.Collections;

public class UnitCreatorControllerBehaviour : MonoBehaviour 
{
    public int[] squadPosition = new int[10];
    public int squadUniqueID = 0;
    public bool isUnitCreatorActive = false;
	/// <summary>
	/// This is to load the current unit selected
    /// 
    /// Alex Reiss
	/// </summary>
   
	void Start () 
    {
      
        for (int index = 0; index < squadPosition.Length; index++)
        {
            squadPosition[index] = 0;
        }
        
	}
	
	
}
