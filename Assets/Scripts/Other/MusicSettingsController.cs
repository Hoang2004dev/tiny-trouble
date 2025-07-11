using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSettingsController : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;

    [Header("UI")]
    public Slider musicVolumeSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        musicVolumeSlider.value = savedVolume;
        SetMusicVolume(savedVolume);

        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float volumeDb = Mathf.Lerp(-80f, 0f, sliderValue);
        audioMixer.SetFloat("MusicVolume", volumeDb);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
}
