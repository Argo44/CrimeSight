using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinkPoint : MonoBehaviour
{
    // Fields
    private Collider collider;
    private bool activated = false;
    [SerializeField] private string info;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        activated = true;

        GameManager.UpdateInfo(info);
    }
}
