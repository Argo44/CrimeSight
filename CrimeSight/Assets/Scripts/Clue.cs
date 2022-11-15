using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : Interactable
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
        // Add clue to notebook
        // Remove object(?)
        Debug.Log("Clue collected!");
    }
}
