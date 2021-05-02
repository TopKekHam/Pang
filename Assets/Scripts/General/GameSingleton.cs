using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSingleton : MonoBehaviour
{
    public static GameSingleton Instance { get; private set; }

    [Tooltip("list of level scene names")]
    public string[] Levels;
    public string MenuScene;

    [HideInInspector]
    public int CurrentLevelIndex;
    [HideInInspector]
    public PlayerNumber NumberOfPlayers = PlayerNumber.One;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadFirstLevel()
    {
        CurrentLevelIndex = 0;
        SceneManager.LoadScene(Levels[0]);
    }

    public void LoadMenuLevel()
    {
        SceneManager.LoadScene(MenuScene);
    }

    public void LoadNextLevel()
    {
        CurrentLevelIndex++;

        if (CurrentLevelIndex == Levels.Length)
        {
            SceneManager.LoadScene(MenuScene);
        }
        else
        {
            Time.timeScale = 0;
            SceneManager.LoadScene(Levels[CurrentLevelIndex]);
            Time.timeScale = 1;
        }
    }
}

