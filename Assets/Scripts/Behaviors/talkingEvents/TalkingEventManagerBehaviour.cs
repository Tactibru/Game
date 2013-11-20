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

    public TalkerBehaviour prehabTalker;
    public TypeWriterBoxBehaviour prehabTextBox;
    
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
        float xOfRatio = 6.0f * ((float)Screen.width / (float)Screen.height);
        float xPositionOfTalker = xOfRatio * 0.1f;
        float xScaleOfTalker = xOfRatio * 0.2f;
        float xScaleOfTextbox = xOfRatio * 0.60f;

        textBox[0] = (TypeWriterBoxBehaviour)Instantiate(prehabTextBox, new Vector3(0.0f, -2.0f, 1.0f), Quaternion.identity);
        textBox[0].transform.parent = transform;
        textBox[0].transform.localPosition = new Vector3(0.0f, -2.0f, 1.0f);
        textBox[0].transform.localScale = new Vector3(xScaleOfTextbox, textBox[0].transform.localScale.y, textBox[0].transform.localScale.z); 
        textBox[0].posOfBox = TypeWriterBoxBehaviour.PositionOfTheTextBox.bottom;

        textBox[1] = (TypeWriterBoxBehaviour)Instantiate(prehabTextBox, new Vector3(0.0f, 2.0f, 1.0f), Quaternion.identity);
        textBox[1].transform.parent = transform;
        textBox[1].transform.localPosition = new Vector3(0.0f, 2.0f, 1.0f);
        textBox[1].transform.localScale = new Vector3(xScaleOfTextbox, textBox[1].transform.localScale.y, textBox[1].transform.localScale.z); 
        textBox[1].posOfBox = TypeWriterBoxBehaviour.PositionOfTheTextBox.top;
        
        //float xOfRatio = 6 * (Screen.width / Screen.height);
        //float xPositionOfTalker = xOfRatio * .075f;
        //float xScaleOfTalker = xOfRatio * 0.2f;
        //Debug.Log(xScaleOfTalker.ToString());

        talkers[0] = (TalkerBehaviour)Instantiate(prehabTalker, new Vector3(-5.0f, -1.0f, 1.0f), Quaternion.identity);
        talkers[0].transform.parent = transform;
        talkers[0].transform.localPosition = new Vector3(-((xOfRatio / 2) -  xPositionOfTalker), -1.0f, 1.0f);
        talkers[0].transform.localScale = new Vector3(xScaleOfTalker, talkers[0].transform.localScale.y, talkers[0].transform.localScale.z);
        talkers[1] = (TalkerBehaviour)Instantiate(prehabTalker, new Vector3(3.785f, -1.0f, 1.0f), Quaternion.identity);
        talkers[1].transform.parent = transform;
        talkers[1].transform.localPosition = new Vector3(((xOfRatio / 2) - xPositionOfTalker), -1.0f, 1.0f);
        talkers[1].transform.localScale = new Vector3(xScaleOfTalker, talkers[1].transform.localScale.y, talkers[1].transform.localScale.z);
        talkers[2] = (TalkerBehaviour)Instantiate(prehabTalker, new Vector3(-3.785f, 1.0f, 1.0f), Quaternion.identity);
        talkers[2].transform.parent = transform;
        talkers[2].transform.localPosition = new Vector3(-((xOfRatio / 2) - xPositionOfTalker), 1.0f, 1.0f);
        talkers[2].transform.localScale = new Vector3(xScaleOfTalker, talkers[2].transform.localScale.y, talkers[2].transform.localScale.z);
        talkers[3] = (TalkerBehaviour)Instantiate(prehabTalker, new Vector3(3.785f, 1.0f, 1.0f), Quaternion.identity);
        talkers[3].transform.parent = transform;
        talkers[3].transform.localPosition = new Vector3(((xOfRatio / 2) - xPositionOfTalker), 1.0f, 1.0f);
        talkers[3].transform.localScale = new Vector3(xScaleOfTalker, talkers[3].transform.localScale.y, talkers[3].transform.localScale.z);

        //Everything below this point is only for testing purposes.

        TalkingEventChain newTalkingEventChain = new TalkingEventChain();

        TalkingEvent newTalkingEvent0 = new TalkingEvent();
        newTalkingEvent0.name = "Hoshi Hoshi Hoshi";
        newTalkingEvent0.normalText = "Hoshi is willing to make deal with the most dishonorable Hiro. Leave now and Hoshi Hoshi Hoshi shall forgive you transgress..transg...transg... Hoshi will let you live! Hoshi believes this is the kindest deal Hoshi could ever possibly offer.";
        //newTalkingEvent0.annoyText = "Yup It works.";
        newTalkingEvent0.selectedPanel = Panels.LowerLeft;
        newTalkingEvent0.theTalker = testActorLeft;
        newTalkingEventChain.talkingEvents.Add(newTalkingEvent0);

        TalkingEvent newTalkingEvent1 = new TalkingEvent();
        newTalkingEvent1.name = "Hiro";
        newTalkingEvent1.normalText = "No I do believe I am going to deal with you now and take what few things you have left.";
        //newTalkingEvent1.annoyText = "Up Top.";
        newTalkingEvent1.selectedPanel = Panels.LowerRight;
        newTalkingEvent1.theTalker = testActorRight;
        newTalkingEventChain.talkingEvents.Add(newTalkingEvent1);

        TalkingEvent newTalkingEvent2 = new TalkingEvent();
        newTalkingEvent2.name = "Hoshi Hoshi Hoshi";
        newTalkingEvent2.normalText = "Nonsence, you have taken nothing from the great Hoshi Hoshi Hoshi, son of Hoshi Yoshi Toshi, descenda-";
        newTalkingEvent2.selectedPanel = Panels.LowerLeft;
        newTalkingEvent2.theTalker = testActorLeft;
        newTalkingEventChain.talkingEvents.Add(newTalkingEvent2);

        TalkingEvent newTalkingEvent3 = new TalkingEvent();
        newTalkingEvent3.name = "Hiro";
        newTalkingEvent3.normalText = "I have taken 2/3rds of your land.";
        newTalkingEvent3.selectedPanel = Panels.LowerRight;
        newTalkingEvent3.theTalker = testActorRight;
        newTalkingEventChain.talkingEvents.Add(newTalkingEvent3);

        TalkingEvent newTalkingEvent4 = new TalkingEvent();
        newTalkingEvent4.name = "Hoshi Hoshi Hoshi";
        newTalkingEvent4.normalText = "................";
        newTalkingEvent4.selectedPanel = Panels.LowerLeft;
        newTalkingEvent4.theTalker = testActorLeft;
        newTalkingEventChain.talkingEvents.Add(newTalkingEvent4);

        TalkingEvent newTalkingEvent5 = new TalkingEvent();
        newTalkingEvent5.name = "Hiro";
        newTalkingEvent5.normalText = "...............";
        newTalkingEvent5.selectedPanel = Panels.LowerRight;
        newTalkingEvent5.theTalker = testActorRight;
        newTalkingEventChain.talkingEvents.Add(newTalkingEvent5);

        TalkingEvent newTalkingEvent6 = new TalkingEvent();
        newTalkingEvent6.name = "Hoshi Hoshi Hoshi";
        newTalkingEvent6.normalText = "You're making that up.";
        newTalkingEvent6.selectedPanel = Panels.LowerLeft;
        newTalkingEvent6.theTalker = testActorLeft;
        newTalkingEventChain.talkingEvents.Add(newTalkingEvent6);

        TalkingEvent newTalkingEvent7 = new TalkingEvent();
        newTalkingEvent7.name = "Hiro";
        newTalkingEvent7.normalText = "Wait I'm wha-";
        newTalkingEvent7.selectedPanel = Panels.LowerRight;
        newTalkingEvent7.theTalker = testActorRight;
        newTalkingEventChain.talkingEvents.Add(newTalkingEvent7);

        TalkingEvent newTalkingEvent8 = new TalkingEvent();
        newTalkingEvent8.name = "Hoshi Hoshi Hoshi";
        newTalkingEvent8.normalText = "Honorable lackeys, defend Hoshi Hoshi Hoshi with your lives!";
        newTalkingEvent8.selectedPanel = Panels.LowerLeft;
        newTalkingEvent8.theTalker = testActorLeft;
        newTalkingEventChain.talkingEvents.Add(newTalkingEvent8);

        talkingEventChain.Add(newTalkingEventChain);

        StarTalkingEventChain(0);

        //float xOfRatio = 6 * (Screen.width / Screen.height);
        //float xPositionOfTalker = xOfRatio * .10f;
        Debug.Log(xOfRatio.ToString());
        Debug.Log(Screen.width.ToString());
        Debug.Log(Screen.height.ToString());
        //Debug.Log(xPositionOfTalker);
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
            currentTalkingEventChain[index].name = talkingEventChain[numberOfEvent].talkingEvents[index].name;
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
