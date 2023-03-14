using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    private const string PLAYER_PREFS_MASTER_VOLUME = "MasterVolume";

    public static SettingsUI Instance { get; private set; }

    [SerializeField] private Button returnButton;
    [SerializeField] private Button startButton;

    [Header("Volume Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Button masterVolumeDecreaseButton;
    [SerializeField] private Button masterVolumeIncreaseButton;
    [SerializeField] private Button musicVolumeDecreaseButton;
    [SerializeField] private Button musicVolumeIncreaseButton;
    [SerializeField] private Button sfxVolumeDecreaseButton;
    [SerializeField] private Button sfxVolumeIncreaseButton;
    [SerializeField] private Image masterVolumeSlider;
    [SerializeField] private Image musicVolumeSlider;
    [SerializeField] private Image sfxVolumeSlider;

    private float masterVolume;

    private void Awake()
    {
        Instance = this;

        masterVolume = GetVolume();

        SetVolume();

    }


    private void Start()
    {
        returnButton.onClick.AddListener(() =>
        {
            Hide();
            MenuUI.Instance.Show();
        });

        masterVolumeIncreaseButton.onClick.AddListener(() =>
        {
            IncreaseVolume();
            UpdateVolume();
        });

        masterVolumeDecreaseButton.onClick.AddListener(() =>
        {
            DecreaseVolume();
            UpdateVolume();
        });

        musicVolumeIncreaseButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.IncreaseVolume();
            UpdateVolume();
        });

        musicVolumeDecreaseButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.DecreaseVolume();
            UpdateVolume();
        });

        sfxVolumeIncreaseButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.IncreaseVolume();
            UpdateVolume();
        });

        sfxVolumeDecreaseButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.DecreaseVolume();
            UpdateVolume();
        });

        UpdateVolume();

        Hide();

    }

    private void UpdateVolume()
    {
        masterVolumeSlider.fillAmount = GetVolume();
        sfxVolumeSlider.fillAmount = SoundManager.Instance.GetVolume();
        musicVolumeSlider.fillAmount = MusicManager.Instance.GetVolume();
    }

    public void IncreaseVolume()
    {
        masterVolume += 0.1f;
        if (masterVolume > 1f)
        {
            masterVolume = 1f;
        }

        SetVolume();

        PlayerPrefs.SetFloat(PLAYER_PREFS_MASTER_VOLUME, masterVolume);
        PlayerPrefs.Save();

    }

    public void DecreaseVolume()
    {
        masterVolume -= 0.1f;
        if (masterVolume < 0f)
        {
            masterVolume = 0.0001f;
        }

        SetVolume();

        PlayerPrefs.SetFloat(PLAYER_PREFS_MASTER_VOLUME, masterVolume);
        PlayerPrefs.Save();
    }

    private float GetVolume()
    {
        masterVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MASTER_VOLUME, 0.5f);
        SetVolume();

        return masterVolume;
    }

    private void SetVolume()
    {
        audioMixer.SetFloat("Master", Mathf.Log10(masterVolume) * 20);
    }

    private void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        startButton.Select();
    }

    public void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        masterVolumeDecreaseButton.Select();
    }

}
