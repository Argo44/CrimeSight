using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Interactable
{
    private bool isActive = true;

    // Start is called before the first frame update
    void Start()
    {
        //Sets the crosshair and interact text objects so they can be updated
        crosshair = GameObject.Find("Crosshair");
        interactTextObject = GameObject.Find("Interact Text");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when a Collider intersects with this object's Collider
    // i.e. when player walks into trap
    private void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            OnDetonate();
        }
    }

    public override void OnInteract()
    {
        if (!isActive) return;

        // Start disarm QTE
        Debug.Log("Trap disarmed!");
        isActive = false;
    }

    /// <summary>
    /// Deals damage to player and disables this trap
    /// </summary>
    public void OnDetonate()
    {
        if (!isActive) return;

        Debug.Log("A trap went off!");
        isActive = false;

        // Add some visual effect
        // Remove from scene
    }
}
