using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GI.Singleton<GameManager>{

    public UnityEditor.MonoScript gamemode;

    private GameMode currentGameMode;
    private GameState currentGameState;

    void Awake()
    {

    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public GameMode GetGameMode()
    {
        return currentGameMode;
    }

    public GameState GetGameState()
    {
        return currentGameState;
    }
}
