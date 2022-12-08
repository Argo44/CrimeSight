using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

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
    private int clueNumber;
    private string info;
    public bool collected = false;
    private UnityAction updateClueCount;
    public GameObject clueText;

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
                    clueNumber = 1;
                    break;

                case ClueType.Blue:
                    clueName = "Blue";
                    info = "This is a blue clue";
                    clueNumber = 2;
                    break;

                case ClueType.Purple:
                    clueName = "Purple";
                    info = "This is a purple clue";
                    clueNumber = 3;
                    break;

                case ClueType.Painting:
                    clueName = "Painting";
                    info = "This is a painting on the wall";
                    clueNumber = 4;
                    break;

                case ClueType.BloodSplatter:
                    clueName = "Blood Splatter";
                    info = "This is a blood splatter on the wall";
                    clueNumber = 5;
                    break;

                case ClueType.ClawMarks:
                    clueName = "Claw Marks";
                    info = "This is a claw mark on the wall";
                    clueNumber = 6;
                    break;

                case ClueType.Blood:
                    clueName = "Blood";
                    info = "This is a hidden blood splatter";
                    clueNumber = 7;
                    break;

                case ClueType.FootPrints:
                    clueName = "Foot Prints";
                    info = "This is a hidden track of foot prints";
                    clueNumber = 8;
                    break;

                case ClueType.Fur:
                    clueName = "Fur";
                    info = "This is a hidden piece of fur";
                    clueNumber = 9;
                    break;

                case ClueType.Red:
                    clueName = "Red";
                    info = "This is a red clue";
                    clueNumber = 10;
                    break;

                default:
                    clueName = "Default";
                    info = "Default";
                    clueNumber = 0;
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
        string clueTextName = "Clue" + clueNumber;
        if (clueNumber != 0)
        {
            clueText = GameObject.Find(clueTextName);
            TextMeshProUGUI clueInfo = clueText.GetComponent<TextMeshProUGUI>();
            clueInfo.text = "-" + clueName + ": " + info;
        }

        if (collected) return;
        collected = true;

        // Add clue to notebook and deactivate clue
        updateClueCount?.Invoke();

        if (clueCollectSFX != null)
            audioSrc.PlayOneShot(clueCollectSFX);

        Debug.Log("Clue collected!");
    }
}
