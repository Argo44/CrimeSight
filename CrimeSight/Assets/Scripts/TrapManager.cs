using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrapManager : MonoBehaviour
{
    // Fields
    public int trapCount = 0;
    public List<GameObject> trapPrefabs;
    private List<Vector3> trapLocations;
    public GameObject canvas;
    public TMP_Text[] GTtexts;
    public TMP_Text timerText;

    public Trap currentTrap;

    // Start is called before the first frame update
    void Start()
    {
        trapLocations = new List<Vector3>();
        
       
        // Then start the Quick Time Event


        // Store trap locations
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            // Skip TrapManager's transform
            if (t == transform) continue;
            trapLocations.Add(t.position);
        }

        // Randomly assign traps to locations
        List<int> indices = new List<int>();
        for (int i = 0; i < trapLocations.Count; i++)
            indices.Add(i);

        // Don't allow more traps than locations
        for (int i = 0; i < Mathf.Min(trapCount, trapLocations.Count); i++)
        {
            // Initialize new trap in unused location
            int randInt = Random.Range(0, indices.Count);
            GameObject prefab = trapPrefabs[Random.Range(0, trapPrefabs.Count)];
            Trap newTrap = Instantiate(prefab, trapLocations[indices[randInt]], prefab.transform.rotation, transform).GetComponent<Trap>();
            newTrap.gameObject.name = "Trap";
            newTrap.onDetonate = () =>
            {
                // ADD VISUAL EFFECT HERE
                
                // Deal damage to player
                GameManager.Player.TakeDamage(25f);

                // Deactivate trap
                newTrap.gameObject.SetActive(false);
            };
            indices.RemoveAt(randInt);
        }

       
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    public void ToggleQuickTimeCanvas()
    {
        canvas.SetActive(!canvas.active);
    }
    

   
}
