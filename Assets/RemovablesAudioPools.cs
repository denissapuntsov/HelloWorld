using UnityEngine;
using Random = UnityEngine.Random;

public class RemovablesAudioPools : MonoBehaviour
{
    public AudioClip[] poolOfClips;

    public AudioClip TrashRemovalAudioClip
    {
        get
        {
            AudioClip chosenClip = poolOfClips[Random.Range(0, poolOfClips.Length)];
            return chosenClip;
        }
    }
}
