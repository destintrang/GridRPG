using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour {


    AudioSource source;

    public AudioClip endTurn;

    public AudioClip strike; 
    public AudioClip miss;
    public AudioClip death;

    //Singleton
    public static SoundEffectsManager instance;
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }


    public void PlayStrike ()
    {
        source.PlayOneShot(strike);
    }

    public void PlayMiss ()
    {
        source.PlayOneShot(miss);
    }

    public void PlayEndTurn ()
    {
        source.PlayOneShot(endTurn);
    }

    public void PlayDeath ()
    {
        source.PlayOneShot(death);
    }
}
