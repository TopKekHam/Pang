using System;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesContainerComponent : MonoBehaviour
{
    public static EntitiesContainerComponent SceneSingleton;

    public Action<List<BallComponent>> OnBallRemoved;

    public GameObject DynamicGameObjects;
    [HideInInspector]
    public GameObject RuntimeEntitiesContainer;

    [HideInInspector]
    public PlayerComponent[] Players = new PlayerComponent[2];

    List<BallComponent> activeBalls = new List<BallComponent>();

    void Awake()
    {
        SceneSingleton = this;
    }

    void Start()
    {
        DynamicGameObjects.SetActive(false);
    }

    public void ResetEntities()
    {
        if (RuntimeEntitiesContainer != null)
        {
            Destroy(RuntimeEntitiesContainer);
        }

        RuntimeEntitiesContainer = Instantiate(DynamicGameObjects);
        RuntimeEntitiesContainer.SetActive(true);

        var players = RuntimeEntitiesContainer.GetComponentsInChildren<PlayerComponent>();

        for (int i = 0; i < players.Length; i++)
        {
            Players[(int)players[i].PlayerNumber] = players[i];
            var active = players[i].PlayerNumber <= GameSingleton.Instance.NumberOfPlayers;
            players[i].gameObject.SetActive(active);
        }

        activeBalls.Clear();
        var balls = RuntimeEntitiesContainer.GetComponentsInChildren<BallComponent>();
        activeBalls.AddRange(balls);

    }

    public void AddBall(BallComponent ball)
    {
        activeBalls.Add(ball);
    }

    public void RemoveBall(BallComponent ball)
    {
        activeBalls.Remove(ball);
        OnBallRemoved?.Invoke(activeBalls);
    }

    void OnDestroy()
    {
        SceneSingleton = null;
    }
}
