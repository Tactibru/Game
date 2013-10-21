using UnityEngine;
using System.Collections;

public class TypeWriterBoxBehaviour : MonoBehaviour {

    bool currentlyPlaying = false;
    string normalText = "";
    string annoyText = "";
    string currntlyDisplayedText = "";
    public TalkingEventManagerBehaviour talkingManager;
	// Use this for initialization
	void Start () 
    {
        talkingManager = Camera.main.transform.GetComponent<TalkingEventManagerBehaviour>();
        transform.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (transform.renderer.enabled)
        //        transform.renderer.enabled = false;
        //    else
        //        transform.renderer.enabled = true;
        //}
	}

    public void startTalkingEvent(string newNormalText, string newAnnoyText)
    {
        normalText = newNormalText;
        annoyText = newAnnoyText;
        currentlyPlaying = true;
    }

    void OnGUI()
    {
        if (currentlyPlaying)
        {
            GUI.Label(new Rect(100, 10, 100, 20), normalText);
        }
    }
}
