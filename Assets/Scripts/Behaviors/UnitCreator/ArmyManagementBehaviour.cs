using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ArmyManagementBehaviour : MonoBehaviour 
{
    /// <summary>
    /// Number of currently made squads
    /// </summary>
 
    public int numberOfUnits;

    /// <summary>
    /// The squads.
    /// </summary>

    public List<SquadBehaviour> squads = new List<SquadBehaviour>();

    /// <summary>
    /// the max number of positions
    /// </summary>

    int numberOfPositions = 10;

    public bool inUnitCreator = false;

    public SquadBehaviour currentSquad;

    public MemberTrayBehaviour MemberTray;

    public StatsPanelBehaviour statsPanel;

    public UnitPlacementBehaviour unitHolder;


    /// <summary>
    /// Will load and place to seen and edited. NOT DONE.
    /// 
    /// Alex Reiss
    /// </summary>

	void Start () 
    {
        loadArmy();

        if (numberOfUnits > 0)
        {
 
        }

	}

    /// <summary>
    /// This will save army only squads that are currently built, and only positionly data. NOT TESTED.
    /// 
    /// Alex Reiss
    /// </summary>

    void SaveArmy()
    {
        PlayerPrefs.SetInt("ArmySize", numberOfUnits);

        string saveString = "";

        for (int index = 0 ; index < numberOfUnits - 1; index++)
        {
            saveString += squads[index].UnitString() + ",";
        }

        saveString += squads[squads.Count - 1].UnitString();

        PlayerPrefs.SetString("Army", saveString);
    }

    /// <summary>
    /// This will load the army, only positionly data will loaded. NOT TESTED.
    /// 
    /// Alex Reiss
    /// </summary>

    void loadArmy()
    {
        numberOfUnits = PlayerPrefs.GetInt("ArmySize");
        char [] army = PlayerPrefs.GetString("Army").ToCharArray();
        
        if (numberOfUnits > 0)
        {
            int currentIndexInArmyString = 0;
            for (int currentUnit = 0; currentUnit < numberOfUnits; currentUnit++)
            {
                SquadBehaviour newUnit = new SquadBehaviour();
                squads.Add(newUnit);
                for (int currentPosition = 0; currentPosition < numberOfPositions; currentPosition++)
                {
                    string currentPositionString = "";
                    while (army[currentIndexInArmyString] != ',')
                    {
                        currentPositionString += army[currentIndexInArmyString].ToString();
                        currentIndexInArmyString++;
                    }
                    squads[currentUnit].memberPositions[currentPosition] = int.Parse(currentPositionString);
                    currentIndexInArmyString++;
                }
            }
        }
    }
	
	/// <summary>
	/// At the moment not used but will be. DO NOT REMOVE.
    /// Will be used for selecting the squad to edit or create.
    /// 
    /// Alex Reiss
	/// </summary>
    
	void Update () 
    {
        if (!inUnitCreator)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (!currentSquad)
                    {
                        if (hitInfo.transform.GetComponent<SquadBehaviour>())
                        {
                            currentSquad = hitInfo.transform.GetComponent<SquadBehaviour>();
                            StartEditor(currentSquad);
                            inUnitCreator = true;
                        }
                    }
                }
            }
        }
        else
        {
            if (Input.mousePosition.x > (Screen.width - (Screen.width / 5.0f)))
            {
                if (Input.mousePosition.y < (Screen.height / 10.0f) && MemberTray.LowerBound.transform.localPosition.y < MemberTray.lowerBound)
                {
                    MemberTray.TopBound.transform.Translate(Vector3.up * MemberTray.scrollingRate * Time.deltaTime, transform);
                    for (int index = 0; index < MemberTray.members.Count; index++)
                    {
                        MemberTray.members[index].transform.Translate(Vector3.up * MemberTray.scrollingRate * Time.deltaTime, transform);
                    }
                    MemberTray.LowerBound.transform.Translate(Vector3.up * MemberTray.scrollingRate * Time.deltaTime, transform);
                }

                if (Input.mousePosition.y > (Screen.height - (Screen.height / 10)) && MemberTray.TopBound.transform.localPosition.y > MemberTray.upperBound)
                {
                    MemberTray.TopBound.transform.Translate(Vector3.up * -MemberTray.scrollingRate * Time.deltaTime, transform);
                    for (int index = 0; index < MemberTray.members.Count; index++)
                    {
                        MemberTray.members[index].transform.Translate(Vector3.up * -MemberTray.scrollingRate * Time.deltaTime, transform);
                    }
                    MemberTray.LowerBound.transform.Translate(Vector3.up * -MemberTray.scrollingRate * Time.deltaTime, transform);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                List<RaycastHit> hits = new List<RaycastHit>();
                hits.AddRange(Physics.RaycastAll(ray));

                foreach (RaycastHit hitInfo in hits.OrderBy(l => l.distance))
                {
                    if (hitInfo.transform.GetComponent<MemberBehaviour>())
                    {
                        if (unitHolder.theSquadBeingEdited.members < 5 || hitInfo.transform.GetComponent<MemberBehaviour>().inUnit)
                        {
                            MemberTray.HeldMember = hitInfo.transform.GetComponent<MemberBehaviour>();

                            MemberTray.currentlyHoldingAMember = true;

                            if (!MemberTray.HeldMember.inUnit)
                            {
                                MemberTray.members.Remove(MemberTray.HeldMember);
                                MemberTray.HeldMember.transform.parent = null;
                                switch (MemberTray.HeldMember.theSizeOfMember)
                                {
                                    case MemberBehaviour.SizeOfMember.OneByOne:
                                        MemberTray.lastIndexOfOneByOne--;
                                        break;
                                    case MemberBehaviour.SizeOfMember.OneByTwo:
                                        MemberTray.lastIndexOfOneByTwo--;
                                        break;
                                    case MemberBehaviour.SizeOfMember.TwoByOne:
                                        MemberTray.lastIndexOfTwoByOne--;
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (MemberTray.currentlyHoldingAMember)
                {
                    float yPos = (6.0f * Input.mousePosition.y / (float)Screen.height) - 2.0f;
                    float xPos = (MemberTray.xRatio * Input.mousePosition.x / (float)Screen.width) - (MemberTray.xRatio / 2);
                    MemberTray.HeldMember.transform.position = new Vector3(xPos, yPos, 0.5f);
                    //Debug.Log(yPos.ToString());
                }
            }
            //After this point.
            if (Input.GetMouseButtonUp(0))
            {
                //Debug.Log("Mouse Let Go.");
                if (MemberTray.currentlyHoldingAMember)
                {
                    if (!unitHolder.LookForMember(MemberTray.HeldMember))
                    {
                        switch (MemberTray.HeldMember.theSizeOfMember)
                        {
                            case MemberBehaviour.SizeOfMember.OneByOne:
                                MemberTray.members.Insert(MemberTray.lastIndexOfOneByOne, MemberTray.HeldMember);
                                MemberTray.lastIndexOfOneByOne++;
                                break;
                            case MemberBehaviour.SizeOfMember.OneByTwo:
                                MemberTray.members.Insert(MemberTray.lastIndexOfOneByTwo, MemberTray.HeldMember);
                                MemberTray.lastIndexOfOneByTwo++;
                                break;
                            case MemberBehaviour.SizeOfMember.TwoByOne:
                                MemberTray.members.Insert(MemberTray.lastIndexOfTwoByOne, MemberTray.HeldMember);
                                MemberTray.lastIndexOfTwoByOne++;
                                break;
                            case MemberBehaviour.SizeOfMember.TwoByTwo:
                                MemberTray.members.Add(MemberTray.HeldMember);
                                break;
                        }
                        //Debug.Log("HI");
                        MemberTray.HeldMember.inUnit = false;
                        MemberTray.BuildMemberTray();
                        unitHolder.UpdateTheTray();
                    }

                    MemberTray.currentlyHoldingAMember = false;
                    MemberTray.HeldMember = null;
                }
            }
        }
	}

    void StartEditor(SquadBehaviour theSquad)
    {
        for (int index = 0; index < squads.Count; index++)
        {
            squads[index].transform.renderer.enabled = false;
        }

        MemberTray.ShowMemberTray();
        statsPanel.transform.renderer.enabled = true;
        unitHolder.ShowUnitHolder(theSquad);
    }
}
