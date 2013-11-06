using UnityEngine;
using System.Collections;

/// <summary>
/// Will have to think about this class again, there maybe an overall.
/// </summary>

public class SquadCreatorControllerBehaviour : MonoBehaviour 
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
