using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioSource> musicSources, SFXSources, dialogueSources;
    [SerializeField] Slider masterSlider, musicSlider, SFXSlider, dialogueSlider;

    Dictionary<AudioSource, float> musicDefaultVolume = new Dictionary<AudioSource, float>();
    Dictionary<AudioSource, float> SFXDefaultVolume = new Dictionary<AudioSource, float>();
    Dictionary<AudioSource, float> dialogueDefaultVolume = new Dictionary<AudioSource, float>();

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(delegate { ControlVolumes("master"); });

        SetDefaultVolumes(musicSources, musicDefaultVolume);
        musicSlider.onValueChanged.AddListener(delegate { ControlVolumes("music"); });

        SetDefaultVolumes(SFXSources, SFXDefaultVolume);
        SFXSlider.onValueChanged.AddListener(delegate { ControlVolumes("sfx"); });

        SetDefaultVolumes(dialogueSources, dialogueDefaultVolume);
        dialogueSlider.onValueChanged.AddListener(delegate { ControlVolumes("dialogue"); });
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
                SetRelativeVolume(musicSources, musicDefaultVolume, musicSlider);
                break;
            case "sfx":
                SetRelativeVolume(SFXSources, SFXDefaultVolume, SFXSlider);
                break;
            case "dialogue":
                SetRelativeVolume(dialogueSources, dialogueDefaultVolume, dialogueSlider);
                break;
            case "master":
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
