using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour 
{

    public string startAddress = "Start Address";
    public string endAddress = "End Address";
    private GUIStyle gUIStyle;
    // Use this for initialization
    void Start()
    {
        gUIStyle = new GUIStyle();
        gUIStyle.fontSize = 10;
        gUIStyle.normal.textColor = Color.white;
    }
	
    //// Update is called once per frame
    //void Update () 
    //{
	
    //}

    void OnGUI()
    {
        GUI.Label(new Rect((Screen.width / 2) - 200, Screen.height - 280, 300, 60), "Enter Start Address.");
        GUI.Label(new Rect((Screen.width / 2) - 150, Screen.height - 300, 300, 60), "Welcome to Zombie Map.");
        GUI.Label(new Rect((Screen.width / 2) - 200, Screen.height - 240, 300, 60), "Enter End Address.");

        startAddress = GUI.TextField(new Rect((Screen.width / 2) - 100, Screen.height - 260, 300, 30), startAddress);
        endAddress = GUI.TextField(new Rect((Screen.width / 2) - 100, Screen.height - 220, 300, 30), endAddress);

        if (GUI.Button(new Rect(10, 10, 50, 50), "Enter", gUIStyle))
        {
            
        }
    }
}
