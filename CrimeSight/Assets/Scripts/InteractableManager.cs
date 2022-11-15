using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//List of all of the clue types
public enum InteractableType
{
    InactiveClue,
    ActiveClue,
}

public class InteractableManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> interactableLocations = new List<GameObject>();

    public GameObject activeObject;
    public GameObject inactiveObject;

    [SerializeField]
    public List<Interactable> activeObjects = new List<Interactable>();

    private int[] chosenNumbers;
    private int clueAmount = 2;

    // Start is called before the first frame update
    void Start()
    {
        FindActiveObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindActiveObjects()
    {
        bool isUsed = false;
        int itemNum = -1;
        
        for (int i = 0; i < clueAmount; i++)
        {
            if (chosenNumbers != null)
            { 
                do
                {
                    int randomNumber = Random.Range(0, 5);
                    for (int j = 0; j < chosenNumbers.Length; j++)
                    {
                        if (randomNumber == chosenNumbers[i])
                        {
                            isUsed = true;
                        }
                    }

                    if (!isUsed)
                    {
                        itemNum = randomNumber;

                    }

                    Debug.Log(randomNumber);

                } while (isUsed == true);

                Debug.Log("Adding New Object");

                Interactable newInteractable = Instantiate(activeObject, interactableLocations[itemNum].transform.position,
                         Quaternion.identity, transform).GetComponent<Interactable>();
                activeObjects.Add(newInteractable);
            }
            else
            {
                itemNum = Random.Range(0, 5);
                Interactable newInteractable = Instantiate(activeObject, interactableLocations[itemNum].transform.position,
                      Quaternion.identity, transform).GetComponent<Interactable>();
                activeObjects.Add(newInteractable);
            }
        }



        Debug.Log("Active Objects has been filled");

/*        for (int i = 0; i < clueAmount; i++)
        {
            Debug.Log(activeObjects[i]);
            activeObjects[i].Initialize();
        }*/


    }
}
