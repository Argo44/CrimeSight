using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for environmental objects that change when interacted with
// Examples: Doors, cabinets

public class ActionObject : Interactable
{
    // Fields
    public Vector3 activeState;
    private Vector3 inactiveState; // Default state
    private bool isActivated = false;

    public override void OnInteract()
    {
        //if (isActivated)
            // Tween to inactive state
        //else
            // Tween to active state
    }
}
