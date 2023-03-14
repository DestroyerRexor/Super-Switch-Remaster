using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource audioSourceMainMenu;
    [SerializeField] private AudioSource audioSourceInGame;
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip inGameMusic;
    private float volume = 0.3f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        volume = GetVolume();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            audioSourceMainMenu.enabled = true;
            audioSourceInGame.enabled = false;
        }
        else
        {
            audioSourceInGame.enabled = true;
            audioSourceMainMenu.enabled = false;
        }
    }

    public void IncreaseVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 1f;
        }

        SetVolume();

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume); 
        PlayerPrefs.Save();

    }

    public void DecreaseVolume()
    {
        volume -= 0.1f;
        if (volume < 0f)
        {
            volume = 0.0001f;
        }

        SetVolume();

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume); 
        PlayerPrefs.Save();

    }

    private void SetVolume()
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public float GetVolume()
    {
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.3f);
        SetVolume();
        return volume;
    }

}
