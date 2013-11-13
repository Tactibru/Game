using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is the overall manager, for the typewriters.
/// 
/// Alex Reiss
/// </summary>
[AddComponentMenu("Tactibru/Dialog/Talking Event Manager")]
public class TalkingEventManagerBehaviour : MonoBehaviour 
{
    /// <summary>
    /// This is used for the switch.
    /// 
    /// Alex Reiss
    /// </summary>
    public enum Panels
    {
        LowerLeft,
        LowerRight,
        UpperLeft,
        UpperRight,
        None,
        NUM_PANELS
    }
    
    /// <summary>
    /// To keep track of the four talking panels. 
    /// </summary>
  
    public TalkerBehaviour[] talkers;

    /// <summary>
    /// To keep track of the two text boxes.
    /// </summary>

    public TypeWriterBoxBehaviour[] textBox;

    /// <summary>
    /// The variable to control the switch.
    /// </summary>
    
    Panels currentPanel = Panels.None;

    /// <summary>
    /// Acts as the status of all panels.
    /// </summary>

    bool activePanels = false;

    /// <summary>
    /// This is the boolean that the textboxes control, the reason it is here is to 
    /// stay static effectively without being static. 
    /// </summary>

    public bool playingEvent = false;

    /// <summary>
    /// This is the main control vairable, if is zero then the current talking event chain starts.
    /// It is also the index of the current current talking event, thats is playing.
    /// When this equals the count of the current talking event chain, the chain stops.
    /// </summary>

    int currentTalkingEvent = 0;

    /// <summary>
    /// This is the data structure that holds all talking events, of the current level.
    /// </summary>

    public List<TalkingEventChain> talkingEventChain = new List<TalkingEventChain>();

    /// <summary>
    /// The currently running talking event chain.
    /// </summary>

    public List<TalkingEvent> currentTalkingEventChain = new List<TalkingEvent>();

    /// <summary>
    /// These are for testing purposes only.
    /// </summary>
    
    public Material testActorLeft;
    public Material testActorRight;

	/// <summary>
	/// This is to tell the textboxes where they are.
	/// </summary>

	void Start () 
    {
        //Vector3 screenPosition = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width, Screen.height, 6.0f));
        //talkers[0].transform.localPosition = screenPosition - Camera.main.transform.forward;
        //talkers[0].transform.position = screenPosition;

        textBox[0].posOfBox = TypeWriterBoxBehaviour.PositionOfTheTextBox.bottom;
        textBox[1].posOfBox = TypeWriterBoxBehaviour.PositionOfTheTextBox.top;

        //Everything below this point is only for testing purposes.

        //TalkingEventChain newTalkingEventChain = new TalkingEventChain();

        //TalkingEvent newTalkingEvent0 = new TalkingEvent();
        //newTalkingEvent0.normalText = "Testing Testing Testing Testing Testing Testing 456 Testing Testing Testing 789 Testing Testing.";
        //newTalkingEvent0.annoyText = "Yup It works.";
        //newTalkingEvent0.selectedPanel = Panels.LowerLeft;
        //newTalkingEvent0.theTalker = testActorLeft;
        //newTalkingEventChain.talkingEvents.Add(newTalkingEvent0);

        //TalkingEvent newTalkingEvent1 = new TalkingEvent();
        //newTalkingEvent1.normalText = "Testing Testing Testing 123 Testing 456 Testing Testing Testing 789 Testing Testing.";
        ////newTalkingEvent1.annoyText = "Up Top.";
        //newTalkingEvent1.selectedPanel = Panels.UpperRight;
        //newTalkingEvent1.theTalker = testActorRight;
        //newTalkingEventChain.talkingEvents.Add(newTalkingEvent1);

        //talkingEventChain.Add(newTalkingEventChain);

        //StarTalkingEventChain(0);
    }
	
	/// <summary>
	/// This is for test at the moment, acting as the test trigger.
    /// 
    /// Alex Reiss
	/// </summary>

