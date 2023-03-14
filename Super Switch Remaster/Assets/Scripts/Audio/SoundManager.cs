using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;

        volume = GetVolume();
    }

    public void IncreaseVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 1f;
        }

        SetVolume();

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
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

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();

    }

    private void SetVolume()
    {
        audioMixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
    }

    public float GetVolume()
    {
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 0.6f);
        SetVolume();
        return volume;
    }

}
