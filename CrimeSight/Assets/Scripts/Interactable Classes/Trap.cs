using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trap : Interactable
{
    private bool isArmed = true;
    private bool isMarked = false;
    public UnityAction onDetonate;

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
        if (isArmed)
            Detonate();
    }

    public override void OnInteract()
    {
        if (!isArmed) return;

        // Start disarm QTE

        Debug.Log("Trap disarmed!");
        isArmed = false;
    }

    // Only select trap if it is marked with Sight
    public override void OnSelect()
    {
        if (isMarked)
            base.OnSelect();
    }

    public override void OnDeselect()
    {
        if (isMarked)
            base.OnDeselect();
    }

    // Allows player to select and disarm trap
    public void Mark()
    {
        isMarked = true;
    }

    /// <summary>
    /// Deals damage to player and disables this trap
    /// </summary>
    public void Detonate()
    {
        if (!isArmed) return;

        Debug.Log("A trap went off!");
        isArmed = false;
        // Deal damage to player

        // Remove trap from scene
        onDetonate?.Invoke();
    }
}
