using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public enum GameState
{
    Menu,
    Game,
    QTE
}

// Delegate for handling HiddenObject activation on Sight usage
public delegate void SightEventHandler();

public class GameManager : MonoBehaviour
{

    // Fields
    static private GameState state;
    [SerializeField] private StarterAssets.FirstPersonController playerController;
    static private StarterAssets.FirstPersonController _playerController;

    // Selection Data
    static private Interactable selectedObject;
    private const float MAX_VIEW_DIST = 3.5f;
    private Image crosshair;
    private TextMeshProUGUI interactText;

    // Sight FX Data
    static public event SightEventHandler OnSight;
    private PostProcessVolume sightFX;
    private static readonly float SIGHT_TIME = 2f;
    static private float sightTimer = 0;

    // Damage FX Data
    private PostProcessVolume damageFX;
    private static readonly float DMG_TIME = 0.5f;
    static private float damageTimer = 0;

    //Key Manager
    public KeyManager keyManager;

    // Properties
    static public GameState State
    {
        get { return state; }
        set { state = value; }
    }
    static public Interactable SelectedObject
    {
        get { return selectedObject; }
        // GameManager is in charge of what the selected object is, so no set accessor
    }
    static public StarterAssets.FirstPersonController Player
    {
        get { return _playerController; }
    }

    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Game;
        _playerController = playerController;
        PostProcessVolume[] fxArray = Camera.main.GetComponents<PostProcessVolume>();
        sightFX = fxArray[0];
        damageFX = fxArray[1];
        SFXPlayer.Initialize();

        // Get selection UI references
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        interactText = GameObject.Find("Interact Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Select object centered in front of camera
        UpdateSelection();

        // Update post-process effects
        PostProcessUpdate(sightFX, ref sightTimer, SIGHT_TIME);
        PostProcessUpdate(damageFX, ref damageTimer, DMG_TIME);

        // Update any tweening objects
        TweenManager.UpdateTweens();
    }

    /// <summary>
    /// Uses a ray from the middle of the camera to select/deselect any interactable objects
    /// </summary>
    private void UpdateSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Interactable hitInteractable = hit.transform.GetComponent<Interactable>();

            if (hit.distance > MAX_VIEW_DIST) // If hit is too far, deselect object
            {
                Deselect();
            }
            else if (hitInteractable != null) // If Interactable object is hit, check for selection
            {
                // If new object was hit, select it
                SelectObject(hitInteractable);
            }
            else if (selectedObject != null) // If no Interactable hit, deselect object
                Deselect();
        }
        else if (selectedObject != null) // If no hit, deselect object
            Deselect();
    }

    private void SelectObject(Interactable obj)
    {
        // Cast obj as Interactable types to determine select conditions
        Trap trap = null;
        if (obj is Trap t) trap = t;
        Clue clue = null;
        if (obj is Clue c) clue = c;

        // Unmarked/unarmed traps & collected clues are not selectable
        bool nonselectable = (trap != null && !trap.IsSelectable) || (clue != null && clue.collected);

        // Deselect any current object
        if (selectedObject == obj)
        {
            // Deselect current obj if it becomes unselectable
            if (nonselectable)
                Deselect();

            // No need to reselect current object
            return;
        }
        else 
            Deselect();

        // Do not select object if it is hidden
        if (obj.GetComponent<HiddenObject>() != null && !obj.GetComponent<HiddenObject>().IsMarked) 
            return;

        // Do not select unselectable objects
        if (nonselectable) return;

        // Select object
        selectedObject = obj;
        obj.OnSelect();

        bool tempLock = false;

        // Update selection UI
        // Set interaction text by type
        if (obj is Trap)
            interactText.text = "Disarm " + obj.name;
        else if (obj is Clue)
            interactText.text = "Collect " + obj.name;
        else if (obj is ActionObject)
        {
            ActionObject tempObj = obj as ActionObject;
            if (tempObj.IsLocked())
            {
                if (keyManager.KeysCollected() > 0)
                    interactText.text = "Use key to unlock";
                else
                {
                    interactText.text = obj.name + " is locked";
                    tempLock = true;
                }
            } 
            else
                interactText.text = "Open " + obj.name;
        }
        else
            interactText.text = "Use " + obj.name;

        if(!tempLock)
            interactText.text += " (E)";

        interactText.gameObject.SetActive(true);
        crosshair.color = Color.red;

        // Play selection SFX
        SFXPlayer.Play(SFX.SelectObject);
    }

    private void Deselect()
    {
        if (selectedObject != null)
        {
            selectedObject.OnDeselect();
            selectedObject = null;
        }

        // Update selection UI
        crosshair.color = Color.white;
        interactText.gameObject.SetActive(false);
    }

    private void PostProcessUpdate(PostProcessVolume fx, ref float timer, float time)
    {
        // Only update if effect is active
        if (timer <= 0) return;
        timer -= Time.deltaTime;

        // Parabolical change in weight over duration
        fx.weight = -Mathf.Pow(2f / time * timer - 1, 2) + 1;
    }

    public static void ActivateSight()
    {
        sightTimer = SIGHT_TIME;
        OnSight?.Invoke();
        SFXPlayer.Play(SFX.SightActivation);
    }

    public static void ActivateDamageFX()
    {
        damageTimer = DMG_TIME;
        // SFXPlayer.Play(SFX.PlayerDamage);
    }

    // Determines if an object is visible to main camera (in view and unobstructed)
    // NOTE: uses position, so technically edge parts may be visible but be marked as not. Maybe raycast each corner?
    public static bool IsObjectVisible(GameObject obj)
    {
        // Frustrum planes act like a 'box' of what the camera can see
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        Vector3 point = obj.transform.position;

        // If object is behind any plane, it is not in view
        foreach (Plane p in planes)
            if (p.GetDistanceToPoint(point) < 0) return false;
        
        // If object is in view, use raycast to see if it is unobstructed
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(obj.transform.position));

        // If object hit by raycast matches param object, return true
        return (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.gameObject == obj);
    }
}
