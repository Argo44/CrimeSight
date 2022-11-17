using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    private Material material;
    private Color visColor;
    private Color invisColor;
    private bool isMarkable;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        invisColor = visColor = material.color;
        invisColor.a = 0.5f;
        visColor.a = 1;

        // If object is interactable, it will stay visible once seen with Sight
        isMarkable = GetComponent<Interactable>() != null;

        GameManager.OnSight += SightFade;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SightFade()
    {
        // Only fade if visible to camera - NOT WORKING YET
        //if (!GameManager.IsObjectVisible(gameObject)) return;

        TweenManager.CreateTween(material, TweenType.MatColor, visColor, 1f, () => {
            if (!isMarkable)
                TweenManager.CreateTween(material, TweenType.MatColor, invisColor, 1f);
            });
    }
}
