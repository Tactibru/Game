using UnityEngine;
using System.Collections;

/// <summary>
/// The Typewriter itself.
/// </summary>
[AddComponentMenu("Tactibru/Dialog/Type-Writer Box")]
public class TypeWriterBoxBehaviour : MonoBehaviour 
{
    /// <summary>
    /// To tell position of the box.
    /// </summary>
    public enum PositionOfTheTextBox
    {
        top, 
        bottom,
        none,
        NUM_POSITIONS
    }
    public PositionOfTheTextBox posOfBox;
    
    /// <summary>
    /// Used to tell when the typewriter is typeing.
    /// </summary>

    bool currentlyPlaying = false;

    /// <summary>
    /// These hold both the normal text and the annoy text.
    /// </summary>

    char [] normalText;
    char [] annoyText;

    /// <summary>
    /// Name of the talking charactor.
    /// </summary>

    string nameOfTalking = "";

    /// <summary>
    /// What is currently displayed on the screen.
    /// </summary>

    string currentlyDisplayedText = "";

    /// <summary>
    /// This is neccessary to tell the talking manager when the event is going and it ends.
    /// </summary>
	[System.NonSerialized]
    public TalkingEventManagerBehaviour talkingManager;

    /// <summary>
    /// The GUI style, for the text.
    /// </summary>

    public GUIStyle textStyle;
    public GUIStyle nameStyle;

    /// <summary>
    ///  This can be set by the game designer. It is the time between letter.
    /// </summary>

    public float timeBetweenLetters = 0.1f;

    /// <summary>
    /// The countdown.
    /// </summary>

    private float currentTime = 0.0f;

    /// <summary>
    /// This is the index of what is next to be printed.
    /// </summary>

    private int currentIndex = 0;

    /// <summary>
    /// This is for when the type writer to continue.
    /// </summary>

    private bool readyToContinue = false;

    /// <summary>
    /// This is to tell the typewriter when to go to the next line.
    /// </summary>

    private int textBoxSize = 66;

    /// <summary>
    /// This the count up to tell the ype writer to go to the next line.
    /// </summary>

    private int currentNumberCharsInLine = 0;

    /// <summary>
    /// These arrays tell the typewriter where the spaces are so the end of the word.
    /// </summary>

    private int[] normalSpaceLocations;
    private int[] annoySpaceLocations;

    /// <summary>
    /// Tracks what space to pay attention to.
    /// </summary>

    private int currentSpace = 0;

    /// <summary>
    /// Toggle for if the event has an annoy
    /// </summary>

    private bool hasAnnoy = false;

    /// <summary>
    /// Sets what needs to be set.
    /// 
    /// Alex Reiss
    /// </summary>
	
	void Start () 
    {
        talkingManager = Camera.main.transform.GetComponent<TalkingEventManagerBehaviour>();
        transform.renderer.enabled = false;
	}
	
	/// <summary>
	/// The first if statement is the typewriter itself.
    /// The second if statement is for when the typewriter is done.
    /// the third if statement is to finish the typeing.
    /// 
    /// Alex Reiss
	/// </summary>

