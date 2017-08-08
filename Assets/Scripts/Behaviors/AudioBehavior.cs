using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that switches between Game music, combat music, and combat sounds.
///  
/// Primary: Darryl Sterne
/// Secondary: 
/// </summary>
[AddComponentMenu("Tactibru/Core Systems/Audio Manager")]
[RequireComponent(typeof(AudioSource))]
public class AudioBehavior : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// List for game music, combat music, combat audio, and movement audio
    /// Accessable in the editor to add or delete audio clip from list
    /// </summary>
    public List<AudioClip> gameMusic;
    public List<AudioClip> combatMusic;
    public List<AudioClip> combatAudio;
    public List<AudioClip> movementAudio;

    public AudioSource audioSource;
    
    
    /// <summary>
    /// Bools used to check if in combat, if audio has been played. Also, checks if it is
    /// game music or combat music so that it transitions between both
    /// </summary>
    public static bool inCombat;
    public static bool hasPlayedAudio = false;
    public static bool isGameMusic = true;
    public static bool isCombatMusic = false;
    public static bool isMoving = false;

    /// <summary>
    /// Don't mess with this inside of the editor. It was used to
    /// make Unity play music and sounds at the same time. -Darryl
    /// </summary>
    public bool isMusic;

    #endregion

    void Start()
    {

    }

    /// <summary>
    /// Checks to see if incombat or out of combat and switches to the
    /// methods accordingly
    /// </summary>
    void Update()
    {
        if (inCombat)
        {
            InCombat();

        }

        if (!inCombat)
        {
            NotInCombat();
        }
    }

    #region Methods
    public virtual void InCombat()
    {
        if (isMusic == true && isCombatMusic == true)
        {
            GetComponent<AudioSource>().clip = combatMusic[Random.Range(0, combatMusic.Count)];
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
            isCombatMusic = false;
            isGameMusic = true;
        }

        if (hasPlayedAudio == false && !isMusic)
        {
            GetComponent<AudioSource>().clip = combatAudio[1];
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
            hasPlayedAudio = true;
        }
    }

    public virtual void NotInCombat()
    {
        if (isMusic == true && isGameMusic == true)
        {
            GetComponent<AudioSource>().clip = gameMusic[Random.Range(0, gameMusic.Count)];
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
            isGameMusic = false;
            isCombatMusic = true;
        }

        if (isMoving == true && !isMusic && !hasPlayedAudio)
        {
            GetComponent<AudioSource>().clip = movementAudio[0];
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
            hasPlayedAudio = true;
        }

        if (!isMusic && !isMoving)
        {
            hasPlayedAudio = false;
        }
        
        if (!inCombat && !isMusic)
        {
            GetComponent<AudioSource>().loop = false;
        }
    }

    #endregion
}
