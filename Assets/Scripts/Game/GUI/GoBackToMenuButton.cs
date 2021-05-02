using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GoBackToMenuButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => GameSingleton.Instance.LoadMenuLevel());
    }
}
