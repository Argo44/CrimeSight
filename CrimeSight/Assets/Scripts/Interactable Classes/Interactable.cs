using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Base class for all game objects that can be interacted with
// Don't really need objects of this type, use subclasses instead

public class Interactable : MonoBehaviour
{
    public GameObject crosshair;
    public GameObject interactTextObject;


    private void Start()
    {
        //Sets the crosshair and interact text objects so they can be updated
        crosshair = GameObject.Find("Crosshair");
        interactTextObject = GameObject.Find("Interact Text");
    }

    public virtual void OnSelect()
    {
        // Highlight object - swap to Outline layer
        // Update cursor & text
        Debug.Log(gameObject.name + " was selected");
        
        //Changes crosshair to red
        crosshair.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        
        //Updates and shows prompt on screen using the name of the GameObject
        TextMeshProUGUI interactText = interactTextObject.GetComponent<TextMeshProUGUI>();
        interactText.text = "Press 'E' to Interact with \"" + this.name + "\"";
        interactTextObject.SetActive(true);
    }

    public virtual void OnDeselect()
    {
        // De-highlight object - swap to default layer
        // Update cursor & text
        Debug.Log(gameObject.name + " was deselected");

        //Changes crosshair back to white
        crosshair.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        
        //Hides prompt text
        interactTextObject.SetActive(false);
    }

    public virtual void OnInteract()
    {
        Debug.Log(gameObject.name + " activated!");
    }
}
