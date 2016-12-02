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

    public PlayerController bla;

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


    public void CreateNewCharacter(PlayerController controller)
    {
        //todo, I'd like to use SpawnLocation.GetSpawnLocation() so it can check if the spawn is valid at runtime
        GameObject newCharacter = Instantiate(characterPrefab, startPositions[0]);
        newCharacter.AddComponent(typeof(Character));
        //newCharacter.AddComponent(playerCharacter.GetClass());
        controller.Possess(newCharacter.GetComponent<Character>());
    }

    public int RegisterNewCharacter(Character character)
    {
        int assignedID = nextCharacterID;
        nextCharacterID++;

        GetGameState().characters.Add(assignedID, character);

        return assignedID;
    }

    public bool RemoveCharacter(int id)
    {
        if (GetGameState().characters.ContainsKey(id))
        {
            GetGameState().characters.Remove(id);
            return true;
        }
        return false;
    }

    public int RegisterNewPlayer(PlayerController controller)
    {
        int assignedID = nextPlayerID;
        nextPlayerID++;

        GetGameState().activeControllers.Add(assignedID, controller);
        CreateNewCharacter(controller);

        return assignedID;
    }

    public bool RemovePlayer(int id)
    {
        if (GetGameState().activeControllers.ContainsKey(id))
        {
            GetGameState().activeControllers.Remove(id);
            return true;
        }
        return false;
    }

    //override 
}
