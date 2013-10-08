using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControllerBehaviour : MonoBehaviour 
{
    public List<ActorBehavior> playerTeam = new List<ActorBehavior>();
    public List<ActorBehavior> enemyTeam = new List<ActorBehavior>();
    public List<ActorBehavior> nuetrals = new List<ActorBehavior>();

	// Use this for initialization
	void Start () 
    {
        for (int index = 0; index < playerTeam.Count; index++)
            playerTeam[index].side = 0;

        for (int index = 0; index < enemyTeam.Count; index++)
            enemyTeam[index].side = 1;

        for (int index = 0; index < nuetrals.Count; index++)
            nuetrals[index].side = 2;
	
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
