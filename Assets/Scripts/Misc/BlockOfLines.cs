using UnityEngine;

[CreateAssetMenu(fileName = "BlockOfLines", menuName = "Scriptable Objects/BlockOfLines")]
public class BlockOfLines : ScriptableObject
{
    public string id;
    public AudioClip clip;
    public Line[] lines;
}
