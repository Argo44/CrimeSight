using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for environmental objects that change when interacted with
// Examples: Doors, cabinets, moving objects

public class ActionObject : Interactable
{
    // Fields
    public bool tweenOnInteract = true;
    public TweenShape tweenShape = TweenShape.Linear;
    public Vector3 activatedPosition = Vector3.zero; // State to tween to
    public Vector3 activatedRotation = Vector3.zero;
    private Vector3 inactivePos; // Default state
    private Vector3 inactiveRot;
    public float tweenTime = 1.0f;
    private float tweenTimer = 0;
    private bool isActivated = false;

    private void Start()
    {
        // Store current transform data
        inactivePos = transform.position;
        inactiveRot = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        // Update tweening time
        if (tweenTimer > 0) tweenTimer -= Time.deltaTime;
    }

    public override void OnInteract()
    {
        // Do not tween if unselected or while tweening
        if (!tweenOnInteract || tweenTimer > 0) return;

        // Tween to opposite state
        // Only tween when states differ
        if (isActivated)
        {
            // Tween to original state
            if (activatedPosition != inactivePos)
                TweenManager.CreateTween(transform, TweenType.Translation, activatedPosition, tweenTime, tweenShape, null);
            if (activatedRotation != inactiveRot)
                TweenManager.CreateTween(transform, TweenType.Rotation, activatedRotation, tweenTime, tweenShape, null);
        }
        else
        {
            // Tween to activated state
            if (activatedPosition != inactivePos)
                TweenManager.CreateTween(transform, TweenType.Translation, inactivePos, tweenTime, tweenShape, null);
            if (activatedRotation != inactiveRot)
                TweenManager.CreateTween(transform, TweenType.Rotation, inactiveRot, tweenTime, tweenShape, null);
        }

        // Set tween timer and toggle active state
        tweenTimer = tweenTime;
        isActivated = !isActivated;
    }
}
