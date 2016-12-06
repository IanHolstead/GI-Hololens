using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMode : NetworkBehaviour
{
    public SpawnLocation spawnLocation;

    private int nextPlayerID = 0;
    private int nextCharacterID = 0;

    // Use this for initialization
    void Awake () {
        spawnLocation = FindObjectOfType<SpawnLocation>();
        
    }


    // Update is called once per frame
    void Update () {
		
	}

    [Server]
    public void CreateNewCharacter(PlayerController controller)
    {
        //TODO: I'd like to use SpawnLocation.GetSpawnLocation() so it can check if the spawn is valid at runtime
        GameObject newCharacter = Instantiate(GameManager.instance.characterPrefab, ((MyNetworkManager)NetworkManager.singleton).startPositions[0]);
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

        GameManager.instance.GetGameState().AddCharacter(assignedID, character);

        return assignedID;
    }

    //TODO: this isn't called
    [Server]
    public bool RemoveCharacter(int id)
    {
        return GameManager.instance.GetGameState().characters.Remove(id);
    }

    [Server]
    public int RegisterNewPlayer(PlayerController controller)
    {
        int assignedID = nextPlayerID;
        nextPlayerID++;

        GameManager.instance.GetGameState().AddController(assignedID, controller);
        CreateNewCharacter(controller);

        return assignedID;
    }

    //TODO: this isn't called
    [Server]
    public bool RemovePlayer(int id)
    {
        return GameManager.instance.GetGameState().activeControllers.Remove(id);
    }
}
