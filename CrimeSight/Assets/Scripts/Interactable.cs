using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnSelect()
    {
        // Highlight object - swap to Outline layer
        // Update cursor & text
        Debug.Log(gameObject.name + " was selected");
    }

    public virtual void OnDeselect()
    {
        // De-highlight object - swap to default layer
        // Update cursor & text
        Debug.Log(gameObject.name + " was deselected");
    }

    public virtual void OnInteract()
    {
        // We haven't really defined a default interaction...
        // This can be overriden by subclasses
        // Maybe we just have classes for each type of interactable? (doors, keys, clues, traps)
        // Any other types of interactables?
        Debug.Log(gameObject.name + " activated!");
    }
}
