using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//List of all of the clue types
public enum InteractableType
{
    InactiveClue,
    ActiveClue,
}

public enum MonsterType
{
    Monster1,
    Monster2,
    Vampire,
    Ghost,
}

public class InteractableManager : MonoBehaviour
{
    //List of interactable locations and an int to track its total
    [SerializeField]
    public List<GameObject> interactableLocations = new List<GameObject>();
    private int locationNum;

    // Reference to Notebook UI
    public GameObject canvasObj;
    private GameUI uiScript;

    //Monster Type to determine what interactables should be used
    public MonsterType monsterType;

    //Prefabs For Clues
    public GameObject activeObject;
    public GameObject inactiveObject;

    public GameObject greenClue;
    public GameObject blueClue;
    public GameObject purpleClue;

    //List of prefabs 
    private List<GameObject> cluePrefab = new List<GameObject>();
    private List<ClueType> clueTypes = new List<ClueType>();

    //All of the active objects in the scene
    [SerializeField]
    public List<Clue> activeObjects = new List<Clue>();

    private int clueAmount;

    // Start is called before the first frame update
    void Start()
    {
        locationNum = interactableLocations.Count;
        uiScript = canvasObj.GetComponent<GameUI>();

        ClueTypes();
        FindActiveObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClueTypes()
    {
        //Find Monster Type
        switch(monsterType)
        {
            case MonsterType.Monster1:
                cluePrefab.Add(greenClue);
                clueTypes.Add(ClueType.Green);

                cluePrefab.Add(blueClue);
                clueTypes.Add(ClueType.Blue);

                cluePrefab.Add(greenClue);
                clueTypes.Add(ClueType.Green);

                clueAmount = 3;
                break;

            case MonsterType.Monster2:
                cluePrefab.Add(purpleClue);
                clueTypes.Add(ClueType.Purple);

                cluePrefab.Add(blueClue);
                clueTypes.Add(ClueType.Blue);

                clueAmount = 2;
                break;

            case MonsterType.Ghost:
                cluePrefab.Add(blueClue);
                clueTypes.Add(ClueType.Blue);

                clueAmount = 1;
                break;

            case MonsterType.Vampire:
                cluePrefab.Add(purpleClue);
                clueTypes.Add(ClueType.Purple);

                clueAmount = 1;
                break;

            default:
                cluePrefab.Add(greenClue);
                clueTypes.Add(ClueType.Green);

                cluePrefab.Add(blueClue);
                clueTypes.Add(ClueType.Blue);

                cluePrefab.Add(purpleClue);
                clueTypes.Add(ClueType.Purple);

                clueAmount = 3;
                break;
        }

    }

    void FindActiveObjects()
    {
        int itemNum;

        // Generate list of location IDs
        List<int> unusedLocationIDs = new List<int>();
        for (int i = 0; i < locationNum; i++)
            unusedLocationIDs.Add(i);
        
        for (int i = 0; i < clueAmount; i++)
        {
            // Pick unused ID and remove from list
            itemNum = unusedLocationIDs[Random.Range(0, unusedLocationIDs.Count)];
            unusedLocationIDs.Remove(itemNum);
         
            Debug.Log(itemNum);

            // Create new clue object at chosen location
            Clue newClue = Instantiate(cluePrefab[i], interactableLocations[itemNum].transform.position,
                     Quaternion.identity, transform).GetComponent<Clue>();

            // Add and initialize new clue
            activeObjects.Add(newClue);
            newClue.Initialize(clueTypes[i], () => { 
                uiScript.numOfNewClues++;
                activeObjects.Remove(newClue);
                newClue.gameObject.SetActive(false);
            });
        }
    }
}
