using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelComponent : MonoBehaviour
{
    [Tooltip("Value in seconds")]
    [Min(1)]
    public float TimeToCompliteLevel = 99;
    [Tooltip("Value in seconds")]
    [Min(1)]
    public float CountDownTime = 3;

    public Collider2D FloorCollider;

    public static LevelComponent SceneSingleton;

    public Action<LevelState> OnStateChanged;
    public Action<int> OnScoreUpdate;
    public UnityEvent OnLevelRestart;

    LevelState state = LevelState.Paused;
    public LevelState State
    {
        get => state; private set
        {
            state = value;
            OnStateChanged.Invoke(state);
        }
    }

    int score;
    public int Score
    {
        get => score; set
        {
            score = value;
            OnScoreUpdate?.Invoke(Score);
        }
    }

    public float FloorY => FloorCollider.bounds.extents.y + FloorCollider.transform.position.y;

    public float LevelTimer { get; private set; }

    int playersAlive;

    void Awake()
    {
        SceneSingleton = this;
    }

    void Start()
    {
        StartCoroutine(Restart());
        EntitiesContainerComponent.SceneSingleton.OnBallRemoved += CheckWinCondition;
    }

    void Update()
    {
        LevelTimer -= Time.deltaTime;

        if (LevelTimer < 0)
        {
            Lose();
        }
    }

    public void PlayerKilled()
    {
        playersAlive--;

        if(playersAlive == 0)
        {
            Lose();
        }
    }

    public void CheckWinCondition(List<BallComponent> balls)
    {
        if (balls.Count == 0)
        {
            Pause();
            GameSingleton.Instance.LoadNextLevel();
            Play();
        }
    }

    public void AddScore(HarpoonComponent harpoon)
    {
        Score += Mathf.FloorToInt(harpoon.HarpoonLength) * 10;
    }

    void Pause()
    {
        State = LevelState.Paused;
        Time.timeScale = 0;
    }

    void Play()
    {
        State = LevelState.Playing;
        Time.timeScale = 1;
    }

    void Lose()
    {
        StartCoroutine(Restart());
    }

    public IEnumerator Restart()
    {
        OnLevelRestart.Invoke();
        EntitiesContainerComponent.SceneSingleton.ResetEntities();
        LevelTimer = TimeToCompliteLevel;
        Score = 0;
        Pause();
        yield return new WaitForSecondsRealtime(CountDownTime);
        playersAlive = (int)GameSingleton.Instance.NumberOfPlayers;
        Play();
        yield return null;
    }

    void OnDestroy()
    {
        SceneSingleton = null;
    }
}
