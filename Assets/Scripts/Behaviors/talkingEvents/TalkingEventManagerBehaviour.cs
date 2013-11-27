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

	public enum ConversationName
	{
		None,
		RicePatty,
		RicePattyQuip1,
		RicePattyQuip2,
		RicePattyVictory,
		RicePattyDefeat,
		SeaportShowdown,
		SeaportShowdownQuip1,
		SeaportShowdownQuip2,
		SeaportShowdownQuip3,
		SeaportShowdownWictory,
		SeaportShowdownDefeat,
		OpenPlains,
		OpenPlainsQuip1,
		OpenPlainsQuip2,
		OpenPlainsVictory,
		OpenPlainsDefeat,
		Example,
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

//    public List<TalkingEventChain> talkingEventChain = new List<TalkingEventChain>();
	public Dictionary<ConversationName, TalkingEventChain> talkingEventChainDictionary = new Dictionary<ConversationName, TalkingEventChain> ();

    /// <summary>
    /// The currently running talking event chain.
    /// </summary>

    public List<TalkingEvent> currentTalkingEventChain = new List<TalkingEvent>();

    /// <summary>
    /// These are for testing purposes only.
    /// </summary>
    
    public Material testActorLeft;
    public Material testActorRight;

	public ConversationName startingConversationID;

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

        
		#region Level 1
        TalkingEventChain newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("First-san",
		                                                        "Father, that crazy homeless man is out in the field shouting again.",
		                                                        Panels.LowerLeft,
		                                                        testActorLeft));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Yoshiro",
		                                                        "Keep him off of my beautiful lawn!",
		                                                        Panels.LowerLeft,
		                                                        testActorLeft));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Second-san",
		                                                        "Yes, father. I will shoo him away.",
		                                                        Panels.LowerLeft,
		                                                        testActorLeft));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Hiro",
		                                                        "Mighty Daimyo, I challenge you for rule over these great lands!",
		                                                        Panels.LowerRight,
		                                                        testActorRight));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Yoshiro",
		                                                        "You stay off-a my lawn, crazy man!",
		                                                        Panels.LowerLeft,
		                                                        testActorLeft));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Hiro",
		                                                        "Ha! So you accept?! The tale of our great battle shall be told for eternity!",
		                                                        Panels.LowerRight,
		                                                        testActorRight));

		#endregion


		//Save the conversation, and give it a name
		talkingEventChainDictionary[ConversationName.RicePatty] = newTalkingEventChain; 
      


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Karl-san",
		                                                        "This is coded poorly, sensei.",
		                                                        Panels.LowerLeft,
		                                                        testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Alex",
		                                                        "I know Karl, I know..",
		                                                        Panels.LowerLeft,
		                                                        testActorLeft));


		talkingEventChainDictionary[ConversationName.Example] = newTalkingEventChain;

		newTalkingEventChain = new TalkingEventChain ();
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "You there! I have conquered as far as the eye can see-",
		                                                        Panels.LowerLeft,
		                                                        testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Peasent",
		                                                          "Psst, milord. You are literally looking at something that you haven't conquered.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));


		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Silence, lowly peasant!",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "As I was saying, I have conquered-",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bukodan",
		                                                          "Ha! So you are the mighty conqueror. Now you stand before a REAL samurai Daiymo.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Peasent",
		                                                          "Hey, Bukodan! It's me, Benji; we worked together at the stables.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bukodan",
		                                                          "Silence, lowly peasant!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Peasent",
		                                                          "Alright, this is just getting rude.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Peasant, STOP TALKING! Now, I was trying to say that I will be needing your ships, Bukodan. It is an honor to do battle with a real samurai Daiymo that is totally legit, totally. Prepare to die.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		talkingEventChainDictionary[ConversationName.SeaportShowdown] = newTalkingEventChain;

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bukodan",
		                                                          "My archers shall rain sheets of arrows down upon you like the great monsoons!",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Archers",
		                                                          "*Misfires* Mulligan, I call a mulligan.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));



		talkingEventChainDictionary[ConversationName.SeaportShowdownQuip1] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bukodan",
		                                                          "How does it feel to face a real samurai on the field of battle?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          " You are as much a samurai as your mother's sushi is, fresh",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		
		talkingEventChainDictionary[ConversationName.SeaportShowdownQuip2] = newTalkingEventChain;

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bukodan",
		                                                          "My archers shall rain sheets of arrows down upon you like the great monsoons!",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Archers",
		                                                          "*Misfires* Mulligan, I call a mulligan.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		
		talkingEventChainDictionary[ConversationName.SeaportShowdownQuip3] = newTalkingEventChain;
////

//		if (startingConversationID != ConversationName.None)
//			StartTalkingEventChain(startingConversationID); //An example of how you call a conversation. Call from anywhere you want to start a conversation.

        //float xOfRatio = 6 * (Screen.width / Screen.height);
        //float xPositionOfTalker = xOfRatio * .10f;
        Debug.Log(xOfRatio.ToString());
        Debug.Log(Screen.width.ToString());
        Debug.Log(Screen.height.ToString());
        //Debug.Log(xPositionOfTalker);
    }

	/// <summary>
	/// This starts the selected talking event. It also copies the talking events into the currently playing events and resets the main control vairable.
	/// 
	/// Alex Reiss
	/// </summary>
	/// <param name="numberOfEvent">This is the index of the selected event.</param>
	
	public void StartTalkingEventChain(ConversationName conversationID)
	{
		if (conversationID == ConversationName.None)
			return;
		currentTalkingEventChain.Clear();
		currentTalkingEvent = 0;
		
		//I have both the shallow copy and deep copy, because I am not sure which one I need.
		currentTalkingEventChain = talkingEventChainDictionary [conversationID].talkingEvents;
		
//		for (int index = 0; index < talkingEventChain[numberOfEvent].talkingEvents.Count; index++)
//		{
//			currentTalkingEventChain[index].name = talkingEventChain[numberOfEvent].talkingEvents[index].name;
//			currentTalkingEventChain[index].annoyText = talkingEventChain[numberOfEvent].talkingEvents[index].annoyText;
//			currentTalkingEventChain[index].normalText = talkingEventChain[numberOfEvent].talkingEvents[index].normalText;
//			currentTalkingEventChain[index].selectedPanel = talkingEventChain[numberOfEvent].talkingEvents[index].selectedPanel;
//			currentTalkingEventChain[index].theTalker = talkingEventChain[numberOfEvent].talkingEvents[index].theTalker;
//		}
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

	public TalkingEvent(){}
	public TalkingEvent(string name, string normalText, TalkingEventManagerBehaviour.Panels panel, Material talker)
	{
		this.name = name;
		this.normalText = normalText;
		this.selectedPanel = panel;
		this.theTalker = talker;
	}
}
