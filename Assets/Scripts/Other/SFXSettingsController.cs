using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXSettingsController : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;

    [Header("UI")]
    public Slider sfxVolumeSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        sfxVolumeSlider.value = savedVolume;
        SetSFXVolume(savedVolume);

        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float volumeDb = Mathf.Lerp(-80f, 0f, sliderValue);
        audioMixer.SetFloat("SFXVolume", volumeDb);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }
}
