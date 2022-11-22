using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Traps are always Hidden Objects
[RequireComponent(typeof(HiddenObject))]
public class Trap : Interactable
{
    // Fields
    private HiddenObject hiddenObj;
    private bool isArmed = true;
    public UnityAction onDetonate;

    // Properties
    public bool IsSelectable
    {
        get { return hiddenObj.IsMarked && isArmed; }
    }

    // Start is called before the first frame update
    void Start()
    {
        hiddenObj = GetComponent<HiddenObject>();
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
        if (!IsSelectable) return;
        
        // Start disarm QTE

        Debug.Log("Trap disarmed!");
        isArmed = false;

        // Visualize deactivation of trap
        TweenManager.CreateTween(GetComponent<ParticleSystem>(), Color.green, 0.3f);
    }

    // Only select trap if it is marked with Sight
    public override void OnSelect()
    {
        if (hiddenObj.IsMarked)
            base.OnSelect();
    }

    public override void OnDeselect()
    {
        if (hiddenObj.IsMarked)
            base.OnDeselect();
    }

    /// <summary>
    /// Deals damage to player and disables this trap
    /// </summary>
    public void Detonate()
    {
        if (!isArmed) return;

        Debug.Log("A trap went off!");
        isArmed = false;

        // Deal damage to play and remove trap from scene
        onDetonate?.Invoke();
    }
}
