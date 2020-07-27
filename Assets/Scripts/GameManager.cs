using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool gameHasStarted = false;
    private bool gameFinished = false;
    private int gameRounds = 0;

    public bool GameHasStarted { get => gameHasStarted; set => gameHasStarted = value; }
    public bool GameFinished { get => gameFinished; set => gameFinished = value; }
    public int GameRounds { get => gameRounds; set => gameRounds = value; }

    void Start()
    {
        if (Instance == null || Instance != this)
            Instance = this;
    }
}
