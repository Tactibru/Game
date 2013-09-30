using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIScript : MonoBehaviour 
{
	GameObject targetObject = null; 
	//public for debug
	public float visionConeAngle = 5.0f; 
	Vector3 target; 
	//public for debug
	public List<NavigationNode> pathToTarget = new List<NavigationNode>(); 
	public float speed =0.00001f; 
	// Use this for initialization
	void Start () 
	{
		targetObject = GameObject.FindGameObjectWithTag("Player"); 
	}
	//line of sight check to see if we can see our target. 
	bool CanSeeTarget()
	{
		return CollisionManager.CanSeeObject(gameObject, targetObject, visionConeAngle); 
	}
	/// <summary>
	/// Gets the target point.
	/// </summary>
	/// if it can see a target go to the target.
	/// if not, than you need to run dijsktras and find the best path. 
	/// <returns>
	/// The target point.
	/// </returns>
	Vector3 GetTargetPoint()
	{
		if(CanSeeTarget())
		{
			return targetObject.transform.position; 
		}
		else
		{
	
			pathToTarget = NavigationNode.RunDijsktras(gameObject, targetObject); 
			print(string.Format("found {0} nodes in path", pathToTarget.Count)); 
			foreach(NavigationNode itemInPath in pathToTarget)
			{
				
				if(CollisionManager.CanSeeObject(gameObject, itemInPath.gameObject))
				{
					return itemInPath.transform.position;
				}
			}
			
			print("FAIL- I cant see nodes!"); 
			return transform.position; 
			
		}
		
	}
	/// <summary>
	/// Move the specified speed.
	/// this will make the agent go to its target location.
	/// Collision doesnt collide. 
	/// </summary>
	/// <param name='speed'>
	/// Speed.
	/// </param>
	void Move(float speed)
	{
		Vector3 endPos = transform.position + transform.forward * speed * Time.deltaTime; 
		Vector3 velocity = transform.forward * speed; 
		//NO COLLISION!
		if(CollisionManager.CheckCollision(gameObject, 0.5f, transform.position, ref endPos, ref velocity, targetObject))
		{
			velocity.y = 0.0f; 
			transform.rotation = Quaternion.LookRotation(velocity); 
		}
		transform.position = endPos; 
	}
	
	void GoToTarget()
	{
		Vector3 targetPosition = GetTargetPoint();
		Vector3 vectorToTarget = targetPosition - transform.position; 
		float turnRate = 50.0f;
		float angleToTarget = Vector3.Angle(vectorToTarget.normalized, transform.forward); 
		if(Vector3.Dot(vectorToTarget, transform.right) < 0.0f)
		{
			angleToTarget *= -1.0f; 
		}
		Vector3 eulerAngles = transform.rotation.eulerAngles; 
		
		if(Mathf.Abs(angleToTarget) > turnRate * Time.deltaTime)
		{
			eulerAngles.y += ((angleToTarget > 0.0f) ? 1.0f : -1.0f) * turnRate * Time.deltaTime; 
		}
		transform.rotation = Quaternion.Euler(eulerAngles); 
		
		
		
		Move(speed); 
	}
	// Update is called once per frame
	void Update () 
	{
		
		GoToTarget(); 
		
	}
}
