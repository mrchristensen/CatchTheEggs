using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject player;
    public Spawner gameController;
    public ScoreManager scoreManager;

    public GameObject score;
    public GameObject classicButton;
    public GameObject endlessButton;
    public GameObject titleQuitButton;
    public GameObject easyButton;
    public GameObject normalButton;
    public GameObject hardButton;
    public GameObject pauseButton;
    public GameObject pausedPrompt;
    public GameObject midroundResumeButton;
    public GameObject midroundQuitButton;
    public GameObject quitPrompt; //Quit the round
    public GameObject closePrompt; //Close the application/game exe
    public GameObject yesButton;
    public GameObject noButton;
    string state = "MainMenu";
    string oldState;
    AudioSource click;

    private void Start()
    {
        click = GetComponent<AudioSource>();
    }

    public void MainMenuGUI()
    {
        state = "MainMenu";
        click.Play();
        pauseButton.SetActive(false);
        classicButton.SetActive(true);
        endlessButton.SetActive(true);
        titleQuitButton.SetActive(true);
        midroundResumeButton.SetActive(false);
        midroundQuitButton.SetActive(false);
        easyButton.SetActive(false);
        normalButton.SetActive(false);
        hardButton.SetActive(false);
        pausedPrompt.SetActive(false);
        quitPrompt.SetActive(false);
        closePrompt.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
    }

    public void SelectDifficultyGUI()
    {
        state = "SelectDifficulty";
        click.Play();
        classicButton.SetActive(false);
        endlessButton.SetActive(false);
        titleQuitButton.SetActive(false);
        easyButton.SetActive(true);
        normalButton.SetActive(true);
        hardButton.SetActive(true);
    }

    public void GameGUI()
    {
        state = "Game";
        click.Play();
        easyButton.SetActive(false);
        normalButton.SetActive(false);
        hardButton.SetActive(false);
        pauseButton.SetActive(true);
        score.SetActive(true);
        pausedPrompt.SetActive(false);
        midroundResumeButton.SetActive(false);
        midroundQuitButton.SetActive(false);
        quitPrompt.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        pauseButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "II";
    }

    public void PausedGUI()
    {
        state = "Paused";
        click.Play();
        pauseButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "►";
        pausedPrompt.SetActive(true);
        midroundResumeButton.SetActive(true);
        midroundQuitButton.SetActive(true);
        quitPrompt.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
    }

    public void QuitPromptGUI()
    {
        state = "QuitPrompt";
        click.Play();
        quitPrompt.SetActive(true);
        yesButton.SetActive(true);
        noButton.SetActive(true);
        midroundResumeButton.SetActive(false);
        midroundQuitButton.SetActive(false);
    }
    public void ClosePromptGUI()
    {
        state = "ClosePrompt";
        click.Play();
        closePrompt.SetActive(true);
        yesButton.SetActive(true);
        noButton.SetActive(true);
        classicButton.SetActive(false);
        endlessButton.SetActive(false);
        titleQuitButton.SetActive(false);
    }

    public void TogglePause()
    {

        //Resume the game
        if (Time.timeScale == 0)
        {
            GameGUI();
            Time.timeScale = 1;
            player.GetComponent<Player>().enabled = true;
        }
        //Pause the game
        else
        {
            PausedGUI();
            player.GetComponent<Player>().Disable();
            Time.timeScale = 0;

        }
    }

    public void Yes()
    {
        if(state == "QuitPrompt")
        {
            gameController.QuitGame();
        }
        else //if (state == "ClosePrompt")_
        {
            Application.Quit();
        }
    }

    public void No()
    {
        if (state == "QuitPrompt")
        {
            PausedGUI();
        }
        else //if (state == "ClosePrompt")_
        {
            MainMenuGUI();
        }
    }

    public void ResultsUI()
    {
        //todo flush this out
        scoreManager.UpdateResultsDisplay();
    }

    void Update()
    {
        //Back button handling
        if (Input.GetKeyDown(KeyCode.Escape))
            switch (state)
            {
                case "MainMenu":
                    ClosePromptGUI();
                    break;
                case "SelectDifficulty":
                    MainMenuGUI();
                    break;
                case "Game":
                    TogglePause();
                    break;
                case "Paused":
                    QuitPromptGUI();
                    break;
                case "QuitPrompt":
                    gameController.QuitGame();
                    break;
                case "ClosePrompt":
                    Application.Quit();
                    break;
                default:
                    Application.Quit();
                    break;
            }

        //Debugger to know what state we're in
        if(state != oldState)
        {
            Debug.Log("New state: " + state);
            oldState = state;
        }
    }

}
