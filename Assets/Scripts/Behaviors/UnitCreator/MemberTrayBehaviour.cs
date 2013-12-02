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

    int lastIndexOfOneByOne = 0;
    int lastIndexOfOneByTwo = 0;
    int lastIndexOfTwoByOne = 0;

    public float upperBound = 2.3f;
    public float lowerBound = -1.9f;

    public UnitPlacementBehaviour unitHolder;
    float xRatio;
    bool currentlyHoldingAMember = false;
    bool isActive = false;

    Vector3 previousPosition = new Vector3(0.0f, 0.0f, 0.0f);

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
	
	// Update is called once per frame
	void Update () 
    {
        //Debug.Log(Input.mousePosition.x.ToString());
        if (isActive)
        {
            if (Input.mousePosition.x > (Screen.width - (Screen.width / 5.0f)))
            {
                if (Input.mousePosition.y < (Screen.height / 10.0f) && LowerBound.transform.localPosition.y < lowerBound)
                {
                    TopBound.transform.Translate(Vector3.up * scrollingRate * Time.deltaTime, transform);
                    for (int index = 0; index < members.Count; index++)
                    {
                        members[index].transform.Translate(Vector3.up * scrollingRate * Time.deltaTime, transform);
                    }
                    LowerBound.transform.Translate(Vector3.up * scrollingRate * Time.deltaTime, transform);
                }



                if (Input.mousePosition.y > (Screen.height - (Screen.height / 10)) && TopBound.transform.localPosition.y > upperBound)
                {
                    TopBound.transform.Translate(Vector3.up * -scrollingRate * Time.deltaTime, transform);
                    for (int index = 0; index < members.Count; index++)
                    {
                        members[index].transform.Translate(Vector3.up * -scrollingRate * Time.deltaTime, transform);
                    }
                    LowerBound.transform.Translate(Vector3.up * -scrollingRate * Time.deltaTime, transform);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
				    
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.transform.GetComponent<MemberBehaviour>())
                    {
                        HeldMember = hitInfo.transform.GetComponent<MemberBehaviour>();
                        
                        currentlyHoldingAMember = true;

                        if (!HeldMember.inUnit)
                        {
                            members.Remove(HeldMember);
                            HeldMember.transform.parent = null;
                            switch (HeldMember.theSizeOfMember)
                            {
                                case MemberBehaviour.SizeOfMember.OneByOne:
                                    lastIndexOfOneByOne--;
                                    break;
                                case MemberBehaviour.SizeOfMember.OneByTwo:
                                    lastIndexOfOneByTwo--;
                                    break;
                                case MemberBehaviour.SizeOfMember.TwoByOne:
                                    lastIndexOfTwoByOne--;
                                    break;
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                
                if (currentlyHoldingAMember)
                {
                    float yPos = (6.0f * Input.mousePosition.y / (float)Screen.height) - 2.0f;
                    float xPos = (xRatio * Input.mousePosition.x / (float)Screen.width) - (xRatio / 2);
                    HeldMember.transform.position = new Vector3(xPos, yPos, 0.5f);
                    //Debug.Log(yPos.ToString());
                    
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Mouse Let Go.");
                if (currentlyHoldingAMember)
                {
                    if (!unitHolder.LookForMember(HeldMember))
                    {
                        switch (HeldMember.theSizeOfMember)
                        {
                            case MemberBehaviour.SizeOfMember.OneByOne:
                                members.Insert(lastIndexOfOneByOne, HeldMember);
                                lastIndexOfOneByOne++;
                                break;
                            case MemberBehaviour.SizeOfMember.OneByTwo:
                                members.Insert(lastIndexOfOneByTwo, HeldMember);
                                lastIndexOfOneByTwo++;
                                break;
                            case MemberBehaviour.SizeOfMember.TwoByOne:
                                members.Insert(lastIndexOfTwoByOne, HeldMember);
                                lastIndexOfTwoByOne++;
                                break;
                            case MemberBehaviour.SizeOfMember.TwoByTwo:
                                members.Add(HeldMember);
                                break;
                        }
                        Debug.Log("HI");

                        BuildMemberTray();
                    }
                }
            }
        }
	}

    public void ShowMemberTray()
    {
        //transform.renderer.enabled = true;
        theBackGround.transform.renderer.enabled = true;
        isActive = true;

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
                currentMember = members[currentIndex];
            }

            Debug.Log("First while loop");

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

                currentMember = members[currentIndex];
            }

            Debug.Log("Second While Loop");

            lastIndexOfOneByTwo = members.Count - 1;

            //currentRowOfTwo--;

            while (currentMember.theSizeOfMember == MemberBehaviour.SizeOfMember.TwoByOne)
            {
                currentMember.transform.parent = transform;

                currentMember.transform.localPosition = new Vector3(columnOne + differenceBetweenOneAndTwoX, currentTopPosition.y - 0.7f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY + 0.5f, 0.0f);
                currentRowOfOne++;

                currentMember = members[currentIndex];
            }

            Debug.Log("Third while loop");

            lastIndexOfTwoByOne = members.Count - 1;

            while (currentMember.theSizeOfMember == MemberBehaviour.SizeOfMember.TwoByOne)
            {
                currentMember.transform.parent = transform;
                currentMember.transform.localPosition = new Vector3(columnOne + differenceBetweenOneAndTwoX, currentTopPosition.y - 0.7f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY, 0.0f);
                currentRowOfTwo++;

                currentMember = members[currentIndex];
            }

            Debug.Log("Fourth while loop");

            LowerBound.transform.localPosition = new Vector3(columnOne + differenceBetweenOneAndTwoX, currentTopPosition.y - 0.7f - (currentRowOfOne * distanceBetweenRowsForOne) - (currentRowOfTwo * distanceBetweenRowsForTwo) + differenceBetweenOneAndTwoY + 0.5f, 0.0f);

        }
    }

    
}
