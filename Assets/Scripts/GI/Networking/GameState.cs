using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameState : NetworkBehaviour
{

    //public static GameState instance;
    int localPlayerController;

    //TODO: make these hashsets? make these arrays?
    internal Dictionary<int, PlayerController> activeControllers;
    internal Dictionary<int, Character> characters;

    
    public void Awake()
    {
        //instance = this;
        activeControllers = new Dictionary<int, PlayerController>();
        characters = new Dictionary<int, Character>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void AddController(int id, PlayerController controller)
    {
        activeControllers.Add(id, controller);
    }

    public void AddCharacter(int id, Character character)
    {
        characters.Add(id, character);
    }

    public PlayerController GetPlayerController()
    {
        return GetPlayerController(localPlayerController);
    }

    public PlayerController GetPlayerController(int id)
    {
        if (activeControllers.ContainsKey(id))
        {
            return activeControllers[id];
        }
        return null;
    }

    public Character GetPlayerCharacter(int id)
    {
        if (characters.ContainsKey(id))
        {
            return characters[id];
        }
        return null;
    }
}
