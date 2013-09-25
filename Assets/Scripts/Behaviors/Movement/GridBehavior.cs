using UnityEngine;
using System.Collections;

public class GridBehavior : MonoBehaviour 
{
    public static int theMapLength = 4;
    public static int theMapWidth = 6;
	
    public MovePointBehavior[] theMap = new MovePointBehavior[theMapWidth * theMapLength];
	
	public GameObject targetNode; 
	public GameObject currentActor; 
    void Start()
    {
		
		
        for (int length = 0; length < theMapLength; length++)
        {
            for (int width = 0; width < theMapWidth; width++)
            {
                if(length < theMapLength -1)
                {
                    if(theMap[width + (length * theMapWidth)] && theMap[width + ((length + 1) * theMapWidth)])
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
		if(currentActor && targetNode)
		{
			RunDijkstras();
			currentActor = null; 
			targetNode = null; 
		}
	
	}
}
