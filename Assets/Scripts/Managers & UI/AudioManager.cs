using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioSource> musicSources, SFXSources, dialogueSources;
    [SerializeField] Slider masterSlider, musicSlider, SFXSlider, dialogueSlider;
    [SerializeField] Toggle subtitlesToggle;

    [SerializeField] GameObject captions;

    Dictionary<AudioSource, float> musicDefaultVolume = new Dictionary<AudioSource, float>();
    Dictionary<AudioSource, float> SFXDefaultVolume = new Dictionary<AudioSource, float>();
    Dictionary<AudioSource, float> dialogueDefaultVolume = new Dictionary<AudioSource, float>();

    private static float masterVolume = 1;
    private static float musicVolume = 1;
    private static float dialogueVolume = 1;
    private static float SFXVolume = 1;

    private static bool areCaptionsEnabled = true;

    private void Start()
    {
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        dialogueSlider.value = dialogueVolume;
        SFXSlider.value = SFXVolume;
        subtitlesToggle.isOn = areCaptionsEnabled;

        masterSlider.onValueChanged.AddListener(delegate { ControlVolumes("master"); });

        SetDefaultVolumes(musicSources, musicDefaultVolume);
        musicSlider.onValueChanged.AddListener(delegate { ControlVolumes("music"); });

        SetDefaultVolumes(SFXSources, SFXDefaultVolume);
        SFXSlider.onValueChanged.AddListener(delegate { ControlVolumes("sfx"); });

        SetDefaultVolumes(dialogueSources, dialogueDefaultVolume);
        dialogueSlider.onValueChanged.AddListener(delegate { ControlVolumes("dialogue"); });

        subtitlesToggle.onValueChanged.AddListener(delegate { ToggleCaptions(subtitlesToggle.isOn); });
    }

    private void ToggleCaptions(bool state)
    {
        areCaptionsEnabled = state;
        if (captions == null) { return; }
        captions.SetActive(state);
    }

    public void PauseAudio(bool state)
    {
        if (state)
        {
            foreach (AudioSource source in musicSources) { source.Pause(); }
            foreach (AudioSource source in SFXSources) { source.Pause(); }
            foreach (AudioSource source in dialogueSources) { source.Pause(); }
            return;
        }
        foreach (AudioSource source in musicSources) { source.UnPause(); }
        foreach (AudioSource source in SFXSources) { source.UnPause(); }
        foreach (AudioSource source in dialogueSources) { source.UnPause(); }

    }

    private void ControlVolumes(string audioCategory)
    {
        switch (audioCategory)
        {
            case "music":
                musicVolume = musicSlider.value;
                SetRelativeVolume(musicSources, musicDefaultVolume, musicSlider);
                break;
            case "sfx":
                SFXVolume = SFXSlider.value;
                SetRelativeVolume(SFXSources, SFXDefaultVolume, SFXSlider);
                break;
            case "dialogue":
                dialogueVolume = dialogueSlider.value;
                SetRelativeVolume(dialogueSources, dialogueDefaultVolume, dialogueSlider);
                break;
            case "master":
                masterVolume = masterSlider.value;
                SetRelativeVolume(musicSources, musicDefaultVolume, musicSlider);
                SetRelativeVolume(SFXSources, SFXDefaultVolume, SFXSlider);
                SetRelativeVolume(dialogueSources, dialogueDefaultVolume, dialogueSlider);
                break;
        }
    }

    private void SetRelativeVolume(List<AudioSource> sources, Dictionary<AudioSource, float> defaultVolumes, Slider slider)
    {
        foreach (AudioSource source in sources)
        {
            source.volume = slider.value * defaultVolumes[source] * masterSlider.value;
        }
    }

    private void SetDefaultVolumes(List<AudioSource> sources, Dictionary<AudioSource, float> defaultVolumes)
    {
        foreach (AudioSource source in sources)
        {
            defaultVolumes.Add(source, source.volume);
        }
    }


}
