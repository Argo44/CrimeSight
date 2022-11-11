using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    private Material material;
    private Color visColor;
    private Color invisColor;
    private const float fadeRate = 1;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        GameManager.OnSight += SightFade;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SightFade()
    {
        StartCoroutine(FlashFadeCoroutine());
    }

    private IEnumerator FlashFadeCoroutine()
    {
        yield return FadeInCoroutine();
        yield return FadeOutCoroutine();
    }

    private IEnumerator FadeInCoroutine()
    {
        while (material.color.a < 1)
        {
            material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a + fadeRate * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        while (material.color.a > 0)
        {
            material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a - fadeRate * Time.deltaTime);

            yield return null;
        }
    }
}
