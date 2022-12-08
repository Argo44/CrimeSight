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
    public GameObject notification;
    public GameObject notifcationNumber;
    public GameObject crosshair;
    public GameObject sightIcon;
    public GameObject cluesSection;
    public GameObject monstersSection;
    public GameObject howToPlaySection;
    public GameObject healthUI;
    public GameObject notebookIcon;
    public GameObject interactText;
    public GameObject quicktimePanel;

    public GameObject keys;
    public GameObject keyNumber;
    public int keyTotal; 

    public int numOfNewClues = 0;
    private bool notebookIsClosed = true;
    public bool inTrapQTE = false;
    public bool isPaused = false;

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

        //Updates number of keys
        if (keyTotal > 0)
        {
            //Activate number of keys in UI
        }
        else if(keyTotal == 0) {
            //Set icon to red
        }

        //If the notebook is closed and the game is not paused update the notification bubble with the current number of new clues
        else if(notebookIsClosed && !pauseScreen.activeInHierarchy)
        {
            notification.SetActive(true);
            TextMeshProUGUI newClue = notifcationNumber.GetComponent<TextMeshProUGUI>();
            newClue.text = numOfNewClues.ToString();
        }

        isPaused = pauseScreen.activeInHierarchy;
    }

    public void PauseGame()
    {
        //Resumes game
        if (isPaused)
        {
            //Disables pause screen UI and enables in-game UI
            pauseScreen.SetActive(false);
            notification.SetActive(true);
            notebook.SetActive(true);
            crosshair.SetActive(true);
            sightIcon.SetActive(true);
            healthUI.SetActive(true);
            notebookIcon.SetActive(true);
            quicktimePanel.SetActive(inTrapQTE);

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
            notebookIsClosed = true;
            notebook.SetActive(false);
            notification.SetActive(false);
            crosshair.SetActive(false);
            sightIcon.SetActive(false);
            healthUI.SetActive(false);
            notebookIcon.SetActive(false);
            interactText.SetActive(false);
            quicktimePanel.SetActive(false);

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
        if (!isPaused)
        {
            numOfNewClues = 0;
            crosshair.SetActive(!notebookIsClosed);
            sightIcon.SetActive(!notebookIsClosed);
            healthUI.SetActive(!notebookIsClosed);
            notebookIcon.SetActive(!notebookIsClosed);
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

                CluesSection();

                notebookIsClosed = true;

                SFXPlayer.Play(SFX.NotebookClose);
            }

            else
            {
                //Changes game state to Menu
                GameManager.State = GameState.Menu;

                //Unlock mosue cursor and set state to menu
                Cursor.lockState = CursorLockMode.None;

                notebookIsClosed = false;

                SFXPlayer.Play(SFX.NotebookOpen);
            }
        }        
    }

    //List clues in notebook when section is opened
    public void CluesSection()
    {
        cluesSection.SetActive(true);
        monstersSection.SetActive(false);
        howToPlaySection.SetActive(false);

        TextMeshProUGUI headerText = notebookHeader.GetComponent<TextMeshProUGUI>();
        headerText.text = "Clues";
        SFXPlayer.Play(SFX.NotebookTabSwitch);
    }

    //List monster section in notebook when section is opened
    public void MonstersSection()
    {
        cluesSection.SetActive(false);
        monstersSection.SetActive(true);
        howToPlaySection.SetActive(false);

        TextMeshProUGUI headerText = notebookHeader.GetComponent<TextMeshProUGUI>();
        headerText.text = "Monsters";
        SFXPlayer.Play(SFX.NotebookTabSwitch);
    }

    //List how to play section in notebook when section is opened
    public void HowToPlaySection()
    {
        cluesSection.SetActive(false);
        monstersSection.SetActive(false);
        howToPlaySection.SetActive(true);

        TextMeshProUGUI headerText = notebookHeader.GetComponent<TextMeshProUGUI>();
        headerText.text = "How To Play";
        SFXPlayer.Play(SFX.NotebookTabSwitch);
    }
}
