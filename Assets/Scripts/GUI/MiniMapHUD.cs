using UnityEngine;
using System.Collections;




public class MiniMapHUD : MonoBehaviour 
{
    public Rect minimap = new Rect(Screen.width / 2 - 400, Screen.height / 2 - 400, 200, 200);


	// Use this for initialization
	void Start () 
    {
	
	}

    public void OnGUI()
    {
        minimap = GUI.Window(0, minimap, MiniMapOverlay, "My Minimap");

    }

    public void MiniMapOverlay(int windowID)
    {
        


    }

	// Update is called once per frame
	void Update () 
    {
	
	}
}
