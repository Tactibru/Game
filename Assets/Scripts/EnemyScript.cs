using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{
	GameObject heroObject = null;
	List<WayPoint> wayPointList = new List<WayPoint>();
    public GameObject returnPoint = null;
	
	public float visionConeAngle = 5.0f;
	public float moveSpeed = 3.0f;
	public float chargeSpeed = 10.0f;
	public float turnRate = 60.0f;
	public float accelerationRate = 10.0f;
	public float nearTargetDistance = 10.0f;
	public float tightTurnRate = 180.0f;
	public float drag = 0.98f;
	public float timeToTarget = 0.75f;
	
	float spinTime = 4.0f;
	float rotationRate = 90.0f;
	float timeLeftInState = 0.0f;
	float patrolTime = 5.0f;
	
	float currentSpeed = 0.0f;
	
	
	ENEMY_STATES currentState = ENEMY_STATES.CHARGE;
	
	//list of enemies
	
	
	public enum ENEMY_STATES
	{
		IDLE = 0,
		PATROL,
		CHARGE,
        FLEE,
		NUM_STATES
	};
	
	delegate void StateUpdate();
	StateUpdate[] StateUpdateArray = new StateUpdate[(int)ENEMY_STATES.NUM_STATES];
	
	public void ChangetoState(ENEMY_STATES newState)
	{
		currentState = newState;
		
		switch(newState)
		{
			case ENEMY_STATES.IDLE:
				timeLeftInState = Random.Range(spinTime/4.0f, spinTime);
				break;
			
			case ENEMY_STATES.PATROL:
				timeLeftInState = Random.Range(patrolTime/4.0f, patrolTime);
				break;
			
			case ENEMY_STATES.CHARGE:
				Vector3 vectorToHero = heroObject.transform.position - transform.position;
				timeLeftInState = 20.0f;//(vectorToHero.magnitude + 2.0f) / chargeSpeed;
				currentSpeed = 0.0f;
				break;

            case ENEMY_STATES.FLEE:
                timeLeftInState = 3.0f;
                break;
			
			default:
				break;
		}
	}

	// Use this for initialization
	void Start () 
	{
		heroObject = GameObject.FindGameObjectWithTag("Player");
		
		if (heroObject != null)
		{
			//print("Player found!");
		}
		
		StateUpdateArray[(int)ENEMY_STATES.IDLE] = SpinningIdle;
		StateUpdateArray[(int)ENEMY_STATES.PATROL] = Patrol;
		StateUpdateArray[(int)ENEMY_STATES.CHARGE] = ChargeHero;
        StateUpdateArray[(int)ENEMY_STATES.FLEE] = Flee;
		
		//Find/add way points
		GameObject[] wayPointObjects = GameObject.FindGameObjectsWithTag("WayPoint");
		
		if (wayPointObjects.Length > 0)
		{
			//print(string.Format("Enemy Found {0} Way Point objects", wayPointObjects.Length));
			
			foreach (GameObject wayPoint in wayPointObjects)
			{
				WayPoint wayPointComponent = wayPoint.GetComponent<WayPoint>();
				
				if (wayPointComponent != null)
				{
					wayPointList.Add(wayPointComponent);
				}
			}
		}
	}
	
	bool CanSeeHero()
	{
		return CollisionManager.CanSeeObject(gameObject, heroObject, visionConeAngle);
	}
	
	Vector3 GetToHero()
	{
		if (CanSeeHero())
		{
			return heroObject.transform.position;
		}
		else
		{
			float closestLOSWayPointDistance = float.MaxValue;
			WayPoint closestLOSWayPoint = null;
			foreach (WayPoint wayPoint in wayPointList)
			{
				if (CollisionManager.CanSeeObject(gameObject, wayPoint.gameObject))
				{
					Vector3 vectorToWayPoint = wayPoint.transform.position - transform.position;
					if (vectorToWayPoint.magnitude < closestLOSWayPointDistance)
					{
						closestLOSWayPointDistance = vectorToWayPoint.magnitude;
						closestLOSWayPoint = wayPoint;
					}
				}
			}
			
			if (closestLOSWayPoint != null)
			{
				closestLOSWayPoint.DistanceToPoint(gameObject, closestLOSWayPointDistance,
					heroObject);
				
				foreach (WayPoint itemInPath in closestLOSWayPoint.pathToTargetPoint)
				{
					if (CollisionManager.CanSeeObject(gameObject, itemInPath.gameObject))
					{
						return itemInPath.transform.position;
					}
				}
				
				//I have line of sight on none of the way points in the path.  Fall back to the closest one to me.
				return closestLOSWayPoint.transform.position;
			}
			else
			{
				//we're screwed.  Return our position as a safe fallback.
				return transform.position;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeLeftInState -= Time.deltaTime;
		StateUpdateArray[(int)currentState]();
		
		if (CaughtHero())
		{
			HeroScript hero = heroObject.GetComponent<HeroScript>();
			hero.gameOver = true;
		}
	}
	
	void Move(float speed)
	{
		Vector3 endPos = transform.position + transform.forward * speed * Time.deltaTime;
		Vector3 velocity = transform.forward * speed;
		if (CollisionManager.CheckCollision(gameObject, 0.5f, transform.position, ref endPos, ref velocity, heroObject))
		{
			//face our new velocity
			velocity.y = 0.0f;			//just in case...
			transform.rotation = Quaternion.LookRotation(velocity);
		}
		transform.position = endPos;
	}
	
	void SpinningIdle()
	{
		Vector3 currentOrientation = transform.rotation.eulerAngles;
		currentOrientation.y += rotationRate * Time.deltaTime;
		transform.rotation = Quaternion.Euler(currentOrientation);
		
		if (timeLeftInState < 0)
		{
			ChangetoState(ENEMY_STATES.PATROL);
		}
		
		//if (CanSeeHero())
		{
			ChangetoState(ENEMY_STATES.CHARGE);
		}
	}
	
	void ChargeHero()
	{
		Vector3 targetPosition = GetToHero();
		Vector3 vectorToHero = targetPosition - transform.position;
		float currentTurnRate = (vectorToHero.magnitude < nearTargetDistance) ? tightTurnRate : turnRate;
		float angleToHero = Vector3.Angle(vectorToHero.normalized, transform.forward);
		if (Vector3.Dot(vectorToHero, transform.right) < 0.0f)
		{
			angleToHero *= -1.0f;
		}
		Vector3  eulerAngles  = transform.rotation.eulerAngles;
		
		if (Mathf.Abs(angleToHero) > currentTurnRate * Time.deltaTime)
		{
			eulerAngles.y += ((angleToHero > 0.0f) ? 1.0f : -1.0f) * currentTurnRate * Time.deltaTime;
		}
		transform.rotation = Quaternion.Euler(eulerAngles);
		
		float desiredSpeed = moveSpeed;
		if (vectorToHero.magnitude > nearTargetDistance)
		{
			desiredSpeed = chargeSpeed;
		}
		float diffBewteenCurrentSpeedAndDesiredSpeed = desiredSpeed - currentSpeed;
		if (Mathf.Abs(diffBewteenCurrentSpeedAndDesiredSpeed) > accelerationRate * Time.deltaTime)
		{
			currentSpeed += ((diffBewteenCurrentSpeedAndDesiredSpeed > 0.0f) ? 1.0f : -1.0f) * accelerationRate * Time.deltaTime;
		}
		currentSpeed *= 1.0f - ((1.0f - drag) * Time.deltaTime);
		currentSpeed = Mathf.Min(vectorToHero.magnitude / timeToTarget, currentSpeed);
		HeroScript theHeroScript = heroObject.GetComponent<HeroScript>();
		if (theHeroScript != null && theHeroScript.gameOver == false)
		{
			currentSpeed = Mathf.Min(theHeroScript.moveSpeed, currentSpeed);
		}
		
		Move(currentSpeed);
		
		if (timeLeftInState < 0)
		{
			ChangetoState(ENEMY_STATES.IDLE);
		}
	}
	
	void Patrol()
	{
		Move(moveSpeed);
		//transform.position += transform.forward * moveSpeed * Time.deltaTime;
		
		if (timeLeftInState < 0)
		{
			ChangetoState(ENEMY_STATES.IDLE);
		}
		
		//if (CanSeeHero())
		{
			ChangetoState(ENEMY_STATES.CHARGE);
		}
	}

    bool CanSeeReturnPoint()
    {
        return CollisionManager.CanSeeObject(gameObject, returnPoint, visionConeAngle);
    }

    Vector3 GetToReturnPoint()
    {
        if (CanSeeReturnPoint())
        {
            return returnPoint.transform.position;
        }
        else
        {
            float closestLOSWayPointDistance = float.MaxValue;
            WayPoint closestLOSWayPoint = null;
            foreach (WayPoint wayPoint in wayPointList)
            {
                if (CollisionManager.CanSeeObject(gameObject, wayPoint.gameObject))
                {
                    Vector3 vectorToWayPoint = wayPoint.transform.position - transform.position;
                    if (vectorToWayPoint.magnitude < closestLOSWayPointDistance)
                    {
                        closestLOSWayPointDistance = vectorToWayPoint.magnitude;
                        closestLOSWayPoint = wayPoint;
                    }
                }
            }

            if (closestLOSWayPoint != null)
            {
                closestLOSWayPoint.DistanceToPoint(gameObject, closestLOSWayPointDistance,
                    returnPoint);

                foreach (WayPoint itemInPath in closestLOSWayPoint.pathToTargetPoint)
                {
                    if (CollisionManager.CanSeeObject(gameObject, itemInPath.gameObject))
                    {
                        return itemInPath.transform.position;
                    }
                }

                //I have line of sight on none of the way points in the path.  Fall back to the closest one to me.
                return closestLOSWayPoint.transform.position;
            }
            else
            {
                //we're screwed.  Return our position as a safe fallback.
                return transform.position;
            }
        }
    }

    void Flee()
    {
        Vector3 targetPosition = GetToReturnPoint();
        Vector3 vectorToReturnPoint = targetPosition - transform.position;
        float currentTurnRate = (vectorToReturnPoint.magnitude < nearTargetDistance) ? tightTurnRate : turnRate;
        float angleToReturnPoint = Vector3.Angle(vectorToReturnPoint.normalized, transform.forward);
        if (Vector3.Dot(vectorToReturnPoint, transform.right) < 0.0f)
        {
            angleToReturnPoint *= -1.0f;
        }
        Vector3 eulerAngles = transform.rotation.eulerAngles;

        if (Mathf.Abs(angleToReturnPoint) > currentTurnRate * Time.deltaTime)
        {
            eulerAngles.y += ((angleToReturnPoint > 0.0f) ? 1.0f : -1.0f) * currentTurnRate * Time.deltaTime;
        }
        transform.rotation = Quaternion.Euler(eulerAngles);

        float desiredSpeed = moveSpeed;
        if (vectorToReturnPoint.magnitude > nearTargetDistance)
        {
            desiredSpeed = chargeSpeed;
        }
        float diffBewteenCurrentSpeedAndDesiredSpeed = desiredSpeed - currentSpeed;
        if (Mathf.Abs(diffBewteenCurrentSpeedAndDesiredSpeed) > accelerationRate * Time.deltaTime)
        {
            currentSpeed += ((diffBewteenCurrentSpeedAndDesiredSpeed > 0.0f) ? 1.0f : -1.0f) * accelerationRate * Time.deltaTime;
        }
        currentSpeed *= 1.0f - ((1.0f - drag) * Time.deltaTime);
        currentSpeed = Mathf.Min(vectorToReturnPoint.magnitude / timeToTarget, currentSpeed);

        Move(currentSpeed);

        if (timeLeftInState < 0)
        {
            ChangetoState(ENEMY_STATES.IDLE);
        }
        //transform.position += transform.forward * (-moveSpeed) * Time.deltaTime;

        //if (timeLeftInState < 0)
        //{
        //    ChangetoState(ENEMY_STATES.IDLE);
        //}
    }
	
	bool CaughtHero()
	{
		if (!heroObject)
			return false;
		
		Vector3 vectorToHero = heroObject.transform.position - transform.position;
		
		if (vectorToHero.magnitude < 1.0f)
			return true;
		return false;
	}
}
