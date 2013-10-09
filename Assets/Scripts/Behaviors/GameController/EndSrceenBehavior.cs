using UnityEngine;
using System.Collections;

public class EndSrceenBehavior : MonoBehaviour 
{
    public bool didPlayerWin;
    private GUIStyle gUIStyle;
    // Use this for initialization
    void Start()
    {
        gUIStyle = new GUIStyle();
        gUIStyle.fontSize = 10;
        gUIStyle.normal.textColor = Color.white;
    }

    void OnGUI() 
    {
        if (didPlayerWin)
        {
            GUI.Label(new Rect((Screen.width / 2) - 200, Screen.height - 280, 300, 60), "The Player Won!");
        }
        else
        {
            GUI.Label(new Rect((Screen.width / 2) - 200, Screen.height - 280, 300, 60), "The Player Loss!");
        }
    }

	// Update is called once per frame
    //void Update () {
	
    //}
}
