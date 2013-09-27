using UnityEngine;
using System.Collections;

public class CollisionManagerBehavior : MonoBehaviour 
{
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
	
	public static bool CheckCollision(GameObject colliderObject, float radius,
		Vector3 oldPos, ref Vector3 newPos, ref Vector3 velocity, GameObject ignoreObject = null)
	{
		Vector3 travelVector = newPos - oldPos;
		float distanceAttempted = travelVector.magnitude;
		
		RaycastHit hitInfo;
		int layerMask = LayerMaskThatIgnoresMe(colliderObject);
		
		if (ignoreObject != null)
		{
			int ignoreMask = LayerMaskThatIgnoresMe(ignoreObject);
			layerMask &= ignoreMask;
		}
		
		int sanity = 1000;
		bool hitAnything = false;
		while (distanceAttempted > 0.0f)
		{
			if (Physics.SphereCast(oldPos, radius, travelVector.normalized, out hitInfo, distanceAttempted, layerMask))
			{
				float distanceTraveled = hitInfo.distance;
				Vector3 collisionPoint = oldPos + travelVector.normalized * distanceTraveled;
				velocity = Vector3.Reflect(velocity, hitInfo.normal);
				
				//reset & try again
				oldPos = collisionPoint;
				distanceAttempted -= distanceTraveled;
				if (distanceAttempted > 0.0f)
				{
					newPos = collisionPoint + velocity.normalized * distanceAttempted;
				}
				travelVector = velocity * distanceAttempted;
				hitAnything = true;
			}
			else
			{
				distanceAttempted = 0.0f;
			}
			
			if (sanity-- < 0)
			{
				Debug.Log("CheckCollision() SANITY CHECK FAILED!");
				break;
			}
		}
		
		return hitAnything;
	}
}
