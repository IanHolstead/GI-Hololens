using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : HoloToolkit.Unity.Singleton<GameManager>
{
    //[Header("Gameplay Script Classes")]
    //public UnityEditor.MonoScript gameMode;
    //public UnityEditor.MonoScript gameState;
    //public UnityEditor.MonoScript playerState;
    //public UnityEditor.MonoScript playerCharacter;
    //public UnityEditor.MonoScript playerController;

    [Header("Gameplay Prefabs")]
    [Tooltip("Remember to add to this to the spawnable prefabs list!")]
    public GameObject gameModePrefab;
    [Tooltip("Remember to add to this to the spawnable prefabs list")]
    public GameObject gameStatePrefab;
    [Tooltip("Remember to add to this to the spawnable prefabs list")]
    public GameObject characterPrefab;

    private GameMode currentGameMode;
    private GameState currentGameState;

    public void Awake()
    {
        Logger.Log("GameManagerAwake");

        GameObject gameModeRef = Instantiate(gameModePrefab);
        currentGameMode = (GameMode)gameModeRef.AddComponent(typeof(GameMode));
        //currentGameMode = (GameMode)gameModeRef.AddComponent(gameMode.GetClass());
        //NetworkServer.Spawn(gameModeRef);
        //TODO: this doesn't look like it will be replicated nicely

        GameObject gameStateRef = Instantiate(gameStatePrefab);
        currentGameState = (GameState)gameStateRef.AddComponent(typeof(GameState));
        //currentGameState = (GameState)gameStateRef.AddComponent(gameState.GetClass());
        //NetworkServer.Spawn(gameStateRef);
    }

    void Start()
    {
        Logger.Log("GameManager Start");
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
