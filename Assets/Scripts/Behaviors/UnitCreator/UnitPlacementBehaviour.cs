using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Not Used yet. Will keep track of the grid for placing the units.
/// </summary>

public class UnitPlacementBehaviour : MonoBehaviour 
{
    /// <summary>
    /// Available poisitions.
    /// </summary>

    int rank = 2;
    int positionPerRank = 5;

    float offSetInX = 0.5f;
    float offSetInY = 0.5f;
   
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
        List<PositionBehaviour> positionsCovered = new List<PositionBehaviour>();

        for (int index = 0; index < thePositions.Length; index++)
        {
            Debug.Log(index.ToString());
            if (thePositions[index].currentOccupant == -1)
            {
                Vector3 origin = new Vector3(thePositions[index].transform.position.x, thePositions[index].transform.position.y, thePositions[index].transform.position.z + 0.5f);
                Ray ray = new Ray(origin, new Vector3(0.0f, 0.0f, -1.0f));

                List<RaycastHit> hits = new List<RaycastHit>();
                hits.AddRange(Physics.RaycastAll(ray));

                Debug.Log(index.ToString());
                foreach (RaycastHit hitInfo in hits.OrderBy(l => l.distance))
                {
                    if (hitInfo.transform.GetComponent<MemberBehaviour>() == theMemberToLookFor)
                    {
                        coveredByMember++;
                        positionsCovered.Add(thePositions[index]);
                        isSet = true;
                        Debug.Log("Found");
                    }
                }
            }
        }

        if (coveredByMember == theMemberToLookFor.size)
        {
            switch (theMemberToLookFor.theSizeOfMember)
            {
                case MemberBehaviour.SizeOfMember.OneByOne:
                    theMemberToLookFor.transform.position = positionsCovered[0].transform.position;
                    break;
                case MemberBehaviour.SizeOfMember.OneByTwo:
                    theMemberToLookFor.transform.position = new Vector3(positionsCovered[0].transform.position.x, positionsCovered[0].transform.position.y + offSetInY, positionsCovered[0].transform.position.z);
                    break;
                case MemberBehaviour.SizeOfMember.TwoByOne:
                    theMemberToLookFor.transform.position = new Vector3(positionsCovered[0].transform.position.x + offSetInX, positionsCovered[0].transform.position.y, positionsCovered[0].transform.position.z);
                    break;
                case MemberBehaviour.SizeOfMember.TwoByTwo:
                    theMemberToLookFor.transform.position = new Vector3(positionsCovered[0].transform.position.x + offSetInX, positionsCovered[0].transform.position.y + offSetInY, positionsCovered[0].transform.position.z);
                    break;
            }
            

            for (int index = 0; index < positionsCovered.Count; index++)
            {
                positionsCovered[index].currentOccupant = theMemberToLookFor.uniqueID;
            }
        }
        return isSet;
    }

    public void UpdateTheTray()
    {
        for (int index = 0; index < thePositions.Length; index++)
        {
            if (thePositions[index].currentOccupant != -1)
            {
                Vector3 origin = new Vector3(thePositions[index].transform.position.x, thePositions[index].transform.position.y, thePositions[index].transform.position.z + 0.5f);
                Ray ray = new Ray(origin, new Vector3(0.0f, 0.0f, -1.0f));

                List<RaycastHit> hits = new List<RaycastHit>();
                hits.AddRange(Physics.RaycastAll(ray));
                bool isGood = false;

                foreach (RaycastHit hitInfo in hits.OrderBy(l => l.distance))
                {
                    if (hitInfo.transform.GetComponent<MemberBehaviour>().uniqueID == thePositions[index].currentOccupant)
                    {
                        isGood = true;
                    }
                    else if (hitInfo.transform.GetComponent<MemberBehaviour>().uniqueID != thePositions[index].currentOccupant)
                    {
                        Debug.Log("This should not happen");
                    }
                }

                if (!isGood)
                {
                    thePositions[index].currentOccupant = -1;
                }
            }
        }
    }

    public void ShowUnitHolder()
    {
        for (int index = 0; index < thePositions.Length; index++)
        {
            thePositions[index].transform.renderer.enabled = true;
        }
    }
}
