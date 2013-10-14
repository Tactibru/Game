using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class MovePointBehavior : MonoBehaviour 
{
    //public MovePointBehavior North;
    //public MovePointBehavior South;
    //public MovePointBehavior East;
    //public MovePointBehavior West;

	/* static */ List<MovePointBehavior> openList = new List<MovePointBehavior>(); 
	/* static */ List<MovePointBehavior> closedList = new List<MovePointBehavior>(); 
	/* static */ List<MovePointBehavior> allNodeList = new List<MovePointBehavior>(); 
	
	//public List<MovePoint> neighborList = new List<MovePoint>(); 
	/* static */ List<MovePointBehavior> pathToTarget = new List<MovePointBehavior>();
    public int index;
	
	public MovePointBehavior[] neighborList = new MovePointBehavior[4]; 
	
	public float costSoFar = 0.0f; 
	public MovePointBehavior previousPathNode = null; 
	public GameObject lastedVisitedBy = null; 
	public int lastVisitedFrame = 0; 
	
	public Material baseColor; 
	public Material selectedColor; 
	
	public GridBehavior theGrid; 

	// Use this for initialization
	void Start () 
    {
		renderer.enabled = false; 
        //for(int i= 0; i < 4; i++)
        //{
			 
        //    //neighborList[i] = null; 
        //}
		
		theGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridBehavior>(); 
		GameObject[] navNodeObjects = GameObject.FindGameObjectsWithTag("Waypoint"); 
		
		bool needToFillAllNodeList = false; 
		
		if(allNodeList.Count == 0)
		{
			needToFillAllNodeList = true; 
		}
		
		if(navNodeObjects.Length > 0)
		{
			foreach(GameObject navNode in navNodeObjects)
			{
				MovePointBehavior navNodeComponent = navNode.GetComponent<MovePointBehavior>(); 
				if(needToFillAllNodeList && navNodeComponent != null)
				{
					allNodeList.Add(navNodeComponent); 
				}

				//check for ordinal locations. 
				//foreach(MovePoint mp in theGrid)
				
					//check to see if north,east than add to neighbor list.
					//neighbor is full after grid runs. 
			}
		}
		
	
	}
	
	
	
	
	public bool HasBeenQueried(GameObject whosAsking)
	{
		if (lastedVisitedBy == whosAsking && lastVisitedFrame == Time.frameCount)
		{
			return true;
		}
		
		return false;
	}
	
	
	
	public /* static */ int LayerMaskThatIgnoresMe(GameObject me)
	{
		int layerMask = 1<<(LayerMask.NameToLayer("Ignore Raycast")); 
		layerMask |= 1<<me.layer; 
		layerMask = ~layerMask; 
		
		return layerMask; 
	}

	public bool CanSeeObject(GameObject viewerObject, GameObject targetObject)
	{
		return CanSeeObject(viewerObject, targetObject, 180.0f);
	}
	
	public /* static */ bool CanSeeObject(GameObject viewerObject, GameObject targetObject, float visionConeAngle)
	{
				return true;
		/*if(!targetObject)
			return false; 
		
		Vector3 vectorToObject = targetObject.transform.position - viewerObject.transform.position; 
		float angle = Vector3.Angle(viewerObject.transform.forward, vectorToObject.normalized); 
		if(angle <= visionConeAngle)
		{
			RaycastHit hitInfo;
			int layerMask = LayerMaskThatIgnoresMe(viewerObject); 
			if(Physics.Raycast(viewerObject.transform.position, vectorToObject.normalized, out hitInfo, vectorToObject.magnitude, layerMask))
			{
				return hitInfo.transform.gameObject == targetObject; 
			}
			else
			{
				return true; 
			}
			
		}
		return false; 
		*/
	}
	
	public /* static */ MovePointBehavior FindClosestNavNodeToGameObject(GameObject theObject)
	{
		MovePointBehavior closestNode = null;
		float closestDistance = float.MaxValue;
		
		foreach (MovePointBehavior navNode in allNodeList)
		{
			float distanceToNode = Vector3.Distance(theObject.transform.position, navNode.transform.position);
			
			if (distanceToNode < closestDistance)
			{
				//The cheap check passed, now do the expensive Line of Sight check
				if (CanSeeObject(theObject, navNode.gameObject))
				{
					closestNode = navNode;
					closestDistance = distanceToNode;
				}
			}
		}
		
		return closestNode;
	}
	
	public /* static */ void AddNodeToOpenList(MovePointBehavior theNode, float costFromPreviousObject, 
		MovePointBehavior previousNode)
	{
		float costSoFar = costFromPreviousObject;
		if (previousNode != null)
		{
			costSoFar += previousNode.costSoFar;
		}
		theNode.costSoFar = costSoFar;
		theNode.previousPathNode = previousNode;
		openList.Add(theNode);
	}
	
	
	public /* static */ MovePointBehavior FindSmallestCostSoFarInOpenList()
	{
		MovePointBehavior returnedNode = null; 
		float smallestCostSoFar = float.MaxValue;
		
		foreach(MovePointBehavior navNode in openList)
		{
			if(navNode.costSoFar < smallestCostSoFar)
			{
				returnedNode = navNode;
				smallestCostSoFar = navNode.costSoFar; 
			}
		}
		
		return returnedNode; 
		
	}
	
	public /* static */ List<MovePointBehavior> RunDijsktras(GameObject startingObject, GameObject targetObject)
	{
		openList.Clear(); 
		closedList.Clear(); 
		pathToTarget.Clear(); 

        GridBehavior theGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridBehavior>(); 
		
		foreach(MovePointBehavior navNode in allNodeList)
		{
			navNode.renderer.material = navNode.baseColor; 
		}
		
		MovePointBehavior startingNode = FindClosestNavNodeToGameObject(startingObject); 
		
		MovePointBehavior destinationNode = FindClosestNavNodeToGameObject(targetObject); 
		
		if(startingNode == null)
		{
			print("No starting node!"); 
			return pathToTarget;
		}

		float costFromAIToStartingNode = Vector3.Distance(startingObject.transform.position, startingNode.transform.position); 
		AddNodeToOpenList(startingNode, costFromAIToStartingNode, null); 
		
		MovePointBehavior currentNode = startingNode; 
		
		int sanity = 1000; 
		 
		while(currentNode != destinationNode)
		{
			foreach(MovePointBehavior neighborNode in currentNode.neighborList)
			{
				if(!neighborNode)
				{
					continue; 
				}

                bool ignored = false;

                for (int index = 0; index < theGrid.ignoreList.Count; index++)
                {
                    if (neighborNode == theGrid.ignoreList[index])
                    {
                        ignored = true;
                    }
                }
				
				//print(count.ToString()); 
                if (!ignored)
                {
                    if (closedList.Contains(neighborNode))
                        continue;
                    else if (openList.Contains(neighborNode))
                    {
                        float costToNode = currentNode.costSoFar;
                        float distanceToNode = Vector3.Distance(currentNode.transform.position, neighborNode.transform.position);

                        if (neighborNode.costSoFar > costToNode + distanceToNode)
                        {
                            neighborNode.costSoFar = costToNode + distanceToNode;
                            neighborNode.previousPathNode = currentNode;
                        }
                    }
                    else
                    {
                        //print(currentNode.transform.position.ToString()); 
                        //print(neighborNode.transform.position.ToString()); 

                        float distanceToNode = Vector3.Distance(currentNode.transform.position, neighborNode.transform.position);
                        //print(distanceToNode.ToString()); 
                        AddNodeToOpenList(neighborNode, distanceToNode, currentNode);
                    }
                    //count++; 
                }
			}
			closedList.Add(currentNode); 
			if(sanity-- < 0)
			{
				print("RunDijkstras Check 1 Failed"); 
				return pathToTarget; 
			}
			
			currentNode = FindSmallestCostSoFarInOpenList(); 
			openList.Remove(currentNode);
		}
		
		sanity = 1000; 
		while(currentNode != null)
		{
			
			currentNode.renderer.material = currentNode.selectedColor; 
			
			pathToTarget.Add(currentNode); 
			currentNode = currentNode.previousPathNode; 
			if(sanity-- < 0)
			{
				print("RunDijkstras check 2 failed"); 
				return pathToTarget; 
			}
		}
        //if (GridBehavior.preCombat)
        //    pathToTarget.RemoveAt(0);

		pathToTarget.Reverse();
        pathToTarget.RemoveAt(0);

        if (GridBehavior.preCombat)
            pathToTarget.RemoveAt(pathToTarget.Count - 1);
        
		return pathToTarget; 
		
		
	}

    public void DepthFirstSearch(ActorBehavior actor)
    {
        //Need to know grid
        //need to know where the target point is
        //traverse its neighbors and add those to the moveable list
        //once we ran past one neight
        //runa gain and keep adding to the depth list. 

        List<MovePointBehavior> canMoveTo = new List<MovePointBehavior>();
        canMoveTo.Add(actor.currentMovePoint);
        MovePointBehavior temp = actor.currentMovePoint;

        canMoveTo = DFSUtil(temp);

        foreach (MovePointBehavior movePoint in canMoveTo)
        {
            if (!movePoint)
                continue;
            movePoint.renderer.enabled = true; 
        }
    }

    List<MovePointBehavior> DFSUtil(MovePointBehavior node)
    {
        List<MovePointBehavior> canMoveList = new List<MovePointBehavior>();
        foreach (MovePointBehavior neighbor in node.neighborList)
        {
            canMoveList.Add(neighbor); 
        }

        return canMoveList; 
    }
    
	// Update is called once per frame
	void Update () {
	
	}
}
