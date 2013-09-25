using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
public class NavigationNode : MonoBehaviour 
{
	
	static List<NavigationNode> openList = new List<NavigationNode>(); 
	static List<NavigationNode> closedList = new List<NavigationNode>(); 
	static List<NavigationNode> allNodeList = new List<NavigationNode>(); 
	
	public List<NavigationNode> neighborList = new List<NavigationNode>(); 
	static List<NavigationNode> pathToTarget = new List<NavigationNode>(); 
	
	public float costSoFar = 0.0f; 
	public NavigationNode previousPathNode = null; 
	public GameObject lastedVisitedBy = null; 
	public int lastVisitedFrame = 0; 
	
	public Material baseColor; 
	public Material selectedColor; 
	
	public Grid theGrid; 
	

	// Use this for initialization
	void Start () 
	{
		theGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>(); 
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
				NavigationNode navNodeComponent = navNode.GetComponent<NavigationNode>(); 
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
			foreach (NavigationNode neighbor in neighborList)
			{
				theLineRenderer.SetPosition(vertexIndex, transform.position);
				theLineRenderer.SetPosition(vertexIndex+1, neighbor.transform.position);
				vertexIndex += 2;
			}
		}
	}
	
		
	bool HaveLineOfSightOnHero(GameObject heroObject)
	{
		return CollisionManager.CanSeeObject(gameObject, heroObject);
	}
	
	public bool HasBeenQueried(GameObject whosAsking)
	{
		if (lastedVisitedBy == whosAsking && lastVisitedFrame == Time.frameCount)
		{
			return true;
		}
		
		return false;
	}
	
	public static NavigationNode AssignNeighborsToAIPathNode(GameObject aiObject)
	{
		NavigationNode aiNavNode = aiObject.GetComponent<NavigationNode>();
		
		if (aiNavNode != null)
		{
			aiNavNode.neighborList.Clear();
			foreach (NavigationNode navNode in allNodeList)
			{
				if (CollisionManager.CanSeeObject(aiObject, navNode.gameObject))
				{
					aiNavNode.neighborList.Add(navNode);
				}
			}
		}
		
		return aiNavNode;
	}
	
	public static NavigationNode FindClosestNavNodeToGameObject(GameObject theObject)
	{
		NavigationNode closestNode = null;
		float closestDistance = float.MaxValue;
		
		foreach (NavigationNode navNode in allNodeList)
		{
			float distanceToNode = Vector3.Distance(theObject.transform.position, navNode.transform.position);
			
			if (distanceToNode < closestDistance)
			{
				//The cheap check passed, now do the expensive Line of Sight check
				if (CollisionManager.CanSeeObject(theObject, navNode.gameObject))
				{
					closestNode = navNode;
					closestDistance = distanceToNode;
				}
			}
		}
		
		return closestNode;
	}
	
	public static void AddNodeToOpenList(NavigationNode theNode, float costFromPreviousObject, 
		NavigationNode previousNode)
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
	
	
	public static NavigationNode FindSmallestCostSoFarInOpenList()
	{
		NavigationNode returnedNode = null; 
		float smallestCostSoFar = float.MaxValue;
		
		foreach(NavigationNode navNode in openList)
		{
			if(navNode.costSoFar < smallestCostSoFar)
			{
				returnedNode = navNode;
				smallestCostSoFar = navNode.costSoFar; 
			}
		}
		
		return returnedNode; 
		
	}
	
	
	public static List<NavigationNode> RunDijsktras(GameObject startingObject, GameObject targetObject)
	{
		openList.Clear(); 
		closedList.Clear(); 
		pathToTarget.Clear(); 
		
		foreach(NavigationNode navNode in allNodeList)
		{
			navNode.renderer.material = navNode.baseColor; 
		}
		
		NavigationNode startingNode = AssignNeighborsToAIPathNode(startingObject); 
		if(startingNode == null)
		{
			startingNode = FindClosestNavNodeToGameObject(startingObject); 
		}
		
		NavigationNode destinationNode = FindClosestNavNodeToGameObject(targetObject); 
		
		if(startingNode == null)
		{
			print("No starting node!"); 
			return pathToTarget; 
		}
		float costFromAIToStartingNode = Vector3.Distance(startingObject.transform.position, startingNode.transform.position); 
		AddNodeToOpenList(startingNode, costFromAIToStartingNode, null); 
		
		NavigationNode currentNode = startingNode; 
		
		int sanity = 1000; 
		
		while(currentNode != destinationNode)
		{
			foreach(NavigationNode neighborNode in currentNode.neighborList)
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
