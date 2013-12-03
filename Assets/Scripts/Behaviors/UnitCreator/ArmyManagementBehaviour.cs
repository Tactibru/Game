using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
                    }
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
        unitHolder.ShowUnitHolder();
    }
}
