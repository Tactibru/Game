using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Will the members in the tray and will also deal scrolls the possible units
/// </summary>

public class MemberTrayBehaviour : MonoBehaviour 
{
    public List<MemberBehaviour> members = new List<MemberBehaviour>();

    public MemberBehaviour memberOneByOne;
    public MemberBehaviour memberOneByTwo;
    public MemberBehaviour memberTwoByOne;
    public MemberBehaviour memberTwoByTwo;
    public GameObject TopBound;
    public GameObject LowerBound;

    public MemberBehaviour HeldMember;

    public TrayBackGroundBehaviour theBackGround;

    public float scrollingRate = 5.0f;

    public float columnOne = -1.1f;
    public float columnTwo = 0.4f;
    public float distanceBetweenRowsForOne = 1.4f;
    public float differenceBetweenOneAndTwoY = 0.4f;
    public float distanceBetweenRowsForTwo = 1.8f;
    public float differenceBetweenOneAndTwoX = 0.5f;

    public int lastIndexOfOneByOne = 0;
    public int lastIndexOfOneByTwo = 0;
    public int lastIndexOfTwoByOne = 0;

    public float upperBound = 2.3f;
    public float lowerBound = -1.9f;

    public UnitPlacementBehaviour unitHolder;
    public float xRatio;
    public bool currentlyHoldingAMember = false;

    /// <summary>
    /// Just makes the tray invisable, at the moment
    /// </summary>

    int magicNumberForTesting = 10;

