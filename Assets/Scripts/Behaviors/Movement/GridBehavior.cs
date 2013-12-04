using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
[AddComponentMenu("Tactibru/Movement/Grid")]
[RequireComponent(typeof(GameControllerBehaviour))]
public class GridBehavior : MonoBehaviour 
{
    public static bool inCombat = false;
    public static bool preCombat = false;
    public GameControllerBehaviour gameController;
    public List<MovePointBehavior> ignoreList;

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

    char[] abc = new char[30] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd'};

    /// <summary>
    /// This to setup neighbor lists for each node in the grid.
    /// 
    /// Alex Reiss
    /// </summary>
    void Start()
    {
        int currentIndex = 0;
       
        gameController = GetComponent<GameControllerBehaviour>();

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

                if (theMap[width + (length * theMapWidth)])
                    theMap[width + (length * theMapWidth)].index = currentIndex;


                if (length < theMapLength - 1)
                {
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
                        if (theMap[width + (length * theMapWidth)] && theMap[width + 1 + (length * theMapWidth)] && theHorizontalFence[width + (length * theMapWidth)])
                        {
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

                currentIndex++;
            }
        }
    }

	public void HideMovePoints()
	{
		foreach(MovePointBehavior movePoint in theMap)
		{
			//change visiblilty of nodes. 
			if(movePoint && movePoint.renderer.enabled == true)
				movePoint.renderer.enabled = false; 
		}
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
