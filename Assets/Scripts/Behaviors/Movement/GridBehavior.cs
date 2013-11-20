using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GridBehavior : MonoBehaviour 
{
    public static bool inCombat = false;
    public static bool preCombat = false;
    public GameControllerBehaviour gameController;
    public List<MovePointBehavior> ignoreList;

    //public MovePointBehavior[] theMap;
    //public FenceBehavour[] theVerticalFence;
    //public FenceBehavour[] theHorizontalFence;
	
	public MovePointBehavior targetNode; 
	public GameObject currentActor;
    public GameObject targetActor;

    [SerializeField]
    public MovePointBehavior theMovePointPrehab;
    public MovePointBehavior theAltMovePointPrehab;
    public FenceBehavour theFencePointPrehab;
    public bool isFenced;
    public int theMapLength;
    public int theMapWidth;
    public MovePointBehavior[] theMap = new MovePointBehavior[900];
    public FenceBehavour[] theVerticalFence;
    public FenceBehavour[] theHorizontalFence;

    //used for depth-first
    bool ableToMoveHere = true;

    char[] abc = new char[30] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd'};

    /// <summary>
    /// This to setup neighbor lists for each node in the grid.
    /// 
    /// Alex Reiss
    /// </summary>

    void Start()
    {
        
        gameController = GameObject.FindGameObjectWithTag("Grid").GetComponent<GameControllerBehaviour>();
        //Debug.Log(theMapLength.ToString());
        //Debug.Log(theMapWidth.ToString());
        //Debug.Log(theVerticalFence.Length.ToString());
        //Debug.Log(theHorizontalFence.Length.ToString());
        //Debug.Log(theMap.Length.ToString());

        for (int index = 0; index < gameController.enemyTeam.Count; index++)
            ignoreList.Add(gameController.enemyTeam[index].currentMovePoint);
        for (int index = 0; index < gameController.playerTeam.Count; index++)
            ignoreList.Add(gameController.playerTeam[index].currentMovePoint);
        for (int index = 0; index < gameController.nuetrals.Count; index++)
            ignoreList.Add(gameController.nuetrals[index].currentMovePoint);
		
        for (int length = 0; length < theMapLength; length++)
        {
            for (int width = 0; width < theMapWidth; width++)
            {
               
                if (length < theMapLength - 1)
                {
                    //Debug.Log((width + (length * theMapWidth)).ToString());
                    
                    if (isFenced)
                    {
                        if (theMap[width + (length * theMapWidth)] && theMap[width + ((length + 1) * theMapWidth)] && theVerticalFence[width + (length * theMapWidth)])
                        {
                            theMap[width + (length * theMapWidth)].neighborList[0] = theMap[width + ((length + 1) * theMapWidth)];
                            theMap[width + ((length + 1) * theMapWidth)].neighborList[2] = theMap[width + (length * theMapWidth)];
                        }
                    }
                    else
                    {
                        if (theMap[width + (length * theMapWidth)] && theMap[width + ((length + 1) * theMapWidth)])
                        {
                            theMap[width + (length * theMapWidth)].neighborList[0] = theMap[width + ((length + 1) * theMapWidth)];
                            theMap[width + ((length + 1) * theMapWidth)].neighborList[2] = theMap[width + (length * theMapWidth)];
                        }
                    }
                }

                if (width < theMapWidth - 1)
                {
                    if (isFenced)
                    {
                        //Debug.Log("Hi 1");
                        if (theMap[width + (length * theMapWidth)] && theMap[width + 1 + (length * theMapWidth)] && theHorizontalFence[width + (length * theMapWidth)])
                        {
                            //Debug.Log("Hi 2");
                            theMap[width + (length * theMapWidth)].neighborList[1] = theMap[width + 1 + (length * theMapWidth)];
                            theMap[width + 1 + (length * theMapWidth)].neighborList[3] = theMap[width + (length * theMapWidth)];
                        }
                    }
                    else
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
    }

	/// <summary>
	/// Disables the current actor, and marks its idle animation as inactive.
	/// </summary>
	public void disableCurrentActor()
	{
		if (currentActor == null)
			return;

		UnitIdleAnimationBehavior[] idles = currentActor.GetComponentsInChildren<UnitIdleAnimationBehavior>();
		foreach (UnitIdleAnimationBehavior idle in idles)
			idle.Active = false;
		currentActor = null;

		foreach (MovePointBehavior movePoint in theMap)
			if (movePoint && movePoint.renderer.enabled == true)
				movePoint.renderer.enabled = false;
	}

    /// <summary>
    /// Run Update.
    ///
    /// I do not create this function, Alex Reiss.
    /// </summary>
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
					if (!currentActor)
					{
						if (hitInfo.transform.GetComponent<ActorBehavior> ())
						{
							if (!hitInfo.transform.GetComponent<ActorBehavior> ().actorHasMovedThisTurn && hitInfo.transform.GetComponent<ActorBehavior> ().theSide == gameController.currentTurn)
							{
								currentActor = hitInfo.transform.gameObject;

								UnitIdleAnimationBehavior[] idles = currentActor.GetComponentsInChildren<UnitIdleAnimationBehavior>();
								foreach(UnitIdleAnimationBehavior idle in idles)
									idle.Active = true;
							}
						}
					}
					else if (hitInfo.transform.GetComponent<MovePointBehavior>() && hitInfo.transform.GetComponent<MovePointBehavior>().renderer.isVisible)
                    {
                        targetNode = hitInfo.transform.GetComponent<MovePointBehavior>();
                    }
                    else if (hitInfo.transform.GetComponent<ActorBehavior>())
                    {
						if (hitInfo.transform.GetComponent<ActorBehavior>().theSide != currentActor.GetComponent<ActorBehavior>().theSide)
							targetActor = hitInfo.transform.gameObject;
						else if (hitInfo.transform.GetComponent<ActorBehavior>() == currentActor.GetComponent<ActorBehavior>())
						{
							disableCurrentActor();
						}
                    }
                }
            }
		}
        
		//After choosing a unit, show movepoints they can go to
        if (currentActor && (!targetNode && !targetActor))
        {
			currentActor.GetComponent<ActorBehavior>().currentMovePoint.HighlightValidNodes(currentActor.GetComponent<ActorBehavior>(), this); 
        }

		if(currentActor && (targetNode || targetActor))
		{
            //if you can't see the point, you can't move to it. 
            //if(targetNode && !targetNode.renderer.enabled)
            //{
                /*if(targetNode && !targetNode.renderer.enabled)
                {
                    ableToMoveHere = false; 
                    currentActor = null; 
                    targetNode = null; 
                    targetActor = null; 
                }*/
                foreach(MovePointBehavior movePoint in theMap)
                {
                    //change visiblilty of nodes. 
                    if(movePoint && movePoint.renderer.enabled == true)
                        movePoint.renderer.enabled = false; 
                }

            //}


			ActorBehavior actor = currentActor.GetComponent<ActorBehavior>();
            if (!preCombat && ableToMoveHere == true && !actor.currentlyMoving)
            {
				MovePointBehavior startingPoint = actor.currentMovePoint;

				CombatSquadBehavior squadBehavior = currentActor.GetComponent<CombatSquadBehavior>();
				int distance = (squadBehavior == null ? 0 : squadBehavior.Squad.Speed);

				if(targetActor)
					ignoreList.Remove(targetActor.GetComponent<ActorBehavior>().currentMovePoint);
				List<MovePointBehavior> pathList = startingPoint.FindPath(targetNode ?? targetActor.GetComponent<ActorBehavior>().currentMovePoint, distance, this);
				if (pathList != null)
				{
					actor.pathList = pathList;
					actor.actorHasMovedThisTurn = true;
					actor.canMove = true;
					gameController.leftToMoveThis--;

					if (targetActor)
					{
						pathList.RemoveAt(pathList.Count - 1);
						preCombat = true;
						ignoreList.Remove(currentActor.GetComponent<ActorBehavior>().currentMovePoint);
					}
				}
				else
				{
					disableCurrentActor();
					targetNode = null;
					targetActor = null;
				}

                if (targetNode)
                {
                    ignoreList.Remove(currentActor.GetComponent<ActorBehavior>().currentMovePoint);
                    ignoreList.Add(targetNode.GetComponent<MovePointBehavior>());
					disableCurrentActor();
                    targetNode = null;
                    targetActor = null;
                }
            }
            ableToMoveHere = true; 
		}
	}

    /// <summary>
    /// I created this function to start combat, for he combat system. The end resets all pre combat stuff.
    /// 
    /// Alex Reiss
    /// </summary>

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

		combatSystem.BeginCombat(offensiveSquadBehavior, defensiveSquadBehavior, this);

        //Current actor is attacker and target actor is defender.
        ignoreList.Clear();
        for (int index = 0; index < gameController.enemyTeam.Count; index++)
            ignoreList.Add(gameController.enemyTeam[index].currentMovePoint);
        for (int index = 0; index < gameController.playerTeam.Count; index++)
            ignoreList.Add(gameController.playerTeam[index].currentMovePoint);

        if (!targetActor)
        {
            if (gameController.currentTurn == GameControllerBehaviour.UnitSide.player)
                gameController.enemyTeamTotal--;
            else
                gameController.playerTeamTotal--;
        }

        if (!currentActor)
        {
            if (gameController.currentTurn == GameControllerBehaviour.UnitSide.player)
                gameController.playerTeamTotal--;
            else
                gameController.enemyTeamTotal--;
        }

		disableCurrentActor();

        targetActor = null;
        preCombat = false;
    }

    /// <summary>
    /// Creates the grid. The fenced variable is used to determine fences are required.
    /// 
    /// Fences are just a name that makes it easier for the gmae designers, the fences are just a means to remove an edge from the graph visually.
    /// 
    /// Alex Reiss
    /// </summary>

    public void CreateGrid()
    {
    	for(int _i = (gameObject.transform.childCount - 1); _i >= 0; _i--)
		    DestroyImmediate(transform.GetChild (_i).gameObject);

    	
        theMap = new MovePointBehavior[theMapLength * theMapWidth];

        if (isFenced)
        {
            theVerticalFence = new FenceBehavour[(theMapLength) * theMapWidth];
            theHorizontalFence = new FenceBehavour[theMapLength * (theMapWidth)];
        }

        float xPositionOffset = -(theMapWidth / 2);
        float yPositionOffset = -(theMapLength / 2);
        float currentXPosition = 0.0f;
        float currentYPosition = 0.0f;

        //Debug.Log(theMapLength.ToString());
        //Debug.Log(theMapWidth.ToString());
        //Debug.Log(theVerticalFence.Length.ToString());
        //Debug.Log(theHorizontalFence.Length.ToString());
        //Debug.Log(theMap.Length.ToString());

        for (int x = 0; x < theMapLength; x++)
        {
            currentXPosition = xPositionOffset;
            currentYPosition = yPositionOffset + x;
            for (int z = 0; z < theMapWidth; z++)
            {
                MovePointBehavior newMovePoint = null;
				if ((z + x) % 2 == 0)
					newMovePoint = (MovePointBehavior)Instantiate(theMovePointPrehab, new Vector3(currentXPosition, 1.0f, currentYPosition), Quaternion.identity);
				else
					newMovePoint = (MovePointBehavior)Instantiate(theAltMovePointPrehab, new Vector3(currentXPosition, 1.0f, currentYPosition), Quaternion.identity);
                newMovePoint.transform.parent = transform;
                newMovePoint.name = abc[z].ToString() + x.ToString();
                theMap[z + (x * theMapWidth)] = newMovePoint;
                

                if (isFenced)
                {
                    if (x < theMapLength - 1)
                    {
                        FenceBehavour newVerticalFence = (FenceBehavour)Instantiate(theFencePointPrehab, new Vector3(currentXPosition, 1.0f, currentYPosition + 0.5f), Quaternion.identity);
                        newVerticalFence.transform.parent = transform;
                        newVerticalFence.name = abc[z].ToString() + x.ToString() + "fence" + abc[z].ToString() + (x + 1).ToString();
                        theVerticalFence[z + (x * theMapWidth)] = newVerticalFence;
                    }

                    if (z < theMapWidth - 1)
                    {
                        FenceBehavour newHorizontalFence = (FenceBehavour)Instantiate(theFencePointPrehab, new Vector3(currentXPosition + 0.5f, 1.0f, currentYPosition), Quaternion.identity);
                        newHorizontalFence.transform.parent = transform;
                        newHorizontalFence.name = abc[z].ToString() + x.ToString() + "fence" + abc[z + 1].ToString() + x.ToString();
                        theHorizontalFence[z + (x * theMapWidth)] = newHorizontalFence;
                    }
                }

                currentXPosition = xPositionOffset + z + 1;
            }
        }
    }
}
