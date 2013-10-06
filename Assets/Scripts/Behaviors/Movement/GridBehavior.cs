using UnityEngine;
using System.Collections;

public class GridBehavior : MonoBehaviour 
{
    public int theMapLength = 10;
    public int theMapWidth = 20;

    public MovePointBehavior[] theMap;
	
	public GameObject targetNode; 
	public GameObject currentActor; 
    void Start()
    {
        //theMap = new MovePointBehavior[theMapLength * theMapWidth];
		
        for (int length = 0; length < theMapLength; length++)
        {
            for (int width = 0; width < theMapWidth; width++)
            {
                //if (theMap[width + (length * theMapWidth)])
                //{
                //    theMap[width + (length * theMapWidth)].index = width + (length * theMapWidth);
                    if (length < theMapLength - 1)
                    {
                        if (theMap[width + (length * theMapWidth)] && theMap[width + ((length + 1) * theMapWidth)])
                        {
                            theMap[width + (length * theMapWidth)].neighborList[0] = theMap[width + ((length + 1) * theMapWidth)];
                            theMap[width + ((length + 1) * theMapWidth)].neighborList[2] = theMap[width + (length * theMapWidth)];
                            theMap[width + (length * theMapWidth)].North = theMap[width + ((length + 1) * theMapWidth)];
                            theMap[width + ((length + 1) * theMapWidth)].South = theMap[width + (length * theMapWidth)];
                        }
                    }
                    if (width < theMapWidth - 1)
                    {
                        if (theMap[width + (length * theMapWidth)] && theMap[width + 1 + (length * theMapWidth)])
                        {
                            theMap[width + (length * theMapWidth)].neighborList[1] = theMap[width + 1 + (length * theMapWidth)];
                            theMap[width + 1 + (length * theMapWidth)].neighborList[3] = theMap[width + (length * theMapWidth)];
                            theMap[width + (length * theMapWidth)].East = theMap[width + 1 + (length * theMapWidth)];
                            theMap[width + 1 + (length * theMapWidth)].West = theMap[width + (length * theMapWidth)];
                        }
                    }
                //}
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
			currentActor.GetComponent<ActorBehavior>().pathList = MovePointBehavior.RunDijsktras(currentActor.GetComponent<ActorBehavior>().currentMovePoint.gameObject, targetNode); 
		}
	}
	
    // Update is called once per frame
    void Update()
    {
		
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			RaycastHit hitInfo; 
			
			if(Physics.Raycast(ray, out hitInfo))
			{
				if(hitInfo.transform.GetComponent<MovePointBehavior>())
				{
					targetNode = hitInfo.transform.gameObject; 
					
				}
				else if(hitInfo.transform.GetComponent<ActorBehavior>())
				{
					//Check if you click on a squad. 
					currentActor = hitInfo.transform.gameObject; 
				}
			}
			
		}
		if(currentActor)
		{
			MovePoint.DepthFirstSearch(currentActor.GetComponent<Actor>()); 
			if(targetNode)
			{
				RunDijkstras();
			
			}
		}
		currentActor = null; 
		targetNode = null; 
	}
}
