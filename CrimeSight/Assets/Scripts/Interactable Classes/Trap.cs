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

    private Queue<KeyCode> keyOrder;
    private List<KeyCode> keys;



    // Properties
    public bool IsSelectable
    {
        get { return hiddenObj.IsMarked && isArmed; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Stores keys in order to be pressed
        keyOrder = new Queue<KeyCode>();

        // Stores list of chosen keys to use
        keys = new List<KeyCode>() {
            KeyCode.T,
            KeyCode.G,
            KeyCode.H,
            KeyCode.Y,
            KeyCode.X,
            KeyCode.C
        };

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
           // QuickTime();
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
        int rand;

        keyOrder.Clear();

        // Create a list that represents unused indices of key List
        List<int> indices = new List<int>();
        for (int i = 0; i < 6; i++)
            indices.Add(i);

        // Randomly pick a number from unused index List, 
        // place the corresponding key in queue,
        // then remove that number from the unused index List
        for (int i = 0; i < 6; i++)
        {
            int randInt = Random.Range(0, indices.Count);
            keyOrder.Enqueue(keys[indices[randInt]]);
            indices.RemoveAt(randInt);
        }

        // Set all the texts to different keys
        for (int i = 0; i < 6; i++)
        {
            rand = Random.Range(0, 6);

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


        timer = 5.0f;
    }

    // Updates the QTEs every second 
    void QuickTime(KeyCode key)
    {
        timer -= Time.deltaTime;
        trapManager.timerText.text = timer.ToString("F2") + "s";

        

        if (key == keyOrder.Peek())
        {
            keyOrder.Dequeue();
        }


         if (keyOrder.Count == 0)
        {
            Debug.Log("Trap disarmed!");
            isArmed = false;
            
            // Visualize deactivation of trap
            TweenManager.CreateTween(GetComponent<ParticleSystem>(), Color.green, 0.3f, () => {
                Color semigreen = Color.green;
                semigreen.a = 0.05f;
                TweenManager.CreateTween(GetComponent<ParticleSystem>(), semigreen, 0.3f);
           });
           // break;
        }

        // If time expires, detonate trap
        if (timer <= 0)
        {
            trapManager.ToggleQuickTimeCanvas();
            disarming = false;
            Detonate();
        }
    }

    void OnPressC()
    {
        Debug.Log("pressed C");
    }

}
