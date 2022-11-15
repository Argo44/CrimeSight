using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public GameObject howToPlayPanel;

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
        SceneManager.LoadScene("TestRoom");
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
