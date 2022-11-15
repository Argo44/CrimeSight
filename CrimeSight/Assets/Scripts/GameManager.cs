using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Menu,
    Game
}

public class GameManager : MonoBehaviour
{

    // Fields
    static private GameState state;
    private Interactable selectedObject;

    public delegate void SightEventHandler();
    static public event SightEventHandler OnSight;

    // Properties
    static public GameState State
    {
        get { return state; }
        set { state = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Game;
    }

    // Update is called once per frame
    void Update()
    {
        RaySelect();

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
            Debug.Log("Hit!");
            Transform hitObj = hit.transform;
            // Only select Interactable objects
            if (hitObj.GetComponent<Interactable>() != null)
            {
                Debug.Log(hitObj.gameObject);

                // If new object was hit, deselect current object
                if (selectedObject != hitObj && selectedObject != null)
                {
                    selectedObject.OnDeselect();
                    selectedObject = null;
                }

                // Select hit object if no object is selected
                if (selectedObject == null)
                {
                    selectedObject = hitObj.GetComponent<Interactable>();
                    selectedObject.OnSelect();
                    Debug.Log("Object selected!");
                }
            }
        }
        else if (selectedObject != null) // If no hit, deselect object
        {
            selectedObject.OnDeselect();
            selectedObject = null;
        }
    }

    public static void ActivateSight()
    {
        OnSight?.Invoke();
    }
}
