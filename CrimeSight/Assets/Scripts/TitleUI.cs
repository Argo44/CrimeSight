using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public GameObject howToPlayPanel;
    public GameObject creditsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("HouseLayout");
    }

    public void HowToPlay()
    {
        if (howToPlayPanel.activeInHierarchy)
        {
            howToPlayPanel.SetActive(false);
        }

        else
        {
            howToPlayPanel.SetActive(true);
        }
    }

    public void Credits()
    {
        if (creditsPanel.activeInHierarchy)
        {
            creditsPanel.SetActive(false);
        }

        else
        {
            creditsPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
