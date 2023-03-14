using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public static MenuUI Instance { get; private set; }

    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    private void Awake()
    {
        Instance = this;

        Time.timeScale = 1f;

    }

    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.Level_1);
        });

        settingsButton.onClick.AddListener(() =>
        {
            Hide();
            SettingsUI.Instance.Show();
        });

        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Show();

    }


    private void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

}
