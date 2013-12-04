using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[AddComponentMenu("Tactibru/Level Components/Game Controller")]
[RequireComponent(typeof(GridBehavior))]
public class GameControllerBehaviour : MonoBehaviour 
{
    public List<ActorBehavior> playerTeam = new List<ActorBehavior>();
    public List<ActorBehavior> enemyTeam = new List<ActorBehavior>();
    public List<ActorBehavior> nuetrals = new List<ActorBehavior>();
    public int playerTeamTotal;
    public int enemyTeamTotal;
    public int nuetralTotal;
    public int leftToMoveThis;
    private GUIStyle gUIStyle;
	public int numberOfTurns = 1;

	/// <summary>
	/// Allows the player to control enemy units.
	/// </summary>
	public bool AllowPlayerControlledEnemies = false;

    public enum UnitSide
    {
        player,
        enemy, 
        nuetral,
        NUMBER_OF_SIDES
    }

    public GameControllerBehaviour.UnitSide currentTurn = UnitSide.player;
	public HUDController controller;

	/// <summary>
	/// Sets the sides up, the end condition up, and the turn counter.
    /// 
    /// Alex Reiss
	/// </summary>
	void Start () 
    {
        for (int index = 0; index < playerTeam.Count; index++)
            playerTeam[index].theSide = GameControllerBehaviour.UnitSide.player;

        for (int index = 0; index < enemyTeam.Count; index++)
            enemyTeam[index].theSide = GameControllerBehaviour.UnitSide.enemy;

        for (int index = 0; index < nuetrals.Count; index++)
            nuetrals[index].theSide = GameControllerBehaviour.UnitSide.nuetral;

        playerTeamTotal = playerTeam.Count;
        enemyTeamTotal = enemyTeam.Count;
        nuetralTotal = nuetrals.Count;
        leftToMoveThis = playerTeamTotal;

        gUIStyle = new GUIStyle();
        gUIStyle.fontSize = 10;
        gUIStyle.normal.textColor = Color.white;
		
		controller = Camera.main.GetComponent<HUDController>();
		
			controller.whoseTurn.text = "Players Turn";
			controller.turnCount.text = "Turn " + numberOfTurns.ToString ();
		

		TalkingEventManagerBehaviour talkingManager = Camera.main.GetComponent<TalkingEventManagerBehaviour> ();
		if(talkingManager != null)
			talkingManager.StartTalkingEventChain(SceneConversationBehavior.instance.introConversation);
	}

	/// <summary>
	/// Iterates over the list and removes dead (null) squads.
	/// </summary>
	/// <param name="list"></param>
	/// <param name="teamTotal"></param>
	void RemoveDeadSquads(List<ActorBehavior> list, ref int teamTotal)
	{
		for (int _i = 0; _i < list.Count; _i++)
		{
			if (list[_i] == null)
			{
				list.RemoveAt(_i);
				teamTotal--;
			}
		}
	}

	/// <summary>
	/// Iterates over the Player & Enemy nodes to determine if the specified node is occupied.
	/// </summary>
	/// <param name="node"></param>
	/// <returns></returns>
	public ActorBehavior GetActorOnNode(MovePointBehavior node)
	{
		foreach (ActorBehavior squad in playerTeam)
			if (squad.currentMovePoint == node)
				return squad;

		foreach (ActorBehavior squad in enemyTeam)
			if (squad.currentMovePoint == node)
				return squad;

		foreach (ActorBehavior squad in nuetrals)
			if (squad.currentMovePoint == node)
				return squad;

		return null;
	}

    /// <summary>
    /// Checks end game conditions, and end turn conditions.
    /// 
    /// Alex Reiss
    /// </summary>
    void Update()
    {
		// Remove dead units.
		RemoveDeadSquads(playerTeam, ref playerTeamTotal);
		RemoveDeadSquads(enemyTeam, ref enemyTeamTotal);
		RemoveDeadSquads(nuetrals, ref nuetralTotal);

        EndGame();

        if (/*Input.GetKeyDown(KeyCode.Space) || */leftToMoveThis == 0)
            EndTurn();
    }

    public void EndGame()
    {
        if (enemyTeamTotal == 0)
        {
			Camera.main.GetComponent<TalkingEventManagerBehaviour>().StartTalkingEventChain(SceneConversationBehavior.instance.victoryConversation);
            Application.LoadLevel("PlayerWins");
        }

        if (playerTeamTotal == 0)
        {
			Camera.main.GetComponent<TalkingEventManagerBehaviour>().StartTalkingEventChain(SceneConversationBehavior.instance.defeatConversation);
            Application.LoadLevel("PlayerLosses");
        }
    }

    public void EndTurn()
    {
        for (int index = 0; index < playerTeam.Count; index++)
            playerTeam[index].actorHasMovedThisTurn = false;

        for (int index = 0; index < enemyTeam.Count; index++)
            enemyTeam[index].actorHasMovedThisTurn = false;

        for (int index = 0; index < nuetrals.Count; index++)
            nuetrals[index].actorHasMovedThisTurn = false;

        if (currentTurn == UnitSide.player)
        {
			GridControlBehavior gridControl = GetComponent<GridControlBehavior>();
			if (gridControl != null)
				gridControl.EndTurn();

            currentTurn = UnitSide.enemy;
            leftToMoveThis = enemyTeamTotal;
			controller.whoseTurn.text = "Enemy Turn";
        }
        else
        {
            currentTurn = UnitSide.player;
            leftToMoveThis = playerTeamTotal;
			controller.whoseTurn.text = "Player Turn";
			numberOfTurns++;
			controller.turnCount.text = "Turn " + numberOfTurns.ToString();

			if(SceneConversationBehavior.instance != null)
				if (SceneConversationBehavior.instance.battleQuips.Length >= numberOfTurns)
					Camera.main.GetComponent<TalkingEventManagerBehaviour>().StartTalkingEventChain(SceneConversationBehavior.instance.battleQuips[numberOfTurns - 1]);
        }
    }
}
