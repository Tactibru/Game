using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class MovePoint : MonoBehaviour 
{
   	
	
	
	static List<MovePoint> openList = new List<MovePoint>(); 
	static List<MovePoint> closedList = new List<MovePoint>(); 
	static List<MovePoint> allNodeList = new List<MovePoint>(); 
	static List<MovePoint> DepthList = new List<MovePoint>(); 
	
	//public List<MovePoint> neighborList = new List<MovePoint>(); 
	static List<MovePoint> pathToTarget = new List<MovePoint>(); 
	
	public MovePoint[] neighborList = new MovePoint[4]; 
	
	public float costSoFar = 0.0f; 
	public MovePoint previousPathNode = null; 
	public GameObject lastedVisitedBy = null; 
	public int lastVisitedFrame = 0; 
	
	public  Material baseColor; 
	public Material selectedColor; 
	
	public GridBehavior theGrid; 

	// Use this for initialization
	void Start () 
    {
		
		/// <summary>
    /// Start initializes all the nodes in the grid. 
    /// </summary>
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
				MovePoint navNodeComponent = navNode.GetComponent<MovePoint>(); 
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
	
	
	/// <summary>
	/// Layers the mask that ignores me.
	/// </summary>
	/// <returns>
	/// used for not casting ray on self. 
	/// The mask that ignores me.
	/// </returns>
	/// <param name='me'>
	/// Me.
	/// </param>
	public static int LayerMaskThatIgnoresMe(GameObject me)
	{
		int layerMask = 1<<(LayerMask.NameToLayer("Ignore Raycast")); 
		layerMask |= 1<<me.layer; 
		layerMask = ~layerMask; 
		
		return layerMask; 
	}
	
	public static bool CanSeeObject(GameObject viewerObject, GameObject targetObject, float visionConeAngle = 180.0f)
	{
		if(!targetObject)
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
		
	}
	/// <summary>
	/// Finds the closest nav node to game object.
	/// </summary>
	/// <returns>
	/// The closest nav node to game object.
	/// </returns>
	/// <param name='theObject'>
	/// The object.
	/// </param>
	public static MovePoint FindClosestNavNodeToGameObject(GameObject theObject)
	{
		MovePoint closestNode = null;
		float closestDistance = float.MaxValue;
		
		foreach (MovePoint navNode in allNodeList)
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
	
	/// <summary>
	/// Adds the node to open list.
	/// </summary>
	/// <param name='theNode'>
	/// The node.
	/// </param>
	/// <param name='costFromPreviousObject'>
	/// Cost from previous object.
	/// </param>
	/// <param name='previousNode'>
	/// Previous node.
	/// </param>
	public static void AddNodeToOpenList(MovePoint theNode, float costFromPreviousObject, 
		MovePoint previousNode)
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
	
	/// <summary>
	/// Finds the smallest cost so far in open list.
	/// </summary>
	/// <returns>
	/// The smallest cost so far in open list.
	/// </returns>
	public static MovePoint FindSmallestCostSoFarInOpenList()
	{
		MovePoint returnedNode = null; 
		float smallestCostSoFar = float.MaxValue;
		
		foreach(MovePoint navNode in openList)
		{
			if(navNode.costSoFar < smallestCostSoFar)
			{
				returnedNode = navNode;
				smallestCostSoFar = navNode.costSoFar; 
			}
		}
		
		return returnedNode; 
		
	}
	
	/// <summary>
	/// Runs the dijsktras. populates a list that the actor can use to navigate the grid
	/// </summary>
	/// <returns>
	/// The dijsktras.
	/// </returns>
	/// <param name='startingObject'>
	/// Starting object.
	/// </param>
	/// <param name='targetObject'>
	/// Target object.
	/// </param>
	public static List<MovePoint> RunDijsktras(GameObject startingObject, GameObject targetObject)
	{
		openList.Clear(); 
		closedList.Clear(); 
		pathToTarget.Clear(); 
		
		foreach(MovePoint navNode in allNodeList)
		{
			navNode.renderer.material = navNode.baseColor; 
		}
		
		MovePoint startingNode = null; 
		if(startingNode == null)
		{
			startingNode = FindClosestNavNodeToGameObject(startingObject); 
		}
		
		MovePoint destinationNode = FindClosestNavNodeToGameObject(targetObject); 
		
		if(startingNode == null)
		{
			print("No starting node!"); 
			return pathToTarget; 
		}
		float costFromAIToStartingNode = Vector3.Distance(startingObject.transform.position, startingNode.transform.position); 
		AddNodeToOpenList(startingNode, costFromAIToStartingNode, null); 
		
		MovePoint currentNode = startingNode; 
		
		int sanity = 1000; 
		int count = 0; 
		while(currentNode != destinationNode)
		{
			foreach(MovePoint neighborNode in currentNode.neighborList)
			{
				if(!neighborNode)
				{
					continue; 
				}
				
				print(count.ToString()); 
				if(closedList.Contains(neighborNode))
					continue; 
				else if(openList.Contains(neighborNode))
				{
					float costToNode = currentNode.costSoFar;
					float distanceToNode = Vector3.Distance(currentNode.transform.position, neighborNode.transform.position); 
					
					if(neighborNode.costSoFar > costToNode + distanceToNode)
					{
						neighborNode.costSoFar = costToNode + distanceToNode; 
						neighborNode.previousPathNode = currentNode; 
					}
				}
				else
				{
					print(currentNode.transform.position.ToString()); 
					print(neighborNode.transform.position.ToString()); 
					
					float distanceToNode = Vector3.Distance(currentNode.transform.position, neighborNode.transform.position); 
					print(distanceToNode.ToString()); 
					AddNodeToOpenList(neighborNode, distanceToNode, currentNode); 
				}
				count++; 
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
		
		pathToTarget.Reverse(); 
		return pathToTarget; 
		
		
	}


    public static void DepthFirstSearch(Actor actor)
    {
        foreach(MovePoint node in actor.pathList)
        {
			foreach(MovePoint second in node.neighborList)
			{
				DepthList.Add(second); 
				second.renderer.material = second.baseColor;
			}
				

        }
    }


	
	// Update is called once per frame
	void Update () {
	
	}
}