	void Update () 
    {
        if (currentTalkingEvent < currentTalkingEventChain.Count || activePanels)
        {
            
            if (!activePanels && !playingEvent)
            {
                //Debug.Log("Hi");
                if (currentTalkingEventChain[currentTalkingEvent].selectedPanel == Panels.UpperLeft)
                {
                    textBox[1].transform.renderer.enabled = true;
                    talkers[2].transform.renderer.enabled = true;
                    activePanels = true;
                    currentPanel = Panels.UpperLeft;
                    textBox[1].startTalkingEvent(currentTalkingEventChain[currentTalkingEvent].normalText, currentTalkingEventChain[currentTalkingEvent].annoyText, currentTalkingEventChain[currentTalkingEvent].name);
                    talkers[2].SetTalker(currentTalkingEventChain[currentTalkingEvent].theTalker);
                }
                else if (currentTalkingEventChain[currentTalkingEvent].selectedPanel == Panels.UpperRight)
                {
                    textBox[1].transform.renderer.enabled = true;
                    talkers[3].transform.renderer.enabled = true;
                    activePanels = true;
                    currentPanel = Panels.UpperRight;
                    textBox[1].startTalkingEvent(currentTalkingEventChain[currentTalkingEvent].normalText, currentTalkingEventChain[currentTalkingEvent].annoyText, currentTalkingEventChain[currentTalkingEvent].name);
                    talkers[3].SetTalker(currentTalkingEventChain[currentTalkingEvent].theTalker);
                }
                else if (currentTalkingEventChain[currentTalkingEvent].selectedPanel == Panels.LowerLeft)
                {
                    
                    textBox[0].transform.renderer.enabled = true;
                    talkers[0].transform.renderer.enabled = true;
                    activePanels = true;
                    currentPanel = Panels.LowerLeft;
                    textBox[0].startTalkingEvent(currentTalkingEventChain[currentTalkingEvent].normalText, currentTalkingEventChain[currentTalkingEvent].annoyText, currentTalkingEventChain[currentTalkingEvent].name);
                    talkers[0].SetTalker(currentTalkingEventChain[currentTalkingEvent].theTalker);
                }
                else if (currentTalkingEventChain[currentTalkingEvent].selectedPanel == Panels.LowerRight)
                {
                    textBox[0].transform.renderer.enabled = true;
                    talkers[1].transform.renderer.enabled = true;
                    activePanels = true;
                    currentPanel = Panels.LowerRight;
                    textBox[0].startTalkingEvent(currentTalkingEventChain[currentTalkingEvent].normalText, currentTalkingEventChain[currentTalkingEvent].annoyText, currentTalkingEventChain[currentTalkingEvent].name);
                    talkers[1].SetTalker(currentTalkingEventChain[currentTalkingEvent].theTalker);
                }
                currentTalkingEvent++;
            }
            else if (!playingEvent)
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

    /// <summary>
    /// This starts the selected talking event. It also copies the talking events into the currently playing events and resets the main control vairable.
    /// 
    /// Alex Reiss
    /// </summary>
    /// <param name="numberOfEvent">This is the index of the selected event.</param>

    public void StarTalkingEventChain(int numberOfEvent)
    {
        currentTalkingEventChain.Clear();
        currentTalkingEvent = 0;

        //I have both the shallow copy and deep copy, because I am not sure which one I need.
        currentTalkingEventChain = talkingEventChain[numberOfEvent].talkingEvents;

        for (int index = 0; index < talkingEventChain[numberOfEvent].talkingEvents.Count; index++)
        {
            currentTalkingEventChain[index].annoyText = talkingEventChain[numberOfEvent].talkingEvents[index].annoyText;
            currentTalkingEventChain[index].normalText = talkingEventChain[numberOfEvent].talkingEvents[index].normalText;
            currentTalkingEventChain[index].selectedPanel = talkingEventChain[numberOfEvent].talkingEvents[index].selectedPanel;
            currentTalkingEventChain[index].theTalker = talkingEventChain[numberOfEvent].talkingEvents[index].theTalker;
        }
    }
}

/// <summary>
/// To handle multiple talking sessions in the level.
/// </summary>

public class TalkingEventChain
{
    public List<TalkingEvent> talkingEvents = new List<TalkingEvent>();
}

/// <summary>
/// Temporary but could be used for the test event but could be used for more.
/// </summary>

public class TalkingEvent
{
    public string name = "";
    public string normalText = "";
    public string annoyText= "";
    public TalkingEventManagerBehaviour.Panels selectedPanel;
    public Material theTalker;
}
