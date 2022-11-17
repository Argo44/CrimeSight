using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ClueType
{
    Green,
    Blue,
    Purple,
}

public class Clue : Interactable
{
    // Fields
    private bool initialized = false;
    public ClueType type;
    private string clueName;
    private string info;
    private UnityAction updateClueCount;

    // Loads clue info for this object
    public void Initialize(ClueType _type, UnityAction clueCollectUpdateCallback)
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

            updateClueCount = clueCollectUpdateCallback;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // move Initialize to Clue component
    // Create callback method in InteractableManager
    // When clue.OnInteract is called, invoke callback
    // Callback alerts UI of new clue

    public override void OnInteract()
    {
        // Add clue to notebook
        updateClueCount();

        // Remove object(?)


        Debug.Log("Clue collected!");
    }
}
