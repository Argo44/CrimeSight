using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SFXPlayer
{
    // Fields
    private static bool initialized = false;
    private static AudioSource audioPlayer;
    private static SFXSource source;

    public static void Initialize()
    {
        if (initialized) return;

        audioPlayer = Camera.main.GetComponent<AudioSource>();
        source = Camera.main.GetComponent<SFXSource>();
        initialized = true;
    }

    public static void Play(SFX sfx)
    {
        if (!initialized) return;
        
        audioPlayer.PlayOneShot(source.Library[sfx]);
    }
}
