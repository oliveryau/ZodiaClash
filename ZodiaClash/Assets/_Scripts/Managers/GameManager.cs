using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    WAIT, PLAY, PAUSE, SETTINGS, INFO, LOSE
}

public class GameManager : MonoBehaviour
{
    public GameState gameState;

    [Header("HUD")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject effectsScreen;
    [SerializeField] private GameObject loseScreen;

    [SerializeField] private bool isPaused;
    [SerializeField] private bool isSettingsDisplayed;
    [SerializeField] private bool isEffectDisplayed;
    [HideInInspector] public bool lostBattle;

    private ScenesManager scenesManager;

    private void Start()
    {
        scenesManager = FindObjectOfType<ScenesManager>();

        gameState = GameState.WAIT;

        StartCoroutine(StartDelay());
    }

    private void Update()
    {
        if (gameState == GameState.PLAY) //normal play state
        {
            Time.timeScale = 1f;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = true;
            }

            if (isPaused)
            {
                pauseScreen.SetActive(true);

                gameState = GameState.PAUSE;
            }

            if (isEffectDisplayed)
            {
                effectsScreen.SetActive(true);

                gameState = GameState.INFO;
            }

            if (lostBattle)
            {
                loseScreen.SetActive(true);

                gameState = GameState.LOSE;
            }
        }

        else if (gameState == GameState.PAUSE) //pause screen
        {
            Time.timeScale = 0f;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = false;
            }

            if (isSettingsDisplayed)
            {
                settingsScreen.SetActive(true);

                gameState = GameState.SETTINGS;
            }

            if (!isPaused)
            {
                pauseScreen.SetActive(false);

                gameState = GameState.PLAY;
            }
        }

        else if (gameState == GameState.SETTINGS)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isSettingsDisplayed = false;
            }

            if (!isSettingsDisplayed)
            {
                settingsScreen.SetActive(false);

                gameState = GameState.PAUSE;
            }
        }

        else if (gameState == GameState.INFO) //effects screen
        {
            Time.timeScale = 0f;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isEffectDisplayed = false;
            }

            if (!isEffectDisplayed)
            {
                effectsScreen.SetActive(false);

                gameState = GameState.PLAY;
            }
        }

        else if (gameState == GameState.LOSE) //lose screen
        {
            Time.timeScale = 0f;
        }
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1f);

        gameState = GameState.PLAY;
    }

    #region Pause/Lose Screen Buttons
    public void ResumeGame()
    {
        isPaused = false;
    }

    public void RestartBattle()
    {
        Time.timeScale = 1f;
        gameState = GameState.WAIT;

        StartCoroutine(scenesManager.LoadLevel());
    }

    public void ShowSettings()
    {
        isSettingsDisplayed = true;
    }

    public void HideSettings()
    {
        isSettingsDisplayed = false;
    }

    public void ExitBattle()
    {
        Time.timeScale = 1f;
        gameState = GameState.WAIT;

        StartCoroutine(scenesManager.LoadMap());
    }

    public void ExitMenu()
    {
        Time.timeScale = 1f;
        gameState = GameState.WAIT;

        StartCoroutine(scenesManager.LoadMenu());
    }
    #endregion

    #region Info Screen Buttons
    public void ShowEffectsInfo()
    {
        isEffectDisplayed = true;
    }

    public void HideEffectsInfo()
    {
        isEffectDisplayed = false;
    }
    #endregion
}
