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
		RicePattyIntro,
		RicePattyQuip1,
		RicePattyQuip2,
		RicePattyVictory,
		RicePattyDefeat,
		SeaportShowdownIntro,
		SeaportShowdownQuip1,
		SeaportShowdownQuip2,
		SeaportShowdownQuip3,
		SeaportShowdownVictory,
		SeaportShowdownDefeat,
		OpenPlainsIntro,
		OpenPlainsQuip1,
		OpenPlainsQuip2,
		OpenPlainsVictory,
		OpenPlainsDefeat,
		ManyFeetFieldsIntro,
		ManyFeetFieldsQuip1,
		ManyFeetFieldsQuip2,
		ManyFeetFieldsVictory,
		ManyFeetFieldsDefeat,
		RockyRetreatIntro,
		RockyRetreatQuip1,
		RockyRetreatQuip2,
		RockyRetreatQuip3,
		RockyRetreatVictory,
		RockyRetreatDefeat,
		BambooForestIntro,
		BambooForestQuip1,
		BambooForestQuip2,
		BambooForestVictory,
		BambooForestDefeat,
		ZenRockGardenIntro,
		ZenRockGardenQuip1,
		ZenRockGardenQuip2,
		ZenRockGardenVictory,
		ZenRockGardenDefeat,
		AsianTempleIntro,
		AsianTempleQuip1,
		AsianTempleQuip2,
		AsianTempleVictory,
		AsianTempleDefeat,
		InteriorJapaneseBuildingIntro,
		InteriorJapaneseBuildingQuip1,
		InteriorJapaneseBuildingQuip2,
		InteriorJapaneseBuildingVictory,
		InteriorJapaneseBuildingDefeat,
		JapponCityIntro,
		JapponCityQuip1,
		JapponCityQuip2,
		JapponCityVictory,
		JapponCityDefeat,
		FinalFatalFujiFortressFightIntro,
		FinalFatalFujiFortressFightQuip1,
		FinalFatalFujiFortressFightQuip2,
		FinalFatalFujiFortressFightQuip3,
		FinalFatalFujiFortressFightVictory,
		FinalFatalFujiFortressFightDefeat 
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
		                                                        Panels.LowerRight,
		                                                        testActorRight));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Yoshiro",
		                                                        "Keep him off of my beautiful lawn!",
		                                                        Panels.LowerRight,
		                                                        testActorRight));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Second-san",
		                                                        "Yes, father. I will shoo him away.",
		                                                        Panels.LowerRight,
		                                                        testActorRight));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Hiro",
		                                                        "Mighty Daimyo, I challenge you for rule over these great lands!",
		                                                        Panels.LowerLeft,
		                                                        testActorLeft));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Yoshiro",
		                                                        "You stay off-a my lawn, crazy man!",
		                                                        Panels.LowerRight,
		                                                        testActorRight));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Hiro",
		                                                        "Ha! So you accept?! The tale of our great battle shall be told for eternity!",
		                                                        Panels.LowerLeft,
		                                                        testActorLeft));

		//Save the conversation, and give it a name
		talkingEventChainDictionary[ConversationName.RicePattyIntro] = newTalkingEventChain; 

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Mighty Diamyo, I see you have mustered a glorious army! Sadly, they shall parish in the wake of my fury!",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add(new TalkingEvent ("Enemy Peasent",
		                                                         "Well, I did win a *most improved* ribbon in judo, once!",
		                                                         Panels.LowerRight,
		                                                         testActorRight));

		talkingEventChainDictionary[ConversationName.RicePattyQuip1] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Yoshiro",
		                                                        "Crazy man, I am a farmer!",
		                                                        Panels.LowerRight,
		                                                        testActorRight));
		
		newTalkingEventChain.talkingEvents.Add(new TalkingEvent ("Hiro",
		                                                         "Haha, you are too modest, great Daimyo. This is why I must crush you!",
		                                                         Panels.LowerLeft,
		                                                         testActorLeft));
		
		talkingEventChainDictionary[ConversationName.RicePattyQuip2] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Yoshiro",
		                                                        "*Weeping* Look at my beautiful lawn, she's ruined!",
		                                                        Panels.LowerRight,
		                                                        testActorRight));
		
		newTalkingEventChain.talkingEvents.Add(new TalkingEvent ("Hiro",
		                                                         "You mean MY lawn. You have fallen, mighty Daimyo. I alone am best.",
		                                                         Panels.LowerLeft,
		                                                         testActorLeft));
		
		talkingEventChainDictionary[ConversationName.RicePattyVictory] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add(new TalkingEvent("Yoshiro",
		                                                        " I love my lawn very much, and now you know that.",
		                                                        Panels.LowerRight,
		                                                        testActorRight));
		
		talkingEventChainDictionary[ConversationName.RicePattyDefeat] = newTalkingEventChain;
		#endregion

		#region Level 2
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

		talkingEventChainDictionary[ConversationName.SeaportShowdownIntro] = newTalkingEventChain;

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

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Lieutenant",
		                                                          "That bow is backwards, soldier.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Archer",
		                                                          " Right, like I'm supposed to face the enemy. Good one!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		
		talkingEventChainDictionary[ConversationName.SeaportShowdownQuip3] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "*Mocking* You have been defeated, oh great samurai.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Peasant",
		                                                          "Say, isn't Seppuku applicable here? Yes, no, yes?",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bukodan",
		                                                          "Silence, peasant!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "No, no, protect your honor, mighty samurai. I insist.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bukodan",
		                                                          "Ohhhh, you thought I was a samurai. No, no, no, you must have misunderstood.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.SeaportShowdownVictory] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bukodan",
		                                                          " Ha! You dared to challenge a real Kendo master, and this is the result!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Peasant",
		                                                          "*Coughs* Poser *Coughs*?",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		talkingEventChainDictionary[ConversationName.SeaportShowdownDefeat] = newTalkingEventChain;
		#endregion

		#region Level 3
		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Spearman",
		                                                          "My lord! You did not need to come all the way to our southern border. We have everything under control here, rumors or not no warlord will cross into the holdings of the Hoshi clan.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Of course you do, but it is a Lord's duty to visit every part of his territory and make certain that it is safe.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Archer",
		                                                          "My lord! Troops have been spotted just outside of our southern border!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "WHAT?! Who would dare march upon me, ME! Hoshi Hoshi Hoshi, leader of the Hoshi clan.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Attention nameless lackeys guarding the border!",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "How dare you address my men in such a way! All of my people have names, like that one there is Makoto. And just over there, that one is Yamahari.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Spearman",
		                                                          "What about me, milord?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Hush nameless lackey, I am speaking with the enemy commander.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Right, anyways I was wondering if you would all give up. You see my plans call for world conquest and I would much rather have you all alive and making me richer than dead, so if you could all just put down your arms and-",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "My loyal soldiers would do no such thing. They serve their lord and master with all the dedication they can muster in their tiny dirty hearts!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Is that so? Well who is this lord and master to command such respect?",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "It is none other than I, Hoshi Hoshi Hoshi, son of Hoshi Yoshi Toshi, descendant of the great warrior Hoshi found of the Hoshi clan! My ancestors have ruled this land for eons, we are the terror of the south, the icons of honor and justice! We, are the HOSHI CLAN and we do not bow down to rebels and tyrants from below.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "...Wait, what was that?",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "I SAID IT IS I, HOSHI HOSHI HOSHI, SON OF-",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Right that should occupy him for awhile, everyone get ready this shouldn't take very long.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Spearman",
		                                                          "What about me, milord?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.OpenPlainsIntro] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Weep before the sight of he who is truly the most honorable of men. Stop your foolish ideas of conquest and submit! Hoshi Hoshi Hoshi is an honorable man and will see that you are not harmed more than tradition calls for!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.OpenPlainsQuip1] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "The Hoshi clans borders have not been breached in a thousand years!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Spearman",
		                                                          "Milord, they quite literally breached them only moments ago!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Those Fiends!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.OpenPlainsQuip2] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "You may have pushed us back, but you attacked us from the shadows! You will not be so fortunate next time. Now I, Hoshi Hoshi Hoshi, shall bring my full might down upon you!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.OpenPlainsVictory] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "BHA HA HA HA! You shameful rabble though you could defeat the great Hoshi Hoshi Hoshi? HA HA, Hoshi Hoshi Hoshi laughs laughs laughs at you, here he goes, HA HA HA HA HA HA HA!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.OpenPlainsDefeat] = newTalkingEventChain;
		#endregion

		#region Level 4

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Welcome dishonorable scum! Hoshi Hoshi Hoshi has fulfilled his promise, look as his might is brought down upon you. Hoshi Hoshi Hoshi shall crush [Hiro] with only the most honorable of horses horses horses!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "You know I am not really all that impressed.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Ha ha, Hoshi Hoshi Hoshi is smarter than that. Even now you must be trembling in fear, imagining Hoshi Hoshi Hoshi coming down upon you in a glorious charge! Hoshi shall beat [Hiro] most thoroughly. Everyone to the fields! Charge!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.ManyFeetFieldsIntro] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "The crashing of many horses feet have you terrified, no?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.ManyFeetFieldsQuip1] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "[Hiro] is a stupid name, Ha Ha! Hoshi has made a funny.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.ManyFeetFieldsQuip2] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Most impressive, but those were only a small part of Hoshi Hoshi Hoshi's cavalry!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Spearman",
		                                                          "Milord, they have captured the stables!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "WHAT!? Now Hoshi Hoshi Hoshi looks like an idiot. Hoshi just said that Hoshi has many cavalry and now Hoshi is a liar. Quickly nameless lackeys we must fall back at once. Hoshi Hoshi Hosh will have his revenge [Hiro]!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.ManyFeetFieldsVictory] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Did Hoshi not say he had many horses? Ha Ha today is a most beautiful day. Hoshi shall write you a poem for you defeat. Roses are a Reddo. Viorets are a Bru. Hoshi Hoshi Hoshi has gloriously crushed you. Ha Ha Ha Ha!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.ManyFeetFieldsDefeat] = newTalkingEventChain;
		#endregion

		#region Level 5

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Hoshi is willing to make deal with the most dishonorable [Hiro]. Leave now and Hoshi Hoshi Hoshi shall forgive your transgress..transg...transfats...Hoshi will let you live! Hoshi believes this is the kindest deal Hoshi could ever possibly offer.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "No I do believe I am going to deal with you now and take what few things you have left.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Nonsense, you have taken nothing from the great Hoshi Hoshi Hoshi, son of Hoshi Yoshi Toshi, descenda-",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "I have taken 2/3rds of your land.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "..........",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          ".........",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "You're making that up.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Wait I'm wha-",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Honorable lackeys, defend Hoshi Hoshi Hoshi with your lives!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.RockyRetreatIntro] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "You know you're only 1/3rd the lord you used to be making you just...Hoshi",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Hoshi Hoshi Hoshi cannot hear you over all of his names!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.RockyRetreatQuip1] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "The most honorable tree spirits will come to Hoshi's aid as they have watched over the Hoshi clan for years, right tree spirits? Rrrriiiiggghhht?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.RockyRetreatQuip2] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Tree spirits are dumb, I Hoshi Hoshi Hoshi, do declare this so.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.RockyRetreatQuip3] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "It seems the world no longer needs Hoshi Hoshi Hoshi. [Hiro], you must watch after the Hoshi clan now.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Alright that doesn't sound so bad.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Everyday you must take the men to the fields and impress them with how honorable you are.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "...I'm not doing that.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Then in their time of need, you must follow bushido and show them compassion.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Uhm yeah I could proba-",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Hold them close and rock them to sleep. When they are all tuckered out you must tuck them in.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          ".......",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Also do not forget that every 3rd Wednesday is Karaoke Wednesday. Hoshi was going to do a Journey cover but it seems it is not to be....",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Spearman",
		                                                          "Oh milord! We shall forever remember you!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Uh-huh...You know Hoshi you are a terrible military commander but for some reason your men seem to....like you",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Yes they are like many nameless sons to Hoshi.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "So they are, Hoshi how would you feel about joining me in my conquest.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "What do you want from Hoshi?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "All of your soldiers, women, and a hefty tax placed upon your lands.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Hhhmm....",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          ".....you can keep Karaoke Wednesday.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "You seem like an honorable man, Hoshi will entrust his people to you and follow you in glorious conquest!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.RockyRetreatVictory] = newTalkingEventChain;
	

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hoshi Hoshi Hoshi",
		                                                          "Wait what- err of course Hoshi has emerged victorious! Even if Hoshi hired dirty Ronin Hoshi is still the most Honorable lord to have ever lived, Ha Ha Ha!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.RockyRetreatDefeat] = newTalkingEventChain;



		#endregion

		#region Level 6

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "♫ …love me, ‘til you don’t know how! ♫",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "What is that noise?",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "Huh? You messed up my groove man. And FYI that ‘noise’ is the best thing to ever happen to any karaoke bar.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "My blade to your throat will be the best thing to ever happen for music. Prepare to die!",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		talkingEventChainDictionary[ConversationName.BambooForestIntro] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "Did you know that Karaoke means ‘empty orchestra.’ Isn’t that hauntingly beautiful",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.BambooForestQuip1] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "I love everybody in this bar.. er, I mean forest. ♫ I’m a fool again. I fell in love with you again… ♫",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.BambooForestQuip2] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "I need to go. My fans are calling me.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.BambooForestVictory] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "Dishonor on your whole family, on you, and on your cow.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.BambooForestDefeat] = newTalkingEventChain;
		#endregion

		#region Level 7

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "It is time for my encore.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Wah Wutishigudfah",
		                                                          "Hai, and I am here to back you up",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Stop, it’s time to finish this war",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "What do you know about War?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Everybody knows war",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "♫ War! What is it good for? ♫",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Wah Wutishigudfah",
		                                                          "♫ Absolutely nothing! Sing it again ♫",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "♫ WA—",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "My ears! I will get revenge for what you have done to my ears.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		talkingEventChainDictionary[ConversationName.ZenRockGardenIntro] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "You know how I know I’m going to kill you. Because I’m a man. I’m tranquil like a forest, and on fire within.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.ZenRockGardenQuip1] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "You getting nervous yet? Your palms look sweaty. Maybe your knees are weak or your arms are heavy. What is that? Looks like you got spaghetti on your sweater. Did your mom make that?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.ZenRockGardenQuip2] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "♫ You’re the best around ♫",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.ZenRockGardenVictory] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Shao Monlee",
		                                                          "Wipe yourself off. YOU DEAD!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.ZenRockGardenDefeat] = newTalkingEventChain;
		#endregion

		#region Level 8

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "om… om… om…",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Hey, stop that om-ing. You eating or meditating?",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Hey, you need to stop winning so that this campaign lasts longer.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "……",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "You will never defeat me!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Who’s Mi?",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "I’m Mi… no, I’m Bado Gai. You frustrate me. You shall die.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.AsianTempleIntro] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "It’s okay to lose. We don’t always have to be winners… unless you’re me",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.AsianTempleQuip1] = newTalkingEventChain;
		
		
		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Why don’t you just die already? Just give up and die. Did that sound too harsh? I’m sorry… NOT!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.AsianTempleQuip2] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "I should go home and cook rice",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.AsianTempleVictory] = newTalkingEventChain;
		
		
		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Now where was I… om… om… om…",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.AsianTempleDefeat] = newTalkingEventChain;
		#endregion

		#region Level 9

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Hao Long, deal with this. I have to go cook rice.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hao Long",
		                                                          "Hai",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Just give up, whatever your name is, and get out of my way",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hao Long",
		                                                          "Whatever my name is??? Hao Long is my name.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "I don’t know, like 7 letters long.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));


		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hao Long",
		                                                          "What?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Huh? Whatever, I will kill you and Bado Gai like I killed Shao Monlee and the rest I’ve killed.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hao Long",
		                                                          "You killed my son, Shao Monlee? FINALLY! He has always been a failure, ever since he brought home a 95 on a math test. He deserved to die, and so do you.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.InteriorJapaneseBuildingIntro] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hao Long",
		                                                          "What’s taking you so long? I could have killed me by now if I were you.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.InteriorJapaneseBuildingQuip1] = newTalkingEventChain;
		
		
		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hao Long",
		                                                          "You suck at this whole thing. You obviously haven’t done your homework.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.InteriorJapaneseBuildingQuip2] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hao Long",
		                                                          "What took so long? You did not do so well, but you did kill me. So… I give you a B+.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.InteriorJapaneseBuildingVictory] = newTalkingEventChain;
		
		
		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hao Long",
		                                                          "You failed… in life, in this game, in school, on your piano lessons, on your SATs… yeah, you get the message.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.InteriorJapaneseBuildingDefeat] = newTalkingEventChain;
		#endregion

		#region Level 10

		newTalkingEventChain = new TalkingEventChain();

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Knock, knock...",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "No, nope, un-uh, I refuse!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Oh, come on!",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "*Sighs* Who's there?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Not your buddy Hao Long, cause he's dead now!",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Funny... you're sick you know. And what happens to sick men? They die! By my blades!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "You kill sick people? That's a little immoral, don't ya think?",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "You stain my city with your presence. With my blades as my baking soda, and my fury as my vinegar, I shall scrub away your stain!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Really, that's supposed to be threatening?",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Maybe you can try going on a comedy tour after I POUND YOU INTO SUBMISSION!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.JapponCityIntro] = newTalkingEventChain;

		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Do you hear that? Your mother is calling from my bedchambers. We need to hurry this battle up. Ha ha ha! Get it?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.JapponCityQuip1] = newTalkingEventChain;
		
		
		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Oh, Mister Comedian, it seems that your army is the joke here!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.JapponCityQuip2] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "I have to leave now, your mother is calling again!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.JapponCityVictory] = newTalkingEventChain;
		
		
		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Knock, knock... Oh, you're dead! Ha ha ha!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.JapponCityDefeat] = newTalkingEventChain;
		#endregion

		#region Level 11

		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "You just had to run, didn't you?",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "I told you, your mother was calling! Haha, remember when I said that? Do you?",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Enough! Time to end this and prove once and for all that I am best!",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Ha! These walls have never been breached by any man, let alone puny Hiro!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "If only the same could be said about your sister!",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Hey! Well, actually, this is true. No matter, your end has come know, puny warrior!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		talkingEventChainDictionary[ConversationName.FinalFatalFujiFortressFightIntro] = new TalkingEventChain();


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Hear, have some tea in my cherry garden... Just kidding, I tricked you!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.FinalFatalFujiFortressFightQuip1] = newTalkingEventChain;
		
		
		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "Please, don't feed the fish, they're on a diet.",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.FinalFatalFujiFortressFightQuip2] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "I'm so good at this... is what your mother tells me!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.FinalFatalFujiFortressFightQuip3] = newTalkingEventChain;


		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "So, now that you are retired, when's your first comedy show, funny man! Ha ha ha!",
		                                                          Panels.LowerRight,
		                                                          testActorRight));
		
		talkingEventChainDictionary[ConversationName.FinalFatalFujiFortressFightVictory] = newTalkingEventChain;
		
		
		newTalkingEventChain = new TalkingEventChain();
		
		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Bado Gai",
		                                                          "GAME OVER. Insert coin to continue. 5. 4. 3 -",
		                                                          Panels.LowerRight,
		                                                          testActorRight));

		newTalkingEventChain.talkingEvents.Add (new TalkingEvent ("Hiro",
		                                                          "Stop it, just, stop it.",
		                                                          Panels.LowerLeft,
		                                                          testActorLeft));
		
		talkingEventChainDictionary[ConversationName.FinalFatalFujiFortressFightDefeat] = newTalkingEventChain;
		#endregion
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
