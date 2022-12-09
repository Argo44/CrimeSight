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
    public GameObject trapsSection;
    public GameObject healthUI;
    public GameObject notebookIcon;
    public GameObject interactText;
    public GameObject quicktimePanel;
    public GameObject infoText;
    public GameObject endScreen;
    public TextMeshProUGUI endTitle;
    public TextMeshProUGUI endSubtext;

    public List<GameObject> keySprites = new List<GameObject>();

    public KeyManager keyManagerScript;
    public int keyTotal; 

    public int numOfNewClues = 0;
    private bool notebookIsClosed = true;
    public bool inTrapQTE = false;
    public bool isPaused = false;
    private float infoTimer = 0;

    private static MonsterInfo selectedMonster;
    public static MonsterInfo SelectedMonster
    {
        get { return selectedMonster; }
        set { selectedMonster = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnInfoUpdate += UpdateInfoText;
        GameManager.OnEndGame += EndGameScreen;
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
        else if (notebookIsClosed && !pauseScreen.activeInHierarchy)
        {
            notification.SetActive(true);
            TextMeshProUGUI newClue = notifcationNumber.GetComponent<TextMeshProUGUI>();
            newClue.text = numOfNewClues.ToString();
        }

        keyTotal = keyManagerScript.KeysCollected();

        //Updates number of keys
        if (keyTotal > 0)
        {
            for (int i = 0; i < keyTotal; i++)
            {
                keySprites[i].SetActive(true);
            }

            for (int i = keyTotal; i < keySprites.Count; i++)
            {
                keySprites[i].SetActive(false);
            }
        }
        else if(keyTotal == 0) {
            //Set icon to red
            foreach(GameObject key in keySprites)
            {
                key.SetActive(false);
            }
        }

        isPaused = pauseScreen.activeInHierarchy;

        // Update info text duration
        if (!isPaused)
        {
            if (infoTimer >= 0)
                infoTimer -= Time.deltaTime;
            else
                infoText.SetActive(false);
        }
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
            infoText.SetActive(true);
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
            infoText.SetActive(false);
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
        trapsSection.SetActive(false);

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
        trapsSection.SetActive(false);

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
        trapsSection.SetActive(false);

        TextMeshProUGUI headerText = notebookHeader.GetComponent<TextMeshProUGUI>();
        headerText.text = "How To Play";
        SFXPlayer.Play(SFX.NotebookTabSwitch);
    }

    public void HowToPlayNextPage()
    {
        howToPlaySection.SetActive(false);
        trapsSection.SetActive(true);

        TextMeshProUGUI headerText = notebookHeader.GetComponent<TextMeshProUGUI>();
        headerText.text = "Traps";
        SFXPlayer.Play(SFX.NotebookTabSwitch);
    }

    private void UpdateInfoText(string info)
    {
        infoText.SetActive(true);
        TextMeshProUGUI text = infoText.GetComponent<TextMeshProUGUI>();
        text.text = info;
        infoTimer = 5;
    }

    private void EndGameScreen(GameEndState state)
    {
        if (state == GameEndState.PlayerDeath)
        {
            endTitle.color = Color.red;
            endTitle.text = "You Died";
            endSubtext.text = "Better keep an eye out for traps...";
        }
        else
        {
            if (InteractableManager.monster == selectedMonster.monsterType)
            {
                endTitle.color = Color.green;
                endTitle.text = "You Win";
                endSubtext.text = "You deduced the culprit's identity!";
            }
            else
            {
                endTitle.color = Color.red;
                endTitle.text = "You Lose";
                endSubtext.text = "Your guess was wrong, and the consequences were deadly...";
            }
        }

        StartCoroutine(EndScreenCorountine());

        //Enables pause screen
        notebookIsClosed = true;
        notebook.SetActive(false);
        notification.SetActive(false);
        crosshair.SetActive(false);
        sightIcon.SetActive(false);
        healthUI.SetActive(false);
        notebookIcon.SetActive(false);
        interactText.SetActive(false);
        infoText.SetActive(false);
        quicktimePanel.SetActive(false);

        //Changes game state to Menu
        GameManager.State = GameState.Menu;

        //Unlock mosue cursor and set state to menu
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator EndScreenCorountine()
    {
        RectTransform t = endScreen.GetComponent<RectTransform>();
        
        while (t.localScale.x < 1)
        {
            t.localScale += new Vector3(Time.deltaTime, Time.deltaTime);
            yield return null;
        }
    }

}
