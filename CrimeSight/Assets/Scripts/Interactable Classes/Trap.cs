using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

// Traps are always Hidden Objects
[RequireComponent(typeof(HiddenObject))]
[RequireComponent(typeof(AudioSource))]
public class Trap : Interactable
{
    // Fields
    private HiddenObject hiddenObj;
    private bool isArmed = true;
    public UnityAction onDetonate;
    private float timer;
    private bool disarming = false;
    private TrapManager trapManager;

    // Audio Data
    private AudioSource audioSrc;
    [SerializeField] private AudioClip interactSFX;
    [SerializeField] private AudioClip disarmSFX;
    [SerializeField] private AudioClip detonateSFX;

    // Properties
    public bool IsSelectable
    {
        get { return hiddenObj.IsMarked && isArmed; }
    }

    // Start is called before the first frame update
    void Start()
    {
        hiddenObj = GetComponent<HiddenObject>();
        audioSrc = GetComponent<AudioSource>();

        trapManager = GameObject.Find("Trap Manager").GetComponent<TrapManager>();
        //timerText = GameObject.Find("QuickTime").transform.GetChild(0).GetChild(6).gameObject.GetComponent<TMP_Text>();

        // Set the texts 
        //GTtexts[0].text = transform.GetChild("GTE1");

        // Set the text for the timer
    }

    // Update is called once per frame
    void Update()
    {
        if (disarming)
        {
            QuickTime();
        }
        
    }

    // Called when a Collider intersects with this object's Collider
    // i.e. when player walks into trap
    private void OnTriggerEnter(Collider other)
    {
        if (isArmed)
            Detonate();
    }

    public override void OnInteract()
    {
        if (!IsSelectable) return;

        // Start disarm QTE
        trapManager.ToggleQuickTimeCanvas();
        disarming = true;
        Disarm();

        // Play SFX
        if (interactSFX != null)
            audioSrc.PlayOneShot(interactSFX);

        // MOVE TO END OF QTE ONCE IMPLEMENTED
        Debug.Log("Trap disarmed!");
        isArmed = false;

        // Visualize deactivation of trap
        TweenManager.CreateTween(GetComponent<ParticleSystem>(), Color.green, 0.3f);
    }

    // Only select trap if it is marked with Sight
    public override void OnSelect()
    {
        if (hiddenObj.IsMarked)
            base.OnSelect();
    }

    public override void OnDeselect()
    {
        if (hiddenObj.IsMarked)
            base.OnDeselect();
    }

    /// <summary>
    /// Deals damage to player and disables this trap
    /// </summary>
    public void Detonate()
    {
        if (!isArmed) return;

        Debug.Log("A trap went off!");
        isArmed = false;

        // Deal damage to play and remove trap from scene
        onDetonate?.Invoke();

        if (detonateSFX != null)
            audioSrc.PlayOneShot(detonateSFX);
    }

    // Sets all the starter varible for the Quick Time events
    // Resets the timer and sets the keys
    void Disarm()
    {
        int rand = Random.Range(0, 6);

        // Set all the texts to different keys
        for (int i = 0; i < 6; i++)
        {
            // Pick a random key to show
            switch (rand)
            {
                case 0:
                    trapManager.GTtexts[i].text = "G";
                    break;

                case 1:
                    trapManager.GTtexts[i].text = "H";
                    break;

                case 2:
                    trapManager.GTtexts[i].text = "J";
                    break;

                case 3:
                    trapManager.GTtexts[i].text = "Y";
                    break;

                case 4:
                    trapManager.GTtexts[i].text = "T";
                    break;

                case 5:
                    trapManager.GTtexts[i].text = "R";
                    break;
            }
        }

        timer = 10.0f;
    }

    // Updates the QTEs every second 
    void QuickTime()
    {
        timer -= Time.deltaTime;
        trapManager.timerText.text = timer.ToString("F2") + "s";

        

        if (Input.GetKeyDown(KeyCode.G))
        {

        }
    }
}
