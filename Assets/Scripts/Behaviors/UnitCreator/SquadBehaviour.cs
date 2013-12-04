using UnityEngine;
using System.Collections;

/// <summary>
/// The Squad, holds the data will show it.
/// </summary>

public class SquadBehaviour : MonoBehaviour 
{
    /// <summary>
    /// Possible positions
    /// </summary>

    public int[] memberPositions = new int[10];
    public int members = 0;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public string UnitString()
    {
        string returnString = "";

        for (int index = 0; index < memberPositions.Length - 1; index++)
        {
            returnString += memberPositions[index].ToString() + ",";
        }

        returnString += memberPositions[memberPositions.Length - 1].ToString();
        return returnString;
    }
}
