using UnityEngine;
using System.Collections;

public class MiniMapOverlay : MonoBehaviour
{
    public Rect minimap = new Rect(0, 0, 300, 225);
    public Texture playerTexture;
    public Texture enemyTexture;

    private GameControllerBehaviour gameController;
    private float enemyX;
    private float enemyZ;
    private float playerX;
    private float playerZ;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Grid").GetComponent<GameControllerBehaviour>();


    }

    // Update is called once per frame
    void Update()
    {
        UpdateMiniMap();
    }

    public void OnGUI()
    {
        minimap = GUI.Window(0, minimap, MiniMapHUD, "My Minimap");
    }


    void UpdateMiniMap()
    {
        if (gameController.currentTurn == GameControllerBehaviour.UnitSide.enemy)
        {
            foreach (ActorBehavior squad in gameController.enemyTeam)
            {
                float enemyX = squad.transform.position.x;
                //Debug.Log("Adding Enemy X and Z "); WORKS
                float enemyZ = squad.transform.position.z;
            }

        }
        else
        {
            foreach (ActorBehavior squad in gameController.playerTeam)
            {
                float playerX = squad.transform.position.x;
                float playerZ = squad.transform.position.z;
                //Debug.Log("Adding Player X and Z " + playerX + " --- " + playerZ);
            }

        }
    }

    public void MiniMapHUD(int windowID)
    {
        GUI.Button(new Rect(minimap.width / 2, minimap.height / 2, 100, 20), "Im a person!");
        //Debug.Log(playerX + "  is the x coord-player " + playerZ + "  is the y coord-player ");
        //Debug.LogError(enemyX + "  is the x coord-enemy " + enemyZ + "  is the y coord-enemy ");
        GUI.DrawTexture(new Rect(playerX, playerZ, 8, 8), playerTexture, ScaleMode.ScaleToFit);
        GUI.DrawTexture(new Rect(enemyX, enemyZ, 16, 16), enemyTexture, ScaleMode.ScaleToFit);
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }


}

