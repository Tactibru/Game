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
        Debug.Log("Here is for combat start.");
        //Current actor is attacker and target actor is defender.
        
        targetActor = null;
        currentActor = null;
        preCombat = false;
    }
}
