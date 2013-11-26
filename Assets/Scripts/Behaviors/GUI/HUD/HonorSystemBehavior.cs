using UnityEngine;
using System.Collections;

[AddComponentMenu("Tactibru/GUI/Honor System")]
public class HonorSystemBehavior : MonoBehaviour
{
    public int playerHonorTotal;
    public int computerHonorTotal;
    public bool usingGameController;
    public GUIStyle style;
    public GameControllerBehaviour gameController;
    public HUDController controller;
    public static int offensiveHonor;
    public static int defensiveHonor;
    public static bool inCombat = false;
    int playerSquadTotal;
    int computerSquadTotal;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Grid").GetComponent<GameControllerBehaviour>();
        playerSquadTotal = 10;
        computerSquadTotal = 10;
    }

    void OnGUI()
    {
        BaseHonor();
        SquadHonor();
    }

    public void BaseHonor()
    {
        if (GridBehavior.inCombat == false)
        {
            if (usingGameController)
            {
                //USING HUD CONTROLLER 
            }
            else
            {
                GUI.Box(new Rect(Screen.width * 0.75f, Screen.height * 0.05f, 190, 100), ("Player Honor Total: " + playerHonorTotal + "\n" + "Computer Honor Total: " + computerHonorTotal), style);
            }
        }

        if (inCombat == true)
        {
            OverFiveCatch();
                        
            if (gameController.currentTurn == GameControllerBehaviour.UnitSide.player)
            {
                //player is offensive and recieves offensive honor
                //computer is defensive and receives defensive honor
                playerHonorTotal = playerHonorTotal + offensiveHonor - defensiveHonor;
                computerHonorTotal = computerHonorTotal + defensiveHonor - offensiveHonor;
                inCombat = false;
                offensiveHonor = 0;
                defensiveHonor = 0;
            }
            else
            {
                //player is defensive receives defensive honor and computer gets offensive honor
                computerHonorTotal = computerHonorTotal + offensiveHonor - defensiveHonor;
                playerHonorTotal = playerHonorTotal + defensiveHonor - offensiveHonor;
                inCombat = false;
                offensiveHonor = 0;
                defensiveHonor = 0;
            }
        }
    }

    void OverFiveCatch()
    {
        if (offensiveHonor > 5)
            offensiveHonor = offensiveHonor - 1;
        if (defensiveHonor > 5)
            defensiveHonor = defensiveHonor - 1;
    }

    void SquadHonor()
    {
        if (playerSquadTotal !=gameController.playerTeamTotal)
        {
            playerHonorTotal = playerHonorTotal - 5;
            playerSquadTotal = gameController.playerTeamTotal;
        }

        if (computerSquadTotal !=gameController.enemyTeamTotal)
        {
            playerHonorTotal = playerHonorTotal + 4;
            computerSquadTotal = gameController.enemyTeamTotal;
        }
    }
}
