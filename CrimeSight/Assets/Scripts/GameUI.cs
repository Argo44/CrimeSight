using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    //Fields
    public GameManager gameManagerScript;
    public GameObject pauseScreen;
    public GameObject notebook;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        //Resumes game
        if (pauseScreen.activeInHierarchy)
        {
            //Disables pause screen UI and enables notebook
            pauseScreen.SetActive(false);
            notebook.SetActive(true);

            //Sets timeScale back to 1 so game can resume
            Time.timeScale = 1;

            //Change state in game manager
            GameManager.State = GameManager.GameState.Game;

            //Lock mouse cursor and set state back to game
            Cursor.lockState = CursorLockMode.Locked;
        }

        else
        {
            //Enables pause screen
            pauseScreen.SetActive(true);

            //Resets notebook position for animation and disables it (I don't know why it needs these values but it works so I'm not gonna bother with it)
            notebook.transform.position = new Vector3(960, 32, 0);
            notebook.SetActive(false);

            //Sets time scale to 0 so game pauses
            Time.timeScale = 0f;

            //Changes game state to Menu
            GameManager.State = GameManager.GameState.Menu;

            //Unlock mosue cursor and set state to menu
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //Quits game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Plays animation for pulling up the notebook
    public void NotebookToggle()
    {
        if (notebook != null)
        {
            Animator animator = notebook.GetComponent<Animator>();
            if (animator != null)
            {
                bool isOpen = animator.GetBool("show");
                animator.SetBool("show", !isOpen);
            }
        }
    }
}
