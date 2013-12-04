using UnityEngine;
using System.Collections;

/// <summary>
/// This is for each possible position in the unit.
/// 
/// Alex Reiss
/// </summary>

public class PositionBehaviour : MonoBehaviour 
{
    /// <summary>
    /// 0 is unoccupied, if anything else, the n it is the unique ID of the occupant.
    /// </summary>
    public int currentOccupant = -1;
    public int rank = 0;
	
    /// <summary>
    /// This sets the renderer off, since, at the start of the scene a unit, will not be currently in the editing process.
    /// 
    /// Alex Reiss
    /// </summary>
   
	void Start () 
    {
        renderer.enabled = false;
	}

    public void SetMemberPosition(MemberBehaviour memberToBeAdded)
    {
 
    }
}
