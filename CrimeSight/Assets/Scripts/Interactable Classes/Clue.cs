using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ClueType
{
    //BasicClue
    Green,
    Blue,
    Purple,

    //WallClue
    Painting,
    BloodSplatter,
    ClawMarks,

    //HiddenClue
    Blood,
    FootPrints,
    Fur,

    //SpecialClue
    Red,
}

[RequireComponent(typeof(AudioSource))]
public class Clue : Interactable
{
    // Fields
    private bool initialized = false;
    public ClueType type;
    private string clueName;
    private string info;
    public bool collected = false;
    private UnityAction updateClueCount;

    // Audio Data
    private AudioSource audioSrc;
    [SerializeField]
    private AudioClip clueCollectSFX;

    // Loads clue info for this object
    public void Initialize(ClueType _type, UnityAction clueCollectUpdateCallback)
    {
        type = _type;

        // Only initialize once
        if (!initialized)
        {
            initialized = true;

            //Type of clue that it is
            switch (_type)
            {
                case ClueType.Green:
                    clueName = "Green";
                    info = "This is a green clue";
                    break;

                case ClueType.Blue:
                    clueName = "Blue";
                    info = "This is a blue clue";
                    break;

                case ClueType.Purple:
                    clueName = "Purple";
                    info = "This is a purple clue";
                    break;

                case ClueType.Painting:
                    clueName = "Painting";
                    info = "This is a painting on the wall";
                    break;

                case ClueType.BloodSplatter:
                    clueName = "Blood Splatter";
                    info = "This is a blood splatter on the wall";
                    break;

                case ClueType.ClawMarks:
                    clueName = "Claw Marks";
                    info = "This is a claw mark on the wall";
                    break;

                case ClueType.Blood:
                    clueName = "Blood";
                    info = "This is a hidden blood splatter";
                    break;

                case ClueType.FootPrints:
                    clueName = "Foot Prints";
                    info = "This is a hidden track of foot prints";
                    break;

                case ClueType.Fur:
                    clueName = "Fur";
                    info = "This is a hidden piece of fur";
                    break;

                case ClueType.Red:
                    clueName = "Red";
                    info = "This is a red clue";
                    break;

                default:
                    clueName = "Default";
                    info = "Default";
                    break;
            }

            updateClueCount = clueCollectUpdateCallback;

            audioSrc = GetComponent<AudioSource>();
        }
    }

    public bool isBasicClue()
    {
        return type == ClueType.Green
            || type == ClueType.Blue
            || type == ClueType.Purple;
    }

    public bool isWallClue()
    {
        return type == ClueType.Painting
            || type == ClueType.BloodSplatter
            || type == ClueType.ClawMarks;
    }

    public bool isHiddenClue()
    {
        return type == ClueType.Blood
            || type == ClueType.FootPrints
            || type == ClueType.Fur;
    }
    public bool isSpecialClue()
    {
        return type == ClueType.Red;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnSelect()
    {
        if (!collected)
            base.OnSelect();
    }

    public override void OnInteract()
    {
        if (collected) return;
        collected = true;

        // Add clue to notebook and deactivate clue
        updateClueCount?.Invoke();

        if (clueCollectSFX != null)
            audioSrc.PlayOneShot(clueCollectSFX);

        Debug.Log("Clue collected!");
    }
}
