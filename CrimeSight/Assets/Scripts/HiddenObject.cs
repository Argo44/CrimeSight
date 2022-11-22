using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    // Fields
    private bool isTrap;
    private Material material;
    private ParticleSystem pSystem;
    private Color visColor;
    private Color invisColor;
    public bool isMarkable = false;
    private bool isMarked = false;

    // Properties
    public bool IsMarked { get { return isMarked; } }

    // Start is called before the first frame update
    void Start()
    {
        // If object is Trap, use particle system instead
        isTrap = GetComponent<Trap>() != null;
        if (isTrap)
        {
            // Traps are always markable
            isMarkable = true;
            pSystem = GetComponent<ParticleSystem>();
            visColor = pSystem.main.startColor.color;
            visColor.a = 1;
        }
        else
        {
            material = gameObject.GetComponent<MeshRenderer>().material;
            invisColor = visColor = material.color;
            invisColor.a = 0f;
            visColor.a = 1;
        }

        // If object is interactable, it will stay visible once seen with Sight
        isMarkable = GetComponent<Interactable>() != null;

        // Hook activation method to Sight activation event
        GameManager.OnSight += SightFade;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SightFade()
    {
        // Only fade if visible to camera - STILL FADES THROUGH WALLS
        if (!GameManager.IsObjectVisible(gameObject)) return;
        if (isMarked) return;

        // Mark trap, making it permanently visible and disarmable
        if (isTrap)
        {
            TweenManager.CreateTween(pSystem, visColor, 1f);
            isMarked = true;
            return;
        }

        // Keep object visisble if it is markable; otherwise, fade back to invisible
        if (!isMarkable)
            TweenManager.CreateTween(material, visColor, 1f, () => {
                TweenManager.CreateTween(material, invisColor, 1f);
            }); 
        else
        {
            // Mark object during first activation
            isMarked = true;
            TweenManager.CreateTween(material, visColor, 1f);
        }
    }
}
