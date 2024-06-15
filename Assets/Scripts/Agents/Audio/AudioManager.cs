using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AudioEvent
{
    public CardType cardType;
    public AudioClip audioClip;
}
public class AudioManager : MonoBehaviour
{
    // public List<AudioClip> areasAudioClips;

    
    public List<AudioEvent> eventsAudioClips;
    
    public AudioSource audioSource;
    

    public void PlaySound(CardType cardType)
    {
        // Play sound

        foreach (AudioEvent audioEvent in eventsAudioClips)
        {
            if (audioEvent.cardType == cardType)
            {
                // Play sound
                Debug.Log("Play sound");

                if (audioEvent.audioClip != null)
                {
                    audioSource.PlayOneShot(audioEvent.audioClip);
                }
            }
        }
    }
}
