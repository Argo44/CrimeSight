using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClueType
{
    Green,
    Blue,
    Purple,
}

public class Interactable : MonoBehaviour
{
    private bool initialized = false;
    public ClueType type;

    private string clueName;
    private string info;
    //private bool isSelected = false;
    //private bool isFound = false;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize(type);
    }

    public void Initialize(ClueType _type)
    {
        // Only initialize once
        if (!initialized)
        {
            initialized = true;

            //Type of clue that it is
            switch (_type)
            {
                case ClueType.Green:
                    clueName = "Green";
                    info = "This is a green clue";
                    break;

                case ClueType.Blue:
                    clueName = "Blue";
                    info = "This is a blue clue";
                    break;

                case ClueType.Purple:
                    clueName = "Purple";
                    info = "This is a purple clue";
                    break;

                default:
                    clueName = "Default";
                    info = "Default";
                    break;
            }
        }
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
