using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInteract()
    {
        // Start disarm QTE
        Debug.Log("Trap disarmed!");
    }

    /// <summary>
    /// Deals damage to player and disables this trap
    /// </summary>
    public void OnDetonate()
    {
        Debug.Log("A trap went off!");
    }
}
