using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
public class NavigationNodeBehavior : MonoBehaviour 
{
	
	static List<NavigationNodeBehavior> openList = new List<NavigationNodeBehavior>(); 
	static List<NavigationNodeBehavior> closedList = new List<NavigationNodeBehavior>(); 
	static List<NavigationNodeBehavior> allNodeList = new List<NavigationNodeBehavior>(); 
	
	public List<NavigationNodeBehavior> neighborList = new List<NavigationNodeBehavior>(); 
	static List<NavigationNodeBehavior> pathToTarget = new List<NavigationNodeBehavior>(); 
	
	public float costSoFar = 0.0f; 
	public NavigationNodeBehavior previousPathNode = null; 
	public GameObject lastedVisitedBy = null; 
	public int lastVisitedFrame = 0; 
	
	public Material baseColor; 
	public Material selectedColor; 
	
	public GridBehavior theGrid; 
	

	// Use this for initialization
	void Start () 
	{
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
				NavigationNodeBehavior navNodeComponent = navNode.GetComponent<NavigationNodeBehavior>(); 
				if(needToFillAllNodeList && navNodeComponent != null)
				{
					allNodeList.Add(navNodeComponent); 
				}
				
				//check for ordinal locations. 
				//foreach(MovePoint mp in theGrid)
				{
					//check to see if north,east than add to neighbor list.
					//neighbor is full after grid runs. 
					
					neighborList.Add(navNodeComponent); 
					
				}
			}
		}
	
	}
	
	void DebugDraw()
	{
		LineRenderer theLineRenderer = GetComponent<LineRenderer>();
		
		if (theLineRenderer != null)
		{
			theLineRenderer.SetVertexCount(neighborList.Count * 2);
			int vertexIndex = 0;
			foreach (NavigationNodeBehavior neighbor in neighborList)
			{
				theLineRenderer.SetPosition(vertexIndex, transform.position);
				theLineRenderer.SetPosition(vertexIndex+1, neighbor.transform.position);
				vertexIndex += 2;
			}
		}
	}
	
		
	bool HaveLineOfSightOnHero(GameObject heroObject)
	{
		return CollisionManagerBehavior.CanSeeObject(gameObject, heroObject);
	}
	
	public bool HasBeenQueried(GameObject whosAsking)
	{
		if (lastedVisitedBy == whosAsking && lastVisitedFrame == Time.frameCount)
		{
			return true;
		}
		
		return false;
	}
	
	public static NavigationNodeBehavior AssignNeighborsToAIPathNode(GameObject aiObject)
	{
		NavigationNodeBehavior aiNavNode = aiObject.GetComponent<NavigationNodeBehavior>();
		
		if (aiNavNode != null)
		{
			aiNavNode.neighborList.Clear();
			foreach (NavigationNodeBehavior navNode in allNodeList)
			{
				if (CollisionManagerBehavior.CanSeeObject(aiObject, navNode.gameObject))
				{
					aiNavNode.neighborList.Add(navNode);
				}
			}
		}
		
		return aiNavNode;
	}
	
	public static NavigationNodeBehavior FindClosestNavNodeToGameObject(GameObject theObject)
	{
		NavigationNodeBehavior closestNode = null;
		float closestDistance = float.MaxValue;
		
		foreach (NavigationNodeBehavior navNode in allNodeList)
		{
			float distanceToNode = Vector3.Distance(theObject.transform.position, navNode.transform.position);
			
			if (distanceToNode < closestDistance)
			{
				//The cheap check passed, now do the expensive Line of Sight check
				if (CollisionManagerBehavior.CanSeeObject(theObject, navNode.gameObject))
				{
					closestNode = navNode;
					closestDistance = distanceToNode;
				}
			}
		}
		
		return closestNode;
	}
	
	public static void AddNodeToOpenList(NavigationNodeBehavior theNode, float costFromPreviousObject, 
		NavigationNodeBehavior previousNode)
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
	
	
	public static NavigationNodeBehavior FindSmallestCostSoFarInOpenList()
	{
		NavigationNodeBehavior returnedNode = null; 
		float smallestCostSoFar = float.MaxValue;
		
		foreach(NavigationNodeBehavior navNode in openList)
		{
			if(navNode.costSoFar < smallestCostSoFar)
			{
				returnedNode = navNode;
				smallestCostSoFar = navNode.costSoFar; 
			}
		}
		
		return returnedNode; 
		
	}
	
	
	public static List<NavigationNodeBehavior> RunDijsktras(GameObject startingObject, GameObject targetObject)
	{
		openList.Clear(); 
		closedList.Clear(); 
		pathToTarget.Clear(); 
		
		foreach(NavigationNodeBehavior navNode in allNodeList)
		{
			navNode.renderer.material = navNode.baseColor; 
		}
		
		NavigationNodeBehavior startingNode = AssignNeighborsToAIPathNode(startingObject); 
		if(startingNode == null)
		{
			startingNode = FindClosestNavNodeToGameObject(startingObject); 
		}
		
		NavigationNodeBehavior destinationNode = FindClosestNavNodeToGameObject(targetObject); 
		
		if(startingNode == null)
		{
			print("No starting node!"); 
			return pathToTarget; 
		}
		float costFromAIToStartingNode = Vector3.Distance(startingObject.transform.position, startingNode.transform.position); 
		AddNodeToOpenList(startingNode, costFromAIToStartingNode, null); 
		
		NavigationNodeBehavior currentNode = startingNode; 
		
		int sanity = 1000; 
		
		while(currentNode != destinationNode)
		{
			foreach(NavigationNodeBehavior neighborNode in currentNode.neighborList)
			{
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
					float distanceToNode = Vector3.Distance(currentNode.transform.position, neighborNode.transform.position); 
					AddNodeToOpenList(neighborNode, distanceToNode, currentNode); 
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
		
		return pathToTarget; 
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		DebugDraw(); 
	
	}
}
