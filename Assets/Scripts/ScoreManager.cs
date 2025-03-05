using Unity.Cinemachine;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField][Range(0, 100)] private int score = 0;
    [SerializeField] private int[] thresholds;
    
    public void AddPoints(int points)
    {
        score += points;
    }

    public void SubtractPoints(int points)
    {
        score -= points;
    }
}