	void Start () 
    {
        xRatio = 6.0f * ((float)Screen.width / (float)Screen.height);
        int currentRowOfOne = 0;
        int currentRowOfTwo = 0;
        int exampleID = 0;

        for(int index = 0; index < magicNumberForTesting; index++)
        {
            MemberBehaviour newMember = null;
            newMember = (MemberBehaviour)Instantiate(memberOneByOne, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            newMember.transform.parent = transform;
            newMember.uniqueID = exampleID;
            exampleID++;

            int columnChoiceByIndex = index % 2;
            if (columnChoiceByIndex == 0)
            {
                newMember.transform.localPosition = new Vector3(columnOne, 2.3f - (currentRowOfOne * distanceBetweenRowsForOne), 0.0f);
            }
            else
            {
                newMember.transform.localPosition = new Vector3(columnTwo, 2.3f - (currentRowOfOne * distanceBetweenRowsForOne), 0.0f);
                currentRowOfOne++;
            }

            members.Add(newMember);
        }
        lastIndexOfOneByOne = members.Count - 1;
        currentRowOfOne++;
  
        for (int index = 0; index < magicNumberForTesting; index++)
        {
            MemberBehaviour newMember = null;
            newMember = (MemberBehaviour)Instantiate(memberOneByTwo, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            newMember.transform.parent = transform;
            newMember.uniqueID = exampleID;
            exampleID++;

            int columnChoiceByIndex = index % 2;
            if (columnChoiceByIndex == 0)
            {
                newMember.transform.localPosition = new Vector3(columnOne, 2.3f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo *distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY, 0.0f);
            }
            else
            {
                newMember.transform.localPosition = new Vector3(columnTwo, 2.3f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY, 0.0f);
                currentRowOfTwo++;
            }

            members.Add(newMember);
        }
        lastIndexOfOneByTwo = members.Count - 1;

        //currentRowOfTwo--;

        for (int index = 0; index < magicNumberForTesting; index++)
        {
            MemberBehaviour newMember = null;
            newMember = (MemberBehaviour)Instantiate(memberTwoByOne, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            newMember.transform.parent = transform;
            newMember.uniqueID = exampleID;
            exampleID++;

            newMember.transform.localPosition = new Vector3(columnOne + differenceBetweenOneAndTwoX, 2.3f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY + 0.5f, 0.0f);
            currentRowOfOne++;
          
            members.Add(newMember);
        }

        lastIndexOfTwoByOne = members.Count - 1;

        for (int index = 0; index < magicNumberForTesting; index++)
        {
            MemberBehaviour newMember = null;
            newMember = (MemberBehaviour)Instantiate(memberTwoByTwo, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            newMember.transform.parent = transform;
            newMember.uniqueID = exampleID;
            exampleID++;

            newMember.transform.localPosition = new Vector3(columnOne + differenceBetweenOneAndTwoX, 2.3f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY, 0.0f);
            currentRowOfTwo++;

            members.Add(newMember);

        }

        LowerBound.transform.localPosition = new Vector3(columnOne + differenceBetweenOneAndTwoX, 2.3f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY + 0.5f, 0.0f);

        //renderer.enabled = false;
	}

    public void ShowMemberTray()
    {
        //transform.renderer.enabled = true;
        theBackGround.transform.renderer.enabled = true;
        //isActive = true;

        for (int index = 0; index < members.Count; index++)
        {
            members[index].transform.renderer.enabled = true;
        }
    }

    public void BuildMemberTray()
    {
        if (members.Count > 0)
        {
            MemberBehaviour currentMember = members[0];
            Vector3 currentTopPosition = TopBound.transform.localPosition;
            int currentIndex = 0;
            int currentRowOfOne = 0;
            int currentRowOfTwo = 0;

            while (currentMember.theSizeOfMember == MemberBehaviour.SizeOfMember.OneByOne)
            {
                currentMember.transform.parent = transform;
                int columnChoiceByIndex = currentIndex % 2;

                if (columnChoiceByIndex == 0)
                {
                    currentMember.transform.localPosition = new Vector3(columnOne, currentTopPosition.y - 0.7f - (currentRowOfOne * distanceBetweenRowsForOne), 0.0f);
                }
                else
                {
                    currentMember.transform.localPosition = new Vector3(columnTwo, currentTopPosition.y - 0.7f - (currentRowOfOne * distanceBetweenRowsForOne), 0.0f);
                    currentRowOfOne++;
                }
                currentIndex++;
                currentMember = members[currentIndex];
            }

            //Debug.Log("First while loop");

            currentRowOfOne++;
            if (currentIndex % 2 != 0)
                currentRowOfOne++;

            lastIndexOfOneByOne = members.Count - 1;
            int usedForToggle = 0;

            while (currentMember.theSizeOfMember == MemberBehaviour.SizeOfMember.OneByTwo)
            {
                currentMember.transform.parent = transform;
                int columnChoiceByIndex = usedForToggle % 2;
                if (columnChoiceByIndex == 0)
                {
                    currentMember.transform.localPosition = new Vector3(columnOne, currentTopPosition.y - 0.7f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY, 0.0f);
                }
                else
                {
                    currentMember.transform.localPosition = new Vector3(columnTwo, currentTopPosition.y - 0.7f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY, 0.0f);
                    currentRowOfTwo++;
                }
                usedForToggle++;
                currentIndex++;
                currentMember = members[currentIndex];
            }

            //Debug.Log("Second While Loop");
            if (usedForToggle % 2 != 0)
                currentRowOfTwo++;

            lastIndexOfOneByTwo = members.Count - 1;

            //currentRowOfTwo--;

            while (currentMember.theSizeOfMember == MemberBehaviour.SizeOfMember.TwoByOne)
            {
                currentMember.transform.parent = transform;

                currentMember.transform.localPosition = new Vector3(columnOne + differenceBetweenOneAndTwoX, currentTopPosition.y - 0.7f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY + 0.5f, 0.0f);
                currentRowOfOne++;

                currentIndex++;
                currentMember = members[currentIndex];
            }

            //Debug.Log("Third while loop");

            lastIndexOfTwoByOne = members.Count - 1;

            while (currentMember)
            {
                currentMember.transform.parent = transform;
                currentMember.transform.localPosition = new Vector3(columnOne + differenceBetweenOneAndTwoX, currentTopPosition.y - 0.7f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY, 0.0f);
                currentRowOfTwo++;

                currentIndex++;
                if (currentIndex != members.Count)
                    currentMember = members[currentIndex];
                else
                    currentMember = null;
            }

            //Debug.Log("Fourth while loop");

            LowerBound.transform.localPosition = new Vector3(columnOne + differenceBetweenOneAndTwoX, currentTopPosition.y - 0.7f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY + 0.5f, 0.0f);

        }
    }
}
