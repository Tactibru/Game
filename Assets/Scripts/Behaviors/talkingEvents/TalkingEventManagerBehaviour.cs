using UnityEngine;
using System.Collections;

public class TalkingEventManagerBehaviour : MonoBehaviour 
{
    public enum Panels
    {
        LowerLeft,
        LowerRight,
        UpperLeft,
        UpperRight,
        None,
        NUM_PANELS
    }
    
    public TalkerBehaviour[] talkers;
    public TypeWriterBoxBehaviour[] textBox;
    Panels currentPanel = Panels.None;
    bool activePanels = false;
    public bool playingEvent = false;
    int currentTalkingEvent = 0;

    //Test vairables
    public TalkingEvent[] talkingEvents = new TalkingEvent[4];  

	// Use this for initialization
	void Start () 
    {
        TalkingEvent newTalkingEvent = new TalkingEvent();
        newTalkingEvent.normalText = "Lower Left.";
        newTalkingEvent.selectedPanel = Panels.LowerLeft;
        talkingEvents[0] = newTalkingEvent;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (!activePanels && !playingEvent && Input.GetKeyDown(KeyCode.Space))
        {
            if (talkingEvents[currentTalkingEvent].selectedPanel == Panels.UpperLeft)
            {
                textBox[1].transform.renderer.enabled = true;
                talkers[2].transform.renderer.enabled = true;
                activePanels = true;
                currentPanel = Panels.UpperLeft;
            }
            else if (talkingEvents[currentTalkingEvent].selectedPanel == Panels.UpperRight)
            {
                textBox[1].transform.renderer.enabled = true;
                talkers[3].transform.renderer.enabled = true;
                activePanels = true;
                currentPanel = Panels.UpperRight;
            }
            else if (talkingEvents[currentTalkingEvent].selectedPanel == Panels.LowerLeft)
            {
                textBox[0].transform.renderer.enabled = true;
                talkers[0].transform.renderer.enabled = true;
                activePanels = true;
                currentPanel = Panels.LowerLeft;
            }
            else if (talkingEvents[currentTalkingEvent].selectedPanel == Panels.LowerRight)
            {
                textBox[0].transform.renderer.enabled = true;
                talkers[1].transform.renderer.enabled = true;
                activePanels = true;
                currentPanel = Panels.LowerRight;
            }
        }
        else if(!playingEvent)
        {
            switch (currentPanel)
            {
                case Panels.LowerLeft:
                    textBox[0].transform.renderer.enabled = false;
                    talkers[0].transform.renderer.enabled = false;
                    activePanels = false;
                    currentPanel = Panels.None;
                    break;
                case Panels.LowerRight:
                    textBox[0].transform.renderer.enabled = false;
                    talkers[1].transform.renderer.enabled = false;
                    activePanels = false;
                    currentPanel = Panels.None;
                    break;
                case Panels.UpperRight:
                    textBox[1].transform.renderer.enabled = false;
                    talkers[3].transform.renderer.enabled = false;
                    activePanels = false;
                    currentPanel = Panels.None;
                    break;
                case Panels.UpperLeft:
                    textBox[1].transform.renderer.enabled = false;
                    talkers[2].transform.renderer.enabled = false;
                    activePanels = false;
                    currentPanel = Panels.None;
                    break;
            }
        }
	}
}

public class TalkingEvent
{
    public string normalText;
    public string annoyText;
    public TalkingEventManagerBehaviour.Panels selectedPanel;
}
