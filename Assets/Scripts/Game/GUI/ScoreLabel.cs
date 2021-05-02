using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class ScoreLabel : MonoBehaviour
{
    TMP_Text label;

    void Start()
    {
        label = GetComponent<TMP_Text>();
        LevelComponent.SceneSingleton.OnScoreUpdate += UpdateScore;
    }

    public void UpdateScore(int score)
    {
        label.text = $"{score}";
    }
}