	void Update () 
    {
        if (currentlyPlaying)
        {
            if (currentTime < 0.0f)
            {
                
                //if (currentNumberCharsInLine < textBoxSize)
                //    currentlyDisplayedText += normalText[currentIndex].ToString();
                //else
                //{
                //    currentlyDisplayedText += "\n";
                //    currentlyDisplayedText += normalText[currentIndex].ToString();
                //    currentNumberCharsInLine = 0;
                //}

                currentlyDisplayedText += normalText[currentIndex].ToString();

                if (normalSpaceLocations.Length > 0)
                {
                    if (currentIndex == normalSpaceLocations[currentSpace] && currentSpace < normalSpaceLocations.Length - 1)
                    {
                        currentSpace++;
                        if (currentNumberCharsInLine + (normalSpaceLocations[currentSpace] - currentIndex) > textBoxSize)
                        {
                            currentlyDisplayedText += "\n";
                            currentNumberCharsInLine = 0;
                        }
                    }
                }
                
                currentIndex++;
                currentNumberCharsInLine++;
                currentTime = timeBetweenLetters;

                if (currentIndex == normalText.Length)
                {
                    currentlyPlaying = false;
                    readyToContinue = true;
                }
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }

        if (readyToContinue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                talkingManager.playingEvent = false;
                readyToContinue = false;
                currentlyDisplayedText = "";
            }
        }

        if (currentlyPlaying && currentIndex > 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                readyToContinue = true;
                currentlyPlaying = false;

                if (hasAnnoy)
                {
                    currentlyDisplayedText = "";
                    currentIndex = 0;
                    currentSpace = 0;
                    currentNumberCharsInLine = 0;

                    while (currentIndex < annoyText.Length)
                    {
                        currentlyDisplayedText += annoyText[currentIndex].ToString();

                        if (annoySpaceLocations.Length > 0)
                        {
                            if (currentIndex == annoySpaceLocations[currentSpace] && currentSpace < annoySpaceLocations.Length - 1)
                            {
                                currentSpace++;
                                if (currentNumberCharsInLine + (annoySpaceLocations[currentSpace] - currentIndex) > textBoxSize)
                                {
                                    currentlyDisplayedText += "\n";
                                    currentNumberCharsInLine = 0;
                                }
                            }
                        }

                        currentIndex++;
                        currentNumberCharsInLine++;
                    }

                }
                else
                {
                    while (currentIndex < normalText.Length)
                    {
                        currentlyDisplayedText += normalText[currentIndex].ToString();
                        if (normalSpaceLocations.Length > 0)
                        {
                            if (currentIndex == normalSpaceLocations[currentSpace] && currentSpace < normalSpaceLocations.Length - 1)
                            {
                                currentSpace++;
                                if (currentNumberCharsInLine + (normalSpaceLocations[currentSpace] - currentIndex) > textBoxSize)
                                {
                                    currentlyDisplayedText += "\n";
                                    currentNumberCharsInLine = 0;
                                }
                            }
                        }

                        currentIndex++;
                        currentNumberCharsInLine++;
                    }
                }
            }
        }
	}

    /// <summary>
    /// Starts the typewriter.
    /// 
    /// Alex Reiss
    /// </summary>

    public void startTalkingEvent(string newNormalText, string newAnnoyText, string name)
    {
        normalText = newNormalText.ToCharArray();
        nameOfTalking = name;

        if (newAnnoyText.Length > 0)
        {
            hasAnnoy = true;
            annoyText = newAnnoyText.ToCharArray();
        }
        else
            hasAnnoy = false;

        currentlyPlaying = true;
        currentIndex = 0;
        currentSpace = 0;
        currentNumberCharsInLine = 0;

        talkingManager.playingEvent = true;

        //currentlyDisplayedText = normalText[0].ToString();
        currentTime = timeBetweenLetters;
        gettingSpaceLocations();
    }

    /// <summary>
    /// Gets the spaces recorded for the typewriter.
    /// 
    /// Alex Reiss
    /// </summary>

    private void gettingSpaceLocations()
    {
        int numberOfSpaces = 0;
        for (int index = 0; index < normalText.Length; index++)
            if (normalText[index] == ' ')
                numberOfSpaces++;

        normalSpaceLocations = new int[numberOfSpaces];

        int numberOfSpacesFound = 0;
        int step = 0;
        while (numberOfSpacesFound < numberOfSpaces)
        {
            if (normalText[step] == ' ')
            {
                normalSpaceLocations[numberOfSpacesFound] = step;
                numberOfSpacesFound++;
            }
            step++;
        }

        if (hasAnnoy)
        {
            numberOfSpaces = 0;
            for (int index = 0; index < annoyText.Length; index++)
                if (annoyText[index] == ' ')
                    numberOfSpaces++;

            annoySpaceLocations = new int[numberOfSpaces];

            numberOfSpacesFound = 0;
            step = 0;
            while (numberOfSpacesFound < numberOfSpaces)
            {
                if (annoyText[step] == ' ')
                {
                    annoySpaceLocations[numberOfSpacesFound] = step;
                    numberOfSpacesFound++;
                }
                step++;
            }
        }
    }

    /// <summary>
    /// Displayes the text.
    /// 
    /// Alex Reiss
    /// </summary>

    void OnGUI()
    {
        if (currentlyPlaying || readyToContinue)
        {
            if (posOfBox == PositionOfTheTextBox.top)
            {
                GUI.Label(new Rect(Screen.width * 0.25f, Screen.height * 0.05f, Screen.width * 0.5f, Screen.height * 0.1f), nameOfTalking, nameStyle);
                GUI.Label(new Rect(Screen.width * 0.25f, Screen.height * 0.1f, Screen.width * 0.5f, Screen.height * 0.1f), currentlyDisplayedText, textStyle);
            }
            else
            {
                //GUI.Label(new Rect(Screen.width * 0.25f, Screen.height * 0.8f, Screen.width * 0.5f, Screen.height * 0.1f), currentlyDisplayedText, gUIStyle);
                GUI.Label(new Rect(Screen.width * 0.20f, Screen.height * 0.62f, Screen.width * 0.5f, Screen.height * 0.1f), nameOfTalking, nameStyle);
                GUI.Label(new Rect(Screen.width * 0.21f, Screen.height * 0.7f, Screen.width * 0.5f, Screen.height * 0.1f), currentlyDisplayedText, textStyle);

            }
        }
    }
}
