using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControllerBehaviour : MonoBehaviour 
{
    public List<ActorBehavior> playerTeam = new List<ActorBehavior>();
    public List<ActorBehavior> enemyTeam = new List<ActorBehavior>();
    public List<ActorBehavior> nuetrals = new List<ActorBehavior>();
    public int playerTeamTotal;
    public int enemyTeamTotal;
    public int nuetralTotal;

	// Use this for initialization
	void Start () 
    {
        for (int index = 0; index < playerTeam.Count; index++)
            playerTeam[index].side = 0;

        for (int index = 0; index < enemyTeam.Count; index++)
            enemyTeam[index].side = 1;

        for (int index = 0; index < nuetrals.Count; index++)
            nuetrals[index].side = 2;

        playerTeamTotal = playerTeam.Count;
        enemyTeamTotal = enemyTeam.Count;
        nuetralTotal = nuetrals.Count;
	}

    // Update is called once per frame
    void Update()
    {
        if (enemyTeam.Count == 0)
        {
            Application.LoadLevel("PlayerWins");
        }

        if (playerTeam.Count == 0)
        {
            Application.LoadLevel("PlayerLosses");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int index = 0; index < playerTeam.Count; index++)
                playerTeam[index].actorHasMovedThisTurn = false;

            for (int index = 0; index < enemyTeam.Count; index++)
                enemyTeam[index].actorHasMovedThisTurn = false;

            for (int index = 0; index < nuetrals.Count; index++)
                nuetrals[index].actorHasMovedThisTurn = false;
        }
	}
}
