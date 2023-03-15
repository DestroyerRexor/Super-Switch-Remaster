using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Loader.Scene restartScene;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;

    private bool isGamePaused = false;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        resumeButton.onClick.AddListener(() =>
        {
            TogglePauseGame();
        });

        restartButton.onClick.AddListener(() =>
        {
            RestartScene();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            GoToMainMenuScene();
        });
    }

    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(() =>
        {
            TogglePauseGame();
        });

        restartButton.onClick.RemoveListener(() =>
        {
            RestartScene();
        });

        mainMenuButton.onClick.RemoveListener(() =>
        {
            GoToMainMenuScene();
        });
    }

    private void Timer_OnTimeOver(object sender, EventArgs e)
    {
        RestartScene();
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            IsPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
            IsPaused = false;
        }

    }

    public void SetPause(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed && true)
        {
            TogglePauseGame();
        }
    }

    public void SetRestart(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed && true)
        {
            RestartScene();
        }
    }

    public void RestartScene()
    {
        if (Time.timeScale == 0)
        {
            TogglePauseGame();
        }
        Loader.Load(restartScene);
    }

    private void GoToMainMenuScene()
    {
        TogglePauseGame();
        Loader.Load(Loader.Scene.Main_Menu_Scene);
        PlayerConfigurationManager.Instance.DestroyObject();
    }

}
