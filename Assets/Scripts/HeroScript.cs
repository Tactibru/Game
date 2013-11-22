using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroScript : MonoBehaviour 
{
	public float moveSpeed = 5.0f;
	public float turnRate = 180.0f;
	
	public bool gameOver = false;
	public GAMESTATE currentState = GAMESTATE.PLAYING;
    

    List<GameObject> AIAgents = new List<GameObject>();
	
	public enum GAMESTATE
	{
		PLAYING,
		GAMEOVER,
		VICTORY
	};
	
	// Use this for initialization
	void Start () 
	{
        GameObject[] AI = GameObject.FindGameObjectsWithTag("EnemyAI");
        foreach (GameObject agent in AI)
        {
             AIAgents.Add(agent);
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (gameOver)
		{
			return;
		}

        if (Input.GetMouseButtonDown(0))
        {
            
            RaycastHit hit;
            Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                Vector3 hitPoint = hit.point;
                Vector3 theVectorFromPlayerPositionToMousePoint = transform.position - hitPoint;
                Vector3 newForward = new Vector3(0.0f, Mathf.Rad2Deg * Mathf.Atan(theVectorFromPlayerPositionToMousePoint.z / theVectorFromPlayerPositionToMousePoint.x), 0.0f);
                transform.rotation = Quaternion.Euler(newForward);
                Debug.Log(hitPoint);
                Debug.Log(newForward);
            }


        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            Vector3 currentOrientation = transform.rotation.eulerAngles;
            currentOrientation.y += turnRate * Time.deltaTime;

            transform.rotation = Quaternion.Euler(currentOrientation);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            Vector3 currentOrientation = transform.rotation.eulerAngles;
            currentOrientation.y -= turnRate * Time.deltaTime;

            transform.rotation = Quaternion.Euler(currentOrientation);
        }
		
	}

    void OnTriggerEnter(Collider theOneThatCollided)
    {
          
        if (theOneThatCollided.tag == "Car")
        {
            
            foreach (GameObject agent in AIAgents)
            {
                EnemyScript currentEnemy = agent.GetComponent<EnemyScript>();
                currentEnemy.ChangetoState(EnemyScript.ENEMY_STATES.FLEE);
                
            }
        }
    }

    void OnTriggerExit(Collider theOneThatCollided)
    {
        if (theOneThatCollided.tag == "Car")
        {
            foreach (GameObject agent in AIAgents)
            {
                EnemyScript currentEnemy = agent.GetComponent<EnemyScript>();
                currentEnemy.ChangetoState(EnemyScript.ENEMY_STATES.IDLE);
            }
        }
    }
}
