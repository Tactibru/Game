using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
public class ActorBehavior : MonoBehaviour 
{
    public MovePointBehavior currentMovePoint;
    public MovePointBehavior pointToMoveTo;
    public float timeToMoveToPoint;
    public int side;
    public GridBehavior theGrid;
	
	public List<MovePointBehavior> pathList;
	
    public enum DirectionOfMovement
    {
        North = 0,
        South,
        East,
        West,
        None
    }

    private DirectionOfMovement currentMovementDirection = DirectionOfMovement.None;

    private float currentMovementTime = 0.0f;
    public bool currentlyMoving = false;
    public bool actorHasMovedThisTurn = false;

	/// <summary>
	/// The start function is only making sure the agent starts on its assigned position.
	/// </summary>


	void Start () 
    {
        transform.position = currentMovePoint.transform.position;
	}
	
	/// <summary>
	/// The update function is to catch input and to do the movement of the agent, this is temporary code. for testing purposes only. 
	/// </summary>

	void Update () 
    {
        if (!currentlyMoving)
        {
            if (pathList.Count > 0)
            {
                if (pathList[0] == currentMovePoint.neighborList[0])
                //if (Input.GetKeyDown(KeyCode.UpArrow) && currentMovePoint.North)
                {
                    pointToMoveTo = currentMovePoint.neighborList[0];
                    currentMovementDirection = DirectionOfMovement.North;
                    currentlyMoving = true;
                    currentMovementTime = timeToMoveToPoint;
                }
                else if (pathList[0] == currentMovePoint.neighborList[2])
                //else if (Input.GetKeyDown(KeyCode.DownArrow) && currentMovePoint.South)
                {
                    pointToMoveTo = currentMovePoint.neighborList[2];
                    currentMovementDirection = DirectionOfMovement.South;
                    currentlyMoving = true;
                    currentMovementTime = timeToMoveToPoint;
                }
                else if (pathList[0] == currentMovePoint.neighborList[3])
                //else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentMovePoint.West)
                {
                    pointToMoveTo = currentMovePoint.neighborList[3];
                    currentMovementDirection = DirectionOfMovement.West;
                    currentlyMoving = true;
                    currentMovementTime = timeToMoveToPoint;
                }
                else if (pathList[0] == currentMovePoint.neighborList[1])
                //else if (Input.GetKeyDown(KeyCode.RightArrow) && currentMovePoint.East)
                {
                    pointToMoveTo = currentMovePoint.neighborList[1];
                    currentMovementDirection = DirectionOfMovement.East;
                    currentlyMoving = true;
                    currentMovementTime = timeToMoveToPoint;
                }
            }
        }
        else
        {
            switch (currentMovementDirection)
            {
                case DirectionOfMovement.North:
                    if (currentMovementTime < 0.0f)
                    {
                        currentMovePoint = pointToMoveTo;
                        currentMovementDirection = DirectionOfMovement.None;
                        currentMovementTime = 0.0f;
                        transform.position = currentMovePoint.transform.position;
                        currentlyMoving = false;
                        pointToMoveTo = null;
                        pathList.RemoveAt(0);
                        if (GridBehavior.preCombat && pathList.Count == 0)
                            theGrid.startCombat();
                    }
                    else
                    {
                        float forTForLerp = (timeToMoveToPoint - currentMovementTime) / timeToMoveToPoint;
                        float forTheChangeInZ = Mathf.Lerp(currentMovePoint.transform.position.z, pointToMoveTo.transform.position.z, forTForLerp);
                        transform.position = new Vector3(transform.position.x, transform.position.y, forTheChangeInZ);
                        currentMovementTime -= Time.deltaTime;
                    }
                    break;
                case DirectionOfMovement.South:
                    if (currentMovementTime < 0.0f)
                    {
                        currentMovePoint = pointToMoveTo;
                        currentMovementDirection = DirectionOfMovement.None;
                        currentMovementTime = 0.0f;
                        transform.position = currentMovePoint.transform.position;
                        currentlyMoving = false;
                        pointToMoveTo = null;
                        pathList.RemoveAt(0);
                        if (GridBehavior.preCombat && pathList.Count == 0)
                            theGrid.startCombat();
                    }
                    else
                    {
                        float forTForLerp = (timeToMoveToPoint - currentMovementTime) / timeToMoveToPoint;
                        float forTheChangeInZ = Mathf.Lerp(currentMovePoint.transform.position.z, pointToMoveTo.transform.position.z, forTForLerp);
                        transform.position = new Vector3(transform.position.x, transform.position.y, forTheChangeInZ);
                        currentMovementTime -= Time.deltaTime;
                    }
                    break;
                case DirectionOfMovement.East:
                    if (currentMovementTime < 0.0f)
                    {
                        currentMovePoint = pointToMoveTo;
                        currentMovementDirection = DirectionOfMovement.None;
                        currentMovementTime = 0.0f;
                        transform.position = currentMovePoint.transform.position;
                        currentlyMoving = false;
                        pointToMoveTo = null;
						pathList.RemoveAt(0);
						if (GridBehavior.preCombat && pathList.Count == 0)
							theGrid.startCombat();
                    }
                    else
                    {
                        float forTForLerp = (timeToMoveToPoint - currentMovementTime) / timeToMoveToPoint;
                        float forTheChangeInX = Mathf.Lerp(currentMovePoint.transform.position.x, pointToMoveTo.transform.position.x, forTForLerp);
                        transform.position = new Vector3(forTheChangeInX, transform.position.y, transform.position.z);
                        currentMovementTime -= Time.deltaTime;
                    }
                    break;
                case DirectionOfMovement.West:
                    if (currentMovementTime < 0.0f)
                    {
                        currentMovePoint = pointToMoveTo;
                        currentMovementDirection = DirectionOfMovement.None;
                        currentMovementTime = 0.0f;
                        transform.position = currentMovePoint.transform.position;
                        currentlyMoving = false;
                        pointToMoveTo = null;
                        pathList.RemoveAt(0);
                        if (GridBehavior.preCombat && pathList.Count == 0)
                            theGrid.startCombat();
                    }
                    else
                    {
                        float forTForLerp = (timeToMoveToPoint - currentMovementTime) / timeToMoveToPoint;
                        float forTheChangeInX = Mathf.Lerp(currentMovePoint.transform.position.x, pointToMoveTo.transform.position.x, forTForLerp);
                        transform.position = new Vector3(forTheChangeInX, transform.position.y, transform.position.z);
                        currentMovementTime -= Time.deltaTime;
                    }
                    break;
            }
        }
	}
}
