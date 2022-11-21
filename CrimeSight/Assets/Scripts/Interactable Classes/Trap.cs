using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HiddenObject))]
public class Trap : Interactable
{
    private bool isArmed = true;
    private bool isMarked = false;
    public UnityAction onDetonate;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if (!isArmed || !isMarked) return;
        // Start disarm QTE

        Debug.Log("Trap disarmed!");
        isArmed = false;

        // Visualize deactivation of trap
        TweenManager.CreateTween(GetComponent<ParticleSystem>(), Color.cyan, 0.3f);
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

        // Deal damage to play and remove trap from scene
        onDetonate?.Invoke();
    }
}
