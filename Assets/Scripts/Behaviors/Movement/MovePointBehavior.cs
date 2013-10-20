using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Units;

public class MovePointBehavior : MonoBehaviour 
{
	List<MovePointBehavior> openList = new List<MovePointBehavior>(); 
	List<MovePointBehavior> closedList = new List<MovePointBehavior>(); 
	List<MovePointBehavior> allNodeList = new List<MovePointBehavior>(); 
	
	List<MovePointBehavior> pathToTarget = new List<MovePointBehavior>();
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
	
	
	
	/// <summary>
	/// Determines whether this instance has been queried the specified whosAsking.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance has been queried the specified whosAsking; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='whosAsking'>
	/// If set to <c>true</c> whos asking.
	/// </param>
	public bool HasBeenQueried(GameObject whosAsking)
	{
		if (lastedVisitedBy == whosAsking && lastVisitedFrame == Time.frameCount)
		{
			return true;
		}
		
		return false;
	}
	
	
	/// <summary>
	/// Makes nodes ignore raycasts from themselves
	/// Layers the mask that ignores me.
	/// </summary>
	/// <returns>
	/// The mask that ignores me.
	/// </returns>
	/// <param name='me'>
	/// 
	/// </param>
	public int LayerMaskThatIgnoresMe(GameObject me)
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
	
	//tried to run this again, did not fix the game crash. 
	public bool CanSeeObject(GameObject viewerObject, GameObject targetObject, float visionConeAngle)
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
	
	public MovePointBehavior FindClosestNavNodeToGameObject(GameObject theObject)
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
					//CAN BE BROKEN
					closestNode = navNode;
					closestDistance = distanceToNode;
				}
			}
		}
		
		return closestNode;
	}
	/// <summary>
	/// This is where the algorithm adds a node to the open list
    /// calculates cost so far and then sets the previous path nodes for dijkstras to follow. 
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
	public void AddNodeToOpenList(MovePointBehavior theNode, float costFromPreviousObject, 
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
	
	/// <summary>
	/// Finds the smallest cost so far in open list.
	/// </summary>
	/// <returns>
	/// The smallest cost so far in open list.
	/// </returns>
	public MovePointBehavior FindSmallestCostSoFarInOpenList()
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
	/// <summary>
	/// Runs the dijsktras.
	///  Dijkstras will run and select the correct path for the unit to use to find their target. 
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
	public List<MovePointBehavior> RunDijsktras(GameObject startingObject, GameObject targetObject, GridBehavior theGrid)
	{
		openList.Clear(); 
		closedList.Clear(); 
		pathToTarget.Clear(); 
		
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
			if(currentNode == null)
				continue;
			
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

        //pathToTarget.RemoveAt(0);
		pathToTarget.Reverse();
        pathToTarget.RemoveAt(0);
        

        if (GridBehavior.preCombat)
            pathToTarget.RemoveAt(pathToTarget.Count - 1);
        
		return pathToTarget;
	}

	/// <summary>
	/// Creates a list of nodes that the actor can move to.
	/// </summary>
	/// <param name="actor"></param>
    public void DepthFirstSearch(ActorBehavior actor)
    {
		int depth = 0;

		if(actor.currentMovePoint == null)
		{
			Debug.LogError("Current move point is null!");
			return;
		}

		int maxDistance = 0;
		CombatSquadBehavior csb = actor.GetComponent<CombatSquadBehavior>();
		if (csb == null)
			Debug.LogWarning("Attempting to move a unit that does not have a squad associated!");

		maxDistance = (csb == null ? 1 : csb.Squad.Speed);

		doDFS(actor.currentMovePoint, maxDistance, depth);
    }

	/// <summary>
	/// Performs the logic behind the depth-first search.
	/// </summary>
	/// <param name="movePoint">Node to perform the depth-first check on.</param>
	/// <param name="maxDepth">Maximum depth to perform checking to.</param>
	/// <param name="currentDepth">Current depth within the search.</param>
	private void doDFS(MovePointBehavior movePoint, int maxDepth, int currentDepth)
	{
		if (currentDepth >= maxDepth || movePoint.neighborList.Length == 0)
			return;

		currentDepth++;

		foreach (MovePointBehavior neighbor in movePoint.neighborList)
		{
			if (neighbor == null)
				continue;

			neighbor.renderer.enabled = true;
			doDFS(neighbor, maxDepth, currentDepth);
		}
	}
}
