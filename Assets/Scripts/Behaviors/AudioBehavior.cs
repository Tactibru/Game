using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that switches between Game music and combat music.
/// TESTING PURPOSES: R=Game Music T= Combat Music
/// 
/// Primary: Darryl Sterne
/// Secondary: 
/// </summary>

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
    public AudioSource audioSource;
    //Future audio support: public List<AudioClip> movementAudio;

    //variables used to control volume and pitch of two different audio clips
    //while fading in and out of the 2 audio clips
    private float audio1Volume;
    private float audio2Volume;
    private float audio1Pitch;
    private float audio2Pitch;
    private float volume = 0.0f;
    private float pitch = 0.0f;
    private bool track1Playing;
    private bool track2Playing;

    //Used to check if audio plays at once or one at a time
    public bool playOneAtATime;

    //variables used for volume to fade in out with
    //not used yet
    public float value1;
    public float value2;
    public float value3;
    public float value4;
    #endregion

    void Start()
    {
        fadeOut(volume, pitch);
        track2Playing = false;
        value1 = 25f;
        value2 = 50f;
        value3 = 75f;
        value4 = 99f;
    }

    /// <summary>
    /// TESTING PURPOSES: R=Game Music T= Combat Music Y= Combat Sounds
    /// </summary>
    void Update()
    {
        fadeOut(volume, pitch);
        //volume = handleVolume();
        //pitch = handlePitch();

        if (Input.GetKey(KeyCode.R))
        {
            track2Playing = false;
            track1Playing = true;
            //fadeIn(volume, pitch);
            audio.clip = gameMusic[Random.Range(0, combatAudio.Count)];
            audio.Play();

        }
        if (Input.GetKey(KeyCode.T))
        {
            track1Playing = false;
            track2Playing = true;
            //fadeIn(volume, pitch);
            audio.clip = combatMusic[Random.Range(0, combatAudio.Count)];
            audio.Play();
        }

        if (Input.GetKey(KeyCode.Y))
        {
            CombatAudio(combatAudio);
        }
        #region unused
        //Fade in and out for music still needs
        //Playing Game Music
        //if (audio2Volume <= 0.1f)
        //{
        //    if (track1Playing == true && track2Playing == false)
        //    {
        //        track1Playing = true;
        //        fadeIn(volume, pitch);
        //        audio.clip = gameMusic;
        //        audio.Play();
        //    }
        //    else
        //    {
        //        fadeOut(volume, pitch);
        //    }
        //}
        ////Playing Combat Music
        //if (audio2Volume >= 0.1f)
        //{
        //    if (track1Playing == false && track2Playing == true)
        //    {
        //        track2Playing = true;
        //        fadeIn(volume, pitch);
        //        audio.clip = combatMusic;
        //        audio.Play();
        //    }
        //    else
        //    {
        //        fadeOut(volume, pitch);
        //    }
        //}
        #endregion
    }

    #region Methods
    /// <summary>
    /// Fades in from one audio source to another
    /// </summary>
    void fadeIn(float volume, float pitch)
    {
        if (audio1Volume <= volume)
        {
            audio1Volume += 0.1f * Time.deltaTime;
            audio.volume = audio1Volume;
        }

        if (audio1Pitch <= pitch)
        {
            audio1Pitch += 0.3f * Time.deltaTime;
            audio.pitch = audio1Pitch;
        }
    }

    /// <summary>
    /// Fades out from one audio source to another
    /// </summary>
    void fadeOut(float volume, float pitch)
    {
        if (track1Playing == true && volume > 0.1f)
        {
            audio1Volume -= 0.1f * Time.deltaTime;
            audio.volume = audio1Volume;
        }
    }

    /// <summary>
    /// Plays Combat audio like attack sounds etc. Also checks if sound needs to wait or
    /// all sounds play at same time.
    /// Randomly selects combat sounds from the list of combat audio. 
    /// </summary>
    void CombatAudio(List<AudioClip> combatAudio)
    {
        audio.clip = combatAudio[Random.Range(0, combatAudio.Count)];

        if (playOneAtATime == true)
        {
            PlayingAudioClip();
        }
        else
        {
            audio.Play();
        }

    }

    void PlayingAudioClip()
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }
    }
    #endregion
}
