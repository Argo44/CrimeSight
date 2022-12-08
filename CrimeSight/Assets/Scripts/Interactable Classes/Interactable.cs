using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Base class for all game objects that can be interacted with
// Don't really need objects of this type, use subclasses instead

public class Interactable : MonoBehaviour
{
  
    private void Start()
    {
        
    }

    public virtual void OnSelect()
    {
        // Highlight object - swap to Outline layer
        Debug.Log(gameObject.name + " was selected");
    }

    public virtual void OnDeselect()
    {
        // De-highlight object - swap to default layer
        Debug.Log(gameObject.name + " was deselected");
    }

    public virtual void OnInteract()
    {
        Debug.Log(gameObject.name + " activated!");
    }
}
