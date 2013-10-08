using UnityEngine;
using System.Collections;

public class GridBehavior : MonoBehaviour 
{
    public int theMapLength = 10;
    public int theMapWidth = 20;
    public static bool inCombat = false;
    public static bool preCombat = false;

    public MovePointBehavior[] theMap;
	
	public GameObject targetNode; 
	public GameObject currentActor;
    public GameObject targetActor;

    public MovePointBehavior theMovePointPrehab;

    char[] abc = new char[30] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd'};

    void Start()
    {
        //theMap = new MovePointBehavior[theMapLength * theMapWidth];
		
        for (int length = 0; length < theMapLength; length++)
        {
            for (int width = 0; width < theMapWidth; width++)
            {
                if (length < theMapLength - 1)
                {
                    if (theMap[width + (length * theMapWidth)] && theMap[width + ((length + 1) * theMapWidth)])
                    {
                        theMap[width + (length * theMapWidth)].neighborList[0] = theMap[width + ((length + 1) * theMapWidth)];
                        theMap[width + ((length + 1) * theMapWidth)].neighborList[2] = theMap[width + (length * theMapWidth)];
                    }
                }
                if (width < theMapWidth - 1)
                {
                    if (theMap[width + (length * theMapWidth)] && theMap[width + 1 + (length * theMapWidth)])
                    {
                        theMap[width + (length * theMapWidth)].neighborList[1] = theMap[width + 1 + (length * theMapWidth)];
                        theMap[width + 1 + (length * theMapWidth)].neighborList[3] = theMap[width + (length * theMapWidth)];
                    }
                } 
            }
        }
    }
	
	void RunDijkstras()
	{
		if(currentActor.GetComponent<ActorBehavior>().currentlyMoving)
		{
			return; 
		}
		else
		{
            if(targetNode)
			    currentActor.GetComponent<ActorBehavior>().pathList = MovePointBehavior.RunDijsktras(currentActor.GetComponent<ActorBehavior>().currentMovePoint.gameObject, targetNode); 
            else
                currentActor.GetComponent<ActorBehavior>().pathList = MovePointBehavior.RunDijsktras(currentActor.GetComponent<ActorBehavior>().currentMovePoint.gameObject, targetActor.GetComponent<ActorBehavior>().currentMovePoint.gameObject); 
		}
	}
	
    // Update is called once per frame
    void Update()
    {
		if(!inCombat)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.transform.GetComponent<MovePointBehavior>())
                    {
                        targetNode = hitInfo.transform.gameObject;

                    }
                    else if (hitInfo.transform.GetComponent<ActorBehavior>())
                    {
                        if (currentActor && hitInfo.transform.GetComponent<ActorBehavior>().side != currentActor.GetComponent<ActorBehavior>().side)
                        {
                            targetActor = hitInfo.transform.gameObject;
                        }
                        else
                        {
                            //Check if you click on a squad. 
                            currentActor = hitInfo.transform.gameObject;
                        }
                    }
                }
            }
		}

		/*if(currentActor)
		{
			MovePoint.DepthFirstSearch(currentActor.GetComponent<Actor>()); 
			if(targetNode)
			{
				RunDijkstras();
			
			}
		}
		currentActor = null; 
		targetNode = null; */

		if(currentActor && (targetNode || targetActor))
		{
            if (!preCombat)
            {
                if (targetActor)
                    preCombat = true;
                RunDijkstras();
                if (targetNode)
                {
                    currentActor = null;
                    targetNode = null;
                    targetActor = null;
                }
            }
		}
	}

    public void startCombat()
    {
		// Locate the combat camera.
		CombatSystemBehavior combatSystem = GameObject.Find("Combat Camera").GetComponent<CombatSystemBehavior>();
		if (combatSystem == null)
		{
			Debug.LogError("Error: Combat camera could not be found in the scene!\nRemember to add the Combat Camera prefix (with the name 'Combat Camera') into the scene.");
			return;
		}

		// Get the combat squad for both the offense and defense.
		CombatSquadBehavior offensiveSquadBehavior = currentActor.GetComponent<CombatSquadBehavior>();
		if (!offensiveSquadBehavior)
		{
			Debug.LogError("Offensive combat squad does not have a CombatSquadBehavior attached!");
			return;
		}

		CombatSquadBehavior defensiveSquadBehavior = targetActor.GetComponent<CombatSquadBehavior>();
		if (!defensiveSquadBehavior)
		{
			Debug.LogError("Defensive combat squad does not have a CombatSquadBehavior attached!");
			return;
		}

		//inCombat = true;

		combatSystem.BeginCombat(offensiveSquadBehavior, defensiveSquadBehavior);

        //Current actor is attacker and target actor is defender.
        targetActor = null;
        currentActor = null;
        preCombat = false;
    }

    public void CreateGrid()
    {
    	for(int _i = (gameObject.transform.childCount - 1); _i >= 0; _i--)
		DestroyImmediate(transform.GetChild (_i).gameObject);
    	
        theMap = new MovePointBehavior[theMapLength * theMapWidth];

        float xPositionOffset = -(theMapLength / 2);
        float yPositionOffset = -(theMapWidth / 2);
        float currentXPosition = 0.0f;
        float currentYPosition = 0.0f;

        for (int width = 0; width < theMapWidth; width++)
        {
            currentXPosition = xPositionOffset;
            currentYPosition = yPositionOffset + width;
            for (int length = 0; length < theMapLength; length++)
            {
                MovePointBehavior newMovePoint = (MovePointBehavior)Instantiate(theMovePointPrehab, new Vector3(currentXPosition, 1.0f, currentYPosition), Quaternion.identity);
                newMovePoint.transform.parent = transform;
                newMovePoint.name = abc[length].ToString() + width.ToString();
                theMap[length + (width * theMapLength)] = newMovePoint;
                currentXPosition = xPositionOffset + length +1;
            }
        }
    }
}
