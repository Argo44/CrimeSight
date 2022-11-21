using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    // Fields
    public int trapCount = 0;
    public GameObject trapPrefab;
    private List<Vector3> trapLocations;

    // Start is called before the first frame update
    void Start()
    {
        trapLocations = new List<Vector3>();

        // Store trap locations
        foreach (Transform t in GetComponentsInChildren<Transform>())
            trapLocations.Add(t.position);

        // Randomly assign traps to locations
        List<int> indices = new List<int>();
        for (int i = 0; i < trapLocations.Count; i++)
            indices.Add(i);

        // Don't allow more traps than locations
        for (int i = 0; i < Mathf.Min(trapCount, trapLocations.Count); i++)
        {
            // Initialize new trap in unused location
            int randInt = Random.Range(0, indices.Count);
            Trap newTrap = Instantiate(trapPrefab, trapLocations[indices[randInt]], trapPrefab.transform.rotation, transform).GetComponent<Trap>();
            newTrap.gameObject.name = "Trap";
            newTrap.onDetonate = () =>
            {
                // ADD VISUAL EFFECT HERE

                newTrap.gameObject.SetActive(false);
            };
            indices.RemoveAt(randInt);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
