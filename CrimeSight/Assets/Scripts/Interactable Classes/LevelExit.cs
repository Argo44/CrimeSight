using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : Interactable
{
    public InteractableManager clueManager;

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
        if (clueManager.collectedClues == 0)
            GameManager.UpdateInfo("I can't leave yet, I haven't gotten any information on the culprit.");
        else if (GameUI.SelectedMonster == null)
            GameManager.UpdateInfo("I need to decide who the culprit is by marking them in my notebook.");
        else
            GameManager.EndGame(GameEndState.LevelExit);
    }
}
