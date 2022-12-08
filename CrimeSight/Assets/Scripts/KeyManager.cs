using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    //List of interactable locations and an int to track its total
    [SerializeField]
    public List<GameObject> keyLocations = new List<GameObject>();
    private int locationNum;

    public GameObject keyPrefab;

    // Reference to Notebook UI
    public GameObject canvasObj;
    //private GameUI uiScript;

    [SerializeField]
    public List<Keys> activeKeys = new List<Keys>();

    private int keyAmount;

    // Start is called before the first frame update
    void Start()
    {
        keyAmount = 3;
        locationNum = keyLocations.Count;
        //uiScript = canvasObj.GetComponent<GameUI>();

        FindActiveKeys();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindActiveKeys()
    {
        int itemNum;

        // Generate list of location IDs
        List<int> unusedLocationIDs = new List<int>();
        for (int i = 0; i < locationNum; i++)
            unusedLocationIDs.Add(i);

        for (int i = 0; i < keyAmount; i++)
        {
            // Pick unused ID and remove from list
            itemNum = unusedLocationIDs[Random.Range(0, unusedLocationIDs.Count)];
            unusedLocationIDs.Remove(itemNum);

            Debug.Log(itemNum);

            keyLocations[itemNum].SetActive(true);

            // Create new clue object at chosen location
            //Keys newKey = Instantiate(keyPrefab, keyLocations[itemNum].transform.position,
                     //Quaternion.identity, transform).GetComponent<Keys>();

/*            // Add and initialize new clue
            activeKeys.Add(newKey);
            newKey.Initialize(() => {
                //uiScript.numOfNewKeys++;
                activeKeys.Remove(newKey);
            });*/
        }
    }
}
