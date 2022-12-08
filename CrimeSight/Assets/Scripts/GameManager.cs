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
            else // Update UI for current obj
                UpdateSelectionUI(obj);

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
        UpdateSelectionUI(obj);

        // Play selection SFX
        SFXPlayer.Play(SFX.SelectObject);
    }

    private void UpdateSelectionUI(Interactable obj)
    {
        // Set interaction text by type
        if (obj is Trap)
            interactText.text = "Disarm " + obj.name;
        else if (obj is Clue || obj is Keys)
            interactText.text = "Collect " + obj.name;
        else
        {
            // Object is ActionObject - check if locked
            ActionObject ao = (ActionObject)obj;
            if (ao.isLocked)
                interactText.text = "Unlock " + obj.name;
            else
                interactText.text = "Use " + obj.name;
        }

        // Update text and crosshair
        interactText.text += " (E)";
        interactText.gameObject.SetActive(true);
        crosshair.color = Color.red;
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
    public static bool IsObjectVisible(GameObject obj)
    {
        // Frustrum planes act like a 'box' of what the camera can see
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Determine bounding points of object using collider
        Collider collider = obj.GetComponent<Collider>();
        Vector3[] vertices =
        {
            obj.transform.position,
            collider.bounds.max,
            collider.bounds.max - new Vector3(collider.bounds.size.x, 0, 0),
            collider.bounds.max - new Vector3(0, collider.bounds.size.y, 0),
            collider.bounds.max - new Vector3(0, 0, collider.bounds.size.z),
            collider.bounds.min + new Vector3(collider.bounds.size.x, 0, 0),
            collider.bounds.min + new Vector3(0, collider.bounds.size.y, 0),
            collider.bounds.min + new Vector3(0, 0, collider.bounds.size.z),
            collider.bounds.min
        };

        // Determine if any bounding points are in view
        bool objInFrame = false;
        foreach (Vector3 v in vertices)
        {
            bool pointInFrame = true;

            // If point is behind any plane, not in view
            foreach (Plane p in planes)
            {
                pointInFrame = p.GetDistanceToPoint(v) >= 0;
                if (!pointInFrame) break;
            }

            // If any bounding point is in view, object is in view
            objInFrame = pointInFrame;
            if (objInFrame) break;
        }

        // If no bounding points in view, object not in view
        if (!objInFrame) return false;

        // If object is in view, use raycast to see if it is unobstructed
        foreach (Vector3 v in vertices)
        {
            Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(v));

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.gameObject == obj)
                return true;
        }

        //Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(obj.transform.position));

        // If object hit by raycast matches param object, return true
        //return (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.gameObject == obj);

        return false;
    }
}
