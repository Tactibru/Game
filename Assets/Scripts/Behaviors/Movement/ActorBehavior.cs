	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Tactibru/Movement/Actor")]
public class ActorBehavior : MonoBehaviour 
{
    public MovePointBehavior currentMovePoint;
    public MovePointBehavior pointToMoveTo;
    public float timeToMoveToPoint = 1.0f;
    public GameControllerBehaviour.UnitSide theSide;
    public GridBehavior theGrid;
    public bool canMove = false;
	
	public List<MovePointBehavior> pathList;
	
    public enum DirectionOfMovement
    {
        North = 0,
        South,
        East,
        West,
        None
    }

    private float currentMovementTime = 0.0f;
    public bool currentlyMoving = false;
    public bool actorHasMovedThisTurn = false;

	/// <summary>
	/// The start function is only making sure the agent starts on its assigned position.
    /// 
    /// Alex Reiss
	/// </summary>


	void Start () 
    {
        transform.position = currentMovePoint.transform.position;
	}
	
	/// <summary>
	/// The update function is to catch input and to do the movement of the agent, this is temporary code. for testing purposes only. 
    /// 
    /// Alex Reiss
	/// </summary>

	void Update () 
    {
        if (!currentlyMoving && canMove)
        {
            if (pathList.Count > 0)
            {
                for (int index = 0; index < currentMovePoint.neighborList.Length; index++)
                {
                    if (currentMovePoint.neighborList[index] == pathList[0])
                    {
                        pointToMoveTo = currentMovePoint.neighborList[index];
                        currentlyMoving = true;
                        currentMovementTime = timeToMoveToPoint;
                    }
                }
            }
           
            //if (pathList.Count > 0)
            //{
                
            //    if (pathList[0] == currentMovePoint.neighborList[0])
            //    //if (Input.GetKeyDown(KeyCode.UpArrow) && currentMovePoint.North)
            //    {
            //        Debug.Log("Hi");
            //        pointToMoveTo = currentMovePoint.neighborList[0];
            //        currentMovementDirection = DirectionOfMovement.North;
            //        currentlyMoving = true;
            //        currentMovementTime = timeToMoveToPoint;
            //    }
            //    else if (pathList[0] == currentMovePoint.neighborList[2])
            //    //else if (Input.GetKeyDown(KeyCode.DownArrow) && currentMovePoint.South)
            //    {
            //        Debug.Log("Hi");
            //        pointToMoveTo = currentMovePoint.neighborList[2];
            //        currentMovementDirection = DirectionOfMovement.South;
            //        currentlyMoving = true;
            //        currentMovementTime = timeToMoveToPoint;
            //    }
            //    else if (pathList[0] == currentMovePoint.neighborList[3])
            //    //else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentMovePoint.West)
            //    {
            //        Debug.Log("Hi");
            //        pointToMoveTo = currentMovePoint.neighborList[3];
            //        currentMovementDirection = DirectionOfMovement.West;
            //        currentlyMoving = true;
            //        currentMovementTime = timeToMoveToPoint;
            //    }
            //    else if (pathList[0] == currentMovePoint.neighborList[1])
            //    //else if (Input.GetKeyDown(KeyCode.RightArrow) && currentMovePoint.East)
            //    {
            //        Debug.Log("Hi");
            //        pointToMoveTo = currentMovePoint.neighborList[1];
            //        currentMovementDirection = DirectionOfMovement.East;
            //        currentlyMoving = true;
            //        currentMovementTime = timeToMoveToPoint;
            //    }
            //}
        }
        else if(canMove)
        {

            if (currentMovementTime < 0.0f)
            {
                currentMovePoint = pointToMoveTo;
                currentMovementTime = 0.0f;
                transform.position = currentMovePoint.transform.position;
                currentlyMoving = false;
                pointToMoveTo = null;
                pathList.RemoveAt(0);
            }
            else
            {
                float forTForLerp = (timeToMoveToPoint - currentMovementTime) / timeToMoveToPoint;
                float forTheChangeInZ = Mathf.Lerp(currentMovePoint.transform.position.z, pointToMoveTo.transform.position.z, forTForLerp);
                float forTheChangeInX = Mathf.Lerp(currentMovePoint.transform.position.x, pointToMoveTo.transform.position.x, forTForLerp);
                transform.position = new Vector3(forTheChangeInX, transform.position.y, forTheChangeInZ);
                currentMovementTime -= Time.deltaTime;
            }

        //    switch (currentMovementDirection)
        //    {
        //        case DirectionOfMovement.North:
        //            if (currentMovementTime < 0.0f)
        //            {
        //                currentMovePoint = pointToMoveTo;
        //                currentMovementDirection = DirectionOfMovement.None;
        //                currentMovementTime = 0.0f;
        //                transform.position = currentMovePoint.transform.position;
        //                currentlyMoving = false;
        //                pointToMoveTo = null;
        //                pathList.RemoveAt(0);
        //            }
        //            else
        //            {
        //                Debug.Log (timeToMoveToPoint);
        //                float forTForLerp = (timeToMoveToPoint - currentMovementTime) / timeToMoveToPoint;
        //                float forTheChangeInZ = Mathf.Lerp(currentMovePoint.transform.position.z, pointToMoveTo.transform.position.z, forTForLerp);
        //                transform.position = new Vector3(transform.position.x, transform.position.y, forTheChangeInZ);
        //                currentMovementTime -= Time.deltaTime;
        //            }
        //            break;
        //        case DirectionOfMovement.South:
        //            if (currentMovementTime < 0.0f)
        //            {
        //                currentMovePoint = pointToMoveTo;
        //                currentMovementDirection = DirectionOfMovement.None;
        //                currentMovementTime = 0.0f;
        //                transform.position = currentMovePoint.transform.position;
        //                currentlyMoving = false;
        //                pointToMoveTo = null;
        //                pathList.RemoveAt(0);
        //            }
        //            else
        //            {
        //                float forTForLerp = (timeToMoveToPoint - currentMovementTime) / timeToMoveToPoint;
        //                float forTheChangeInZ = Mathf.Lerp(currentMovePoint.transform.position.z, pointToMoveTo.transform.position.z, forTForLerp);
        //                transform.position = new Vector3(transform.position.x, transform.position.y, forTheChangeInZ);
        //                currentMovementTime -= Time.deltaTime;
        //            }
        //            break;
        //        case DirectionOfMovement.East:
        //            if (currentMovementTime < 0.0f)
        //            {
        //                currentMovePoint = pointToMoveTo;
        //                currentMovementDirection = DirectionOfMovement.None;
        //                currentMovementTime = 0.0f;
        //                transform.position = currentMovePoint.transform.position;
        //                currentlyMoving = false;
        //                pointToMoveTo = null;
        //                pathList.RemoveAt(0);
        //            }
        //            else
        //            {
        //                float forTForLerp = (timeToMoveToPoint - currentMovementTime) / timeToMoveToPoint;
        //                float forTheChangeInX = Mathf.Lerp(currentMovePoint.transform.position.x, pointToMoveTo.transform.position.x, forTForLerp);
        //                transform.position = new Vector3(forTheChangeInX, transform.position.y, transform.position.z);
        //                currentMovementTime -= Time.deltaTime;
        //            }
        //            break;
        //        case DirectionOfMovement.West:
        //            if (currentMovementTime < 0.0f)
        //            {
        //                currentMovePoint = pointToMoveTo;
        //                currentMovementDirection = DirectionOfMovement.None;
        //                currentMovementTime = 0.0f;
        //                transform.position = currentMovePoint.transform.position;
        //                currentlyMoving = false;
        //                pointToMoveTo = null;
        //                pathList.RemoveAt(0);
        //            }
        //            else
        //            {
        //                float forTForLerp = (timeToMoveToPoint - currentMovementTime) / timeToMoveToPoint;
        //                float forTheChangeInX = Mathf.Lerp(currentMovePoint.transform.position.x, pointToMoveTo.transform.position.x, forTForLerp);
        //                transform.position = new Vector3(forTheChangeInX, transform.position.y, transform.position.z);
        //                currentMovementTime -= Time.deltaTime;
        //            }
        //            break;
        //    }
        }
        
        if (GridBehavior.preCombat && pathList.Count == 0 && canMove)
			theGrid.startCombat();
		
		if (pathList.Count == 0)
			canMove = false;
	}
}
