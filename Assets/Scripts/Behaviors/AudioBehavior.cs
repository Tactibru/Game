using UnityEngine;
using System.Collections;

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
    public AudioSource audioSource;
    public AudioClip gameMusic;
    public AudioClip combatMusic;

    private float audio1Volume;
    private float audio2Volume;
    private float audio1Pitch;
    private float audio2Pitch;
    private bool track1Playing;
    private bool track2Playing;
    private float volume;
    private float pitch;

    //variables used for volume
    //not used yet
    public float value1;
    public float value2;
    public float value3;
    public float value4;

    void Start()
    {
        fadeOut(volume, pitch);
        track2Playing = false;
        value1 = 25f;
        value2 = 50f;
        value3 = 75f;
        value4 = 90f;
    }

    /// <summary>
    /// TESTING PURPOSES: R=Game Music T= Combat Music
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
            audio.clip = gameMusic;
            audio.Play();

        }
        if (Input.GetKey(KeyCode.T))
        {
            track1Playing = false;
            track2Playing = true;
            //fadeIn(volume, pitch);
            audio.clip = combatMusic;
            audio.Play();
        }

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
    }

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
}
