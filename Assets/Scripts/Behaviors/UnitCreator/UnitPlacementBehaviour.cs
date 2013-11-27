using UnityEngine;
using System.Collections;

/// <summary>
/// Not Used yet. Will keep track of the grid for placing the units.
/// </summary>

public class UnitPlacementBehaviour : MonoBehaviour 
{
    /// <summary>
    /// Available poisitions.
    /// </summary>
   
    public PositionBehaviour[] thePositions;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public bool LookForMember(MemberBehaviour theMemberToLookFor)
    {
        bool isSet = false;
        int coveredByMember = 0;

        //for
        

        return isSet;
    }

    public void ShowUnitHolder()
    {
        for (int index = 0; index < thePositions.Length; index++)
        {
            thePositions[index].transform.renderer.enabled = true;
        }
    }
}
