using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource globalMusicAudioSource, globalSFXAudioSource;
    [SerializeField] AudioClip regularMusic, fastMusic;
    [SerializeField] AudioClip endMusicKeyClip;

    public void PlayMusic()
    {
        globalMusicAudioSource.clip = regularMusic;
        globalMusicAudioSource.Play();
    }

    public void ShiftIntoHighGear()
    {
        StartCoroutine(TimedMusicChange());
    }

    IEnumerator TimedMusicChange()
    {
        for (float i = 1; i > 0; i -= 0.01f)
        {
            globalMusicAudioSource.pitch = i;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        yield return new WaitForSecondsRealtime(1);
        globalMusicAudioSource.pitch = 1;

        globalMusicAudioSource.clip = null;
        globalMusicAudioSource.clip = fastMusic;
        globalMusicAudioSource.Play();
    }

    public void GraduallyEndMusic()
    {
        StartCoroutine(EndMusic());
    }

    IEnumerator EndMusic()
    {
        for (float i = 1; i > 0; i -= 0.01f)
        {
            globalMusicAudioSource.pitch = i;
            yield return new WaitForSecondsRealtime(0.025f);
        }

        globalMusicAudioSource.clip = null;
        globalSFXAudioSource.PlayOneShot(endMusicKeyClip);
        yield return new WaitForSecondsRealtime(endMusicKeyClip.length);

        FindAnyObjectByType<ScoreManager>().FinishEndStateTransition();
    }
}
