using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

// Base class for all game objects that can be interacted with
// Don't really need objects of this type, use subclasses instead

public class Keys : Interactable
{
    public bool collected = false;
    private UnityAction updateClueCount;

    public KeyManager keyScript;


    public void Initialize(UnityAction clueCollectUpdateCallback)
    {
        //updateClueCount = clueCollectUpdateCallback;
    }

    private void Start()
    {
        
    }

    public override void OnSelect()
    {
        // Highlight object - swap to Outline layer
        Debug.Log(gameObject.name + " was selected");
    }

    public override void OnDeselect()
    {
        // De-highlight object - swap to default layer
        Debug.Log(gameObject.name + " was deselected");
    }

    public override void OnInteract()
    {
        collected = true;
        this.gameObject.SetActive(false);
        keyScript.AddKey();

        Debug.Log("This was interacted with");
        Debug.Log("key collected!");
    }
}
