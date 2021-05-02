using UnityEngine;

public class MenuComponent : MonoBehaviour
{

    public AudioClip BackgroundMusic;

    void Start()
    {
        AudioPlayerSingleton.Instance.PlayMusic(BackgroundMusic);
    }

    public void ButtonClickOnePlayer()
    {
        Play(PlayerNumber.One);
    }

    public void ButtonClickTwoPlayers()
    {
        Play(PlayerNumber.Two);
    }

    void Play(PlayerNumber number)
    {
        GameSingleton.Instance.NumberOfPlayers = number;
        GameSingleton.Instance.LoadFirstLevel();
    }

    public void ButtonClickQuit()
    {
        Application.Quit();
    }

}
