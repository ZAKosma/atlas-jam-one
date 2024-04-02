using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // Required for interacting with the Audio Mixer

public class SettingsMenu : MonoBehaviour
{
    public Slider audioMusicSlider; // Assign in inspector
    public Slider audioSFXSlider; // Assign in inspector
    public Slider sensitivitySlider; // Assign in inspector
    public AudioMixer audioMixer; // Assign your Audio Mixer in inspector

    private void Start()
    {
        // Initialize sliders with saved values or default
        audioMusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        audioSFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);

        // Add listeners for slider value changes
        audioMusicSlider.onValueChanged.AddListener(SetMusicVolume);
        audioSFXSlider.onValueChanged.AddListener(SetSFXVolume);
        sensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}