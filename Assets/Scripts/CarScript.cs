using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarScript : MonoBehaviour 
{

    List<GameObject> AIAgents = new List<GameObject>();

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
	
	}

    
}
