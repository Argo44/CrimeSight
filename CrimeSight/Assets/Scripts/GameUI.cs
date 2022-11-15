using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    //Fields
    public GameManager gameManagerScript;
    public GameObject pauseScreen;
    public GameObject notebook;
    public GameObject notebookHeader;
    public GameObject notebookText;
    public GameObject notification;
    public GameObject notifcationNumber;
    public GameObject crosshair;

    public int numOfNewClues = 0;
    private bool notebookIsClosed = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Disable notification bubble if no new clues are found
        if (numOfNewClues == 0)
        {
            notification.SetActive(false);
        }

        //If the notebook is closed and the game is not paused update the notification bubble with the current number of new clues
        else if(notebookIsClosed && !pauseScreen.activeInHierarchy)
        {
            notification.SetActive(true);
            TextMeshProUGUI newClue = notifcationNumber.GetComponent<TextMeshProUGUI>();
            newClue.text = numOfNewClues.ToString();
        }
    }

    public void PauseGame()
    {
        //Resumes game
        if (pauseScreen.activeInHierarchy)
        {
            //Disables pause screen UI and enables notebook
            pauseScreen.SetActive(false);
            notification.SetActive(true);
            notebook.SetActive(true);
            crosshair.SetActive(true);

            //Sets timeScale back to 1 so game can resume
            Time.timeScale = 1;

            //Change state in game manager
            GameManager.State = GameState.Game;

            //Lock mouse cursor and set state back to game
            Cursor.lockState = CursorLockMode.Locked;
        }

        else
        {
            //Enables pause screen
            pauseScreen.SetActive(true);

            //Resets notebook position for animation and disables it (I don't know why it needs these values but it works so I'm not gonna bother with it)
            notebook.transform.position = new Vector3(960, -500, 0);
            notebook.SetActive(false);
            notification.SetActive(false);
            crosshair.SetActive(false);

            //Sets time scale to 0 so game pauses
            Time.timeScale = 0f;

            //Changes game state to Menu
            GameManager.State = GameState.Menu;

            //Unlock mosue cursor and set state to menu
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("Title Screen");
    }

    //Plays animation for pulling up the notebook
    public void NotebookToggle()
    {
        if (!pauseScreen.activeInHierarchy)
        {
            numOfNewClues = 0;
            crosshair.SetActive(!notebookIsClosed);
        }
       
        if (notebook != null)
        {
            Animator animator = notebook.GetComponent<Animator>();
            if (animator != null)
            {
                bool isOpen = animator.GetBool("show");
                notebookIsClosed = isOpen;
                animator.SetBool("show", !isOpen);
            }

            if (notebookIsClosed)
            {
                //Change state in game manager
                GameManager.State = GameState.Game;

                //Lock mouse cursor and set state back to game
                Cursor.lockState = CursorLockMode.Locked;

                notebookIsClosed = true;
            }

            else
            {
                //Changes game state to Menu
                GameManager.State = GameState.Menu;

                //Unlock mosue cursor and set state to menu
                Cursor.lockState = CursorLockMode.None;

                notebookIsClosed = false;
            }
        }        
    }

    //List clues in notebook when section is opened
    public void CluesSection()
    {
        TextMeshProUGUI headerText = notebookHeader.GetComponent<TextMeshProUGUI>();
        headerText.text = "Clues";

        TextMeshProUGUI clueText = notebookText.GetComponent<TextMeshProUGUI>();
        clueText.text = "Placeholder section for found clues \n-???\n-???\n-???\n-???\n-???";
    }

    //List monster section in notebook when section is opened
    public void MonstersSection()
    {
        TextMeshProUGUI headerText = notebookHeader.GetComponent<TextMeshProUGUI>();
        headerText.text = "Monsters";

        TextMeshProUGUI clueText = notebookText.GetComponent<TextMeshProUGUI>();
        clueText.text = "Placeholder section for monster info: \n-Werewolf:\n-Vampire:\n-Santa Claus:";
    }

    //List how to play section in notebook when section is opened
    public void HowToPlaySection()
    {
        TextMeshProUGUI headerText = notebookHeader.GetComponent<TextMeshProUGUI>();
        headerText.text = "How To Play";

        TextMeshProUGUI clueText = notebookText.GetComponent<TextMeshProUGUI>();
        clueText.text = "Welcome to CrimeSight!\nIn this game you play as a detective trying to solve the cause behind the scene of the crime.\nIn this paranormal world, there are various monsters that could be behind the crime, so use your skills and abilities as a detective to find clues and deduce the true culprit!\n\nControls:\n-WASD: Move\n-Mouse: Look\n-E: Interact with nearby objects/clues\n-Space: Sight Ability (highlights nearby clues)\n-Q: Open/Close Notebook\n-Escape: Pause Game";
    }
}
