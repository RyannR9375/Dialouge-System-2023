using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;
    public float audioVolume = 0.5f;
    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        //SUBSCRIBE TO EVENT 'PlayAudio', INVOKE 'PlayAudioClip()'
        EvtSystem.EventDispatcher.AddListener<PlayAudio>(PlayAudioClip);
    }

    //FUNCTION TO START AN AUDIO CLIP, OPTION TO PRIORITIZE OR NOT,
    //IF PRIORITY, CANCELS OTHER AUDIO CLIPS IF THERE ARE ANY 
    //IF NOT, START ANYWAY
    private void PlayAudioClip(PlayAudio eventData)
    {
        //IF THE AUDIO IS MARKED AS PRIORITY, STOP OTHER AUDIOS
        if (eventData.isPriority)
        {
            audioSource.Stop();
        }

        if(audioSource != null)
        {
            audioSource.PlayOneShot(eventData.clipToPlay, audioVolume);
        }
    }
}

