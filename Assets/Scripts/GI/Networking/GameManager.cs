using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GI.Singleton<GameManager>{

    protected GameMode currentGameMode;

    private GameState currentGameState;
    private Dictionary<int, PlayerController> activeControllers;
    private Dictionary<int, Character> characters;

    private int nextPlayerID = 0;

    public void Start()
    {
        if (!currentGameMode)
        {
            //currentGameMode = new GameMode();//? this doesn't seem right
        }
        //currentGameState = currentGameMode
        activeControllers = new Dictionary<int, PlayerController>();
        characters = new Dictionary<int, Character>();

    }

    public void RegisterNewCharacter()
    {

    }

    public void RemoveCharacter()
    {

    }

    public void RegisterNewPlayer()
    {

    }

    public void RemovePlayer()
    {

    }

    public GameMode GetGameMode()
    {
        return currentGameMode;
    }

    public GameState GetGameState()
    {
        return currentGameState;
    }

    public PlayerController GetPlayerController(int id)
    {
        if (activeControllers.ContainsKey(id))
        {
            return activeControllers[id];
        }
        return null;
    }

    public Character GetPlayerCharacter (int id)
    {
        if (characters.ContainsKey(id))
        {
            return characters[id];
        }
        return null;
    }
}
