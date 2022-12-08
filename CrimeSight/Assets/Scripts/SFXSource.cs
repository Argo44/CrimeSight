using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    SelectObject,
    FlashlightOn,
    FlashlightOff,
    SightActivation,
    NotebookOpen,
    NotebookTabSwitch,
    NotebookClose
}

public class SFXSource : MonoBehaviour
{
    // Fields
    private Dictionary<SFX, AudioClip> library;
    [SerializeField] private AudioClip selectObjectSFX;
    [SerializeField] private AudioClip flashlightOnSFX;
    [SerializeField] private AudioClip flashlightOffSFX;
    [SerializeField] private AudioClip sightActivateSFX;
    [SerializeField] private AudioClip notebookOpenSFX;
    [SerializeField] private AudioClip notebookTabSwitchSFX;
    [SerializeField] private AudioClip notebookCloseSFX;

    public Dictionary<SFX, AudioClip> Library => library;

    void Start()
    {
        library = new Dictionary<SFX, AudioClip>();
        library[SFX.SelectObject] = selectObjectSFX;
        library[SFX.FlashlightOn] = flashlightOnSFX;
        library[SFX.FlashlightOff] = flashlightOffSFX;
        library[SFX.SightActivation] = sightActivateSFX;
        library[SFX.NotebookOpen] = notebookOpenSFX;
        library[SFX.NotebookTabSwitch] = notebookTabSwitchSFX;
        library[SFX.NotebookClose] = notebookCloseSFX;
    }
}
