using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    private MeshRenderer renderer;
    private Material material;
    private Color visColor;
    private Color invisColor;
    public bool isMarkable = false;

    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
        material = renderer.material;
        invisColor = visColor = material.color;
        invisColor.a = 0f;
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
        // Only fade if visible to camera - STILL FADES THROUGH WALLS
        if (!renderer.isVisible) return;

        // Keep object visisble if it is markable; otherwise, fade back to invisible
        if (!isMarkable)
            TweenManager.CreateTween(material, TweenType.MatColor, visColor, 1f);
        else
        {
            TweenManager.CreateTween(material, TweenType.MatColor, visColor, 1f, () => {
                TweenManager.CreateTween(material, TweenType.MatColor, invisColor, 1f);
            });
        }
    }
}
