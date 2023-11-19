using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioClip defaultAmbience;
    public AudioMixer audioMixer; 
    private AudioSource track01, track02;
    private bool isPlayingTrack01;

    public int activateTime = 0;
    
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void Start()
    {
        track01 = gameObject.AddComponent<AudioSource>();
        track02 = gameObject.AddComponent<AudioSource>();
        isPlayingTrack01 = true;

        // Assign the Audio Mixer to both AudioSources
        if (audioMixer != null)
        {
            track01.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
            track02.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
        }

        // Set the volume to half (0.5f)
        track01.volume = 0.5f;
        track02.volume = 0.5f;

        SwapTrack(defaultAmbience);
    }

    public void SwapTrack(AudioClip newClip)
    {
        //StopAllCorountines();
        StartCoroutine(FadeTrack(newClip)); //this is if we want to make the audio get triggered by a collider Ex. Entering into a dark hallway

        activateTime++;

        if(activateTime == 0)
        {
            isPlayingTrack01 = isPlayingTrack01;
        }
        else if(activateTime == 1)
        {
            isPlayingTrack01 = !isPlayingTrack01;
        }
    }

    public void activateTimeZero()
    {
        activateTime = 0;
    }
    
    public void activateTimeOne()
    {
        activateTime = 1;
    }

    public void ReturnToDefault()
    {
        SwapTrack(defaultAmbience);
    }

    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float timeToFade = 5f;
        float timeElapsed = 0f;

        if (isPlayingTrack01) // if track one is playing and needs to be stopped
        {
            track02.clip = newClip; 
            track02.Play();
            while(timeElapsed < timeToFade)
            {
                track02.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade); //from 0 volume to 1 volume
                track01.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade); //from 1 volume to 0 volume
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track01.Stop();
        }
        else // if track one is not playing and needs to be Played
        {
            track01.clip = newClip;
            track01.Play();

            while(timeElapsed < timeToFade)
            {
                track01.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade); //from 0 volume to 1 volume
                track02.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade); //from 1 volume to 0 volume
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track02.Stop();
        }
    } 
}
