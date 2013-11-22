using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayPoint : MonoBehaviour 
{	
	List<WayPoint> neighborList = new List<WayPoint>();
	public List<WayPoint> pathToTargetPoint = new List<WayPoint>();
	
	public GameObject 	lastedVisitedBy = null;
	public int 			lastVisitedFrame = 0;

	// Use this for initialization
	void Start () 
	{
		GameObject[] wayPointObjects = GameObject.FindGameObjectsWithTag("WayPoint");
		
		if (wayPointObjects.Length > 0)
		{
			//print(string.Format("Found {0} Way Point objects", wayPointObjects.Length));
			
			foreach (GameObject wayPoint in wayPointObjects)
			{
				if (wayPoint != gameObject && 
					CollisionManager.CanSeeObject(gameObject, wayPoint))
				{
					WayPoint wayPointComponent = wayPoint.GetComponent<WayPoint>();
					
					if (wayPointComponent != null)
					{
						neighborList.Add(wayPointComponent);
					}
				}
			}
		}
	}
	
	bool HaveLineOfSightOnPoint(GameObject targetPoint)
	{
		return CollisionManager.CanSeeObject(gameObject, targetPoint);
	}
	
	public bool HasBeenQueried(GameObject whosAsking)
	{
		if (lastedVisitedBy == whosAsking && lastVisitedFrame == Time.frameCount)
		{
			return true;
		}
		
		return false;
	}
	
	public float DistanceToPoint(GameObject whosAsking, float distanceFromAsker, GameObject targetPoint)
	{
		lastedVisitedBy = whosAsking;
		lastVisitedFrame = Time.frameCount;
		pathToTargetPoint.Clear();
		
		if (HaveLineOfSightOnPoint(targetPoint))
		{
			Vector3 vectorToTargetPoint = targetPoint.transform.position - transform.position;
			return distanceFromAsker + vectorToTargetPoint.magnitude;
		}
		else
		{
			WayPoint closestNeighborToTargetPoint = null;
			float closestDistanceToTargetPoint = float.MaxValue;
			foreach (WayPoint neighbor in neighborList)
			{
				if (neighbor.HasBeenQueried(whosAsking) == false)
				{
					Vector3 vectorToNeightbor = neighbor.transform.position - transform.position;
					float distance = neighbor.DistanceToPoint(whosAsking, distanceFromAsker + vectorToNeightbor.magnitude,
						targetPoint);
					
					if (distance < closestDistanceToTargetPoint)
					{
						closestDistanceToTargetPoint = distance;
						closestNeighborToTargetPoint = neighbor;
					}
				}
			}
			
			if (closestNeighborToTargetPoint != null)
			{
				foreach (WayPoint item in closestNeighborToTargetPoint.pathToTargetPoint)
				{
					pathToTargetPoint.Add(item);
				}
				pathToTargetPoint.Add (closestNeighborToTargetPoint);
			}
			
			return closestDistanceToTargetPoint;
		}
	}
}
