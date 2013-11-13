using UnityEngine;
using System.Collections;

public class MiniMapOverlay : MonoBehaviour 
{
    public Rect minimap = new Rect(0,0,300, 225);
    public Texture playerTexture;

    private GameControllerBehaviour gameController;
    private float x;
    private float z;
    
	// Use this for initialization
	void Start () 
    {
        gameController = GameObject.FindGameObjectWithTag("Grid").GetComponent<GameControllerBehaviour>();
        
	
	}
	
	// Update is called once per frame
	void Update () 
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
                float x = squad.transform.localPosition.x;
                float z = squad.transform.localPosition.z;
            }

        }
        else
        {
            foreach (ActorBehavior squad in gameController.playerTeam)
            {
                float x = squad.transform.localPosition.x;
                float z = squad.transform.localPosition.z;
            }

        }
    }

    public void MiniMapHUD(int windowID)
    {
        GUI.Button(new Rect(minimap.width /2, minimap.height /2, 100, 20), "Im a person!");
        Debug.Log(x + "  is the x coord" + z + "  is the z coord");
        GUI.DrawTexture(new Rect(x, z, 8, 8), playerTexture, ScaleMode.ScaleToFit);
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }


}
