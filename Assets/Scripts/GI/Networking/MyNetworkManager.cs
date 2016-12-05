using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

    [Header("Gameplay Script Classes")]
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

    private int nextPlayerID = 0;
    private int nextCharacterID = 0;

    private GameMode currentGameMode;
    private GameState currentGameState;

    public void Awake()
    {
        GameObject gameModeRef = Instantiate(gameModePrefab);
        currentGameMode = (GameMode)gameModeRef.AddComponent(typeof(GameMode));
        //currentGameMode = (GameMode)gameModeRef.AddComponent(gameMode.GetClass());
        //NetworkServer.Spawn(gameModeRef);

        GameObject gameStateRef = Instantiate(gameStatePrefab);
        currentGameState = (GameState)gameStateRef.AddComponent(typeof(GameState));
        //currentGameState = (GameState)gameStateRef.AddComponent(gameState.GetClass());
        //NetworkServer.Spawn(gameStateRef);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //GameObject gameModeRef = Instantiate(gameModePrefab);
        //currentGameMode = (GameMode)gameModeRef.AddComponent(gameMode.GetType());
        //gameModeRef.SetActive(true);
        ////NetworkServer.Spawn(gameModeRef);
        
        //GameObject gameStateRef = Instantiate(gameStatePrefab);
        //currentGameState = (GameState)gameStateRef.AddComponent(gameState.GetType());
        ////NetworkServer.Spawn(gameStateRef);
    }

    public GameMode GetGameMode()
    {
        return currentGameMode;
    }

    public GameState GetGameState()
    {
        return currentGameState;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
    }

    [Server]
    public void CreateNewCharacter(PlayerController controller)
    {
        //TODO: I'd like to use SpawnLocation.GetSpawnLocation() so it can check if the spawn is valid at runtime
        GameObject newCharacter = Instantiate(characterPrefab, startPositions[0]);
        newCharacter.AddComponent(typeof(Character));
        NetworkServer.Spawn(newCharacter);
        //newCharacter.AddComponent(playerCharacter.GetClass());
        controller.Possess(newCharacter.GetComponent<Character>());
    }

    [Server]
    public int RegisterNewCharacter(Character character)
    {
        int assignedID = nextCharacterID;
        nextCharacterID++;

        GetGameState().characters.Add(assignedID, character);

        return assignedID;
    }

    //TODO: this isn't called
    [Server]
    public bool RemoveCharacter(int id)
    {
        if (GetGameState().characters.ContainsKey(id))
        {
            GetGameState().characters.Remove(id);
            return true;
        }
        return false;
    }

    [Server]
    public int RegisterNewPlayer(PlayerController controller)
    {
        int assignedID = nextPlayerID;
        nextPlayerID++;

        GetGameState().AddController(assignedID, controller);
        CreateNewCharacter(controller);

        return assignedID;
    }

    //TODO: this isn't called
    [Server]
    public bool RemovePlayer(int id)
    {
        return GetGameState().activeControllers.Remove(id);
    }

    //override 
}
