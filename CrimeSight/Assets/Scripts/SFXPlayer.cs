using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{ 
    ClueCollect,
    SightActivation
}

public static class SFXPlayer
{
    // Fields
    private static bool initialized = false;
    private static AudioSource source;

    public static void Initialize()
    {
        if (initialized) return;

        source = Camera.main.GetComponent<AudioSource>();
        initialized = true;
    }
}
