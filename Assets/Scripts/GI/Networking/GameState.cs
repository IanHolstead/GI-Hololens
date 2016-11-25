using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameState : NetworkBehaviour
{

    int localPlayerController;

    //make these hashsets? make these arrays?
    internal Dictionary<int, PlayerController> activeControllers;
    internal Dictionary<int, Character> characters;

    
    public void Awake()
    {
        activeControllers = new Dictionary<int, PlayerController>();
        characters = new Dictionary<int, Character>();

    }

    // Update is called once per frame
    void Update () {
		
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
