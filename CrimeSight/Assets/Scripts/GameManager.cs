using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
    static private Interactable selectedObject;
    private const float MAX_VIEW_DIST = 3.5f;

    // Sight Data
    static public event SightEventHandler OnSight;
    private PostProcessVolume sightCamEffect;
    static private float sightTimer = 0;

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


    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Game;
        sightCamEffect = Camera.main.GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        // Select object centered in front of camera
        RaySelect();

        // Update sight post-process effect
        SightPostProcessing();

        // Update any tweening objects
        TweenManager.UpdateTweens();
    }

    /// <summary>
    /// Uses a ray from the middle of the camera to select/deselect any interactable objects
    /// </summary>
    private void RaySelect()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        
        if (Physics.Raycast(ray, out hit))
        {
            Transform hitObj = hit.transform;

            if (hit.distance > MAX_VIEW_DIST) // If hit is too far, deselect object
            {
                if (selectedObject != null)
                {
                    selectedObject.OnDeselect();
                    selectedObject = null;
                }
            }
            else if (hitObj.GetComponent<Interactable>() != null) // If Interactable object is hit, check for selection
            {
                // If new object was hit, deselect current object
                if (selectedObject != null && selectedObject.transform != hitObj)
                {
                    selectedObject.OnDeselect();
                    selectedObject = null;
                }

                // Select hit object if no object is selected
                if (selectedObject == null)
                {
                    selectedObject = hitObj.GetComponent<Interactable>();
                    selectedObject.OnSelect();
                }
            }
            else if (selectedObject != null) // If no Interactable hit, deselect object
            {
                selectedObject.OnDeselect();
                selectedObject = null;
            }
        }
        else if (selectedObject != null) // If no hit, deselect object
        {
            selectedObject.OnDeselect();
            selectedObject = null;
        }
    }

    private void SightPostProcessing()
    {
        // Only update if sight is active
        if (sightTimer <= 0) return;
        sightTimer -= Time.deltaTime;

        // Update post-process weight
        // Creates transition that lingers at high values
        sightCamEffect.weight = -1 * Mathf.Pow(sightTimer - 1, 2) + 1;
    }

    public static void ActivateSight()
    {
        sightTimer = 2f;
        OnSight?.Invoke();
    }


    public static bool IsObjectVisible(GameObject obj)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        Vector3 point = obj.transform.position;

        foreach (Plane p in planes)
            if (p.GetDistanceToPoint(point) > 0) return false;

        return true;
    }
}
