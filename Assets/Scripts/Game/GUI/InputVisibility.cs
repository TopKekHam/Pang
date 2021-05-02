using UnityEngine;

public class InputVisibility : MonoBehaviour
{

    public PlayerNumber NumberOfPlayers;

    void Awake()
    {
        var visiable = NumberOfPlayers == GameSingleton.Instance.NumberOfPlayers;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(visiable);
        }
    }

}
