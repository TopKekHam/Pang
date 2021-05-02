using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LevelTimerLabel : MonoBehaviour
{

    TMP_Text label;
    int prev_value = -1;

    void Start()
    {
        label = GetComponent<TMP_Text>();
    }

    void Update()
    {
        int value = Mathf.CeilToInt(LevelComponent.SceneSingleton.LevelTimer);

        if (prev_value != value)
        {
            prev_value = value;
            label.text = $"{value}";
        }
    }
}
