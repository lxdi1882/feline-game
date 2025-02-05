using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ResolutionSettings : MonoBehaviour
{
    // Resolution settings
    public TMP_Dropdown resolutionDropdown;  // Reference to the TMP_Dropdown
    public Toggle fullscreenToggle;  // Reference to the Toggle for fullscreen
    private Resolution[] resolutions;
    private List<Resolution> popularResolutions = new List<Resolution>();

    // Music settings
    public AudioSource audioSource; // Reference to the AudioSource for background music
    public AudioClip[] musicTracks; // Array of music tracks to shuffle
    public Slider volumeSlider; // UI slider for volume control

    private List<AudioClip> shuffledPlaylist;
    private int currentTrackIndex = 0;

    void Start()
    {
        // Set up resolution settings
        int[,] commonResolutions = {
        {1920, 1080}, {1366, 768}, {1440, 900}, {1600, 900}, {1280, 720}, {2560, 1440}
    };

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (var resolution in resolutions)
        {
            for (int i = 0; i < commonResolutions.GetLength(0); i++)
            {
                if (resolution.width == commonResolutions[i, 0] && resolution.height == commonResolutions[i, 1])
                {
                    popularResolutions.Add(resolution);
                    options.Add(resolution.width + "x" + resolution.height);
                    break;
                }
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", -1);
        if (savedResolutionIndex != -1 && savedResolutionIndex < popularResolutions.Count)
        {
            resolutionDropdown.value = savedResolutionIndex;
            ApplyResolution(savedResolutionIndex);
        }
        else
        {
            resolutionDropdown.value = GetCurrentResolutionIndex();
            ApplyResolution(resolutionDropdown.value);
        }

        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        fullscreenToggle.onValueChanged.AddListener(ToggleFullscreen);

        // Set up music settings
        volumeSlider.onValueChanged.AddListener(AdjustVolume);

        // Load saved volume
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f); // Default to 1 if no volume is saved
        audioSource.volume = savedVolume;
        volumeSlider.value = savedVolume;  // Set the slider to match the saved volume

        ShuffleAndPlayMusic();
    }

    public void ChangeResolution(int index)
    {
        ApplyResolution(index);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyResolution(int index)
    {
        Resolution selectedResolution = popularResolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    private int GetCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;
        for (int i = 0; i < popularResolutions.Count; i++)
        {
            if (popularResolutions[i].width == currentResolution.width && popularResolutions[i].height == currentResolution.height)
            {
                return i;
            }
        }
        return 0;
    }

    // Music methods
    void ShuffleAndPlayMusic()
    {
        shuffledPlaylist = new List<AudioClip>(musicTracks);
        ShufflePlaylist();
        PlayNextTrack();
    }

    void ShufflePlaylist()
    {
        for (int i = 0; i < shuffledPlaylist.Count; i++)
        {
            AudioClip temp = shuffledPlaylist[i];
            int randomIndex = Random.Range(i, shuffledPlaylist.Count);
            shuffledPlaylist[i] = shuffledPlaylist[randomIndex];
            shuffledPlaylist[randomIndex] = temp;
        }
    }

    void PlayNextTrack()
    {
        if (shuffledPlaylist.Count == 0)
        {
            ShufflePlaylist();
        }

        audioSource.clip = shuffledPlaylist[currentTrackIndex];
        audioSource.Play();
        currentTrackIndex = (currentTrackIndex + 1) % shuffledPlaylist.Count;

        // Start playing the next track once the current one finishes
        StartCoroutine(WaitForTrackEnd());  // <-- Add parentheses here
    }


    // Coroutine to wait until the current track finishes playing
    private IEnumerator WaitForTrackEnd()

    {
        // Wait until the audio source finishes playing the current track
        yield return new WaitForSeconds(audioSource.clip.length);

        // Once the track ends, play the next one
        PlayNextTrack();
    }

    // Adjusts the music volume based on slider value
    void AdjustVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}
