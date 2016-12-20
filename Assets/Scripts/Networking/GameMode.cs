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
        Logger.Log("GameModeAwake");
        spawnLocation = FindObjectOfType<SpawnLocation>();
    }


    // Update is called once per frame
    void Update () {
		
	}

    [Server]
    public void UpdateConnectingClient(PlayerController connectingController)
    {
        Logger.Log("Updating controller: " + connectingController.ID);
        foreach(KeyValuePair<int, PlayerController> controller in GameManager.Instance.GetGameState().activeControllers)
        {
            if (controller.Value != connectingController)
            {
                controller.Value.TargetSetID(connectingController.GetComponent<NetworkIdentity>().connectionToClient, controller.Key);
            }
        }
    }

    [Server]
    public void CreateNewCharacter(PlayerController controller)
    {
        //TODO: I'd like to use SpawnLocation.GetSpawnLocation() so it can check if the spawn is valid at runtime
        GameObject newCharacter = Instantiate(GameManager.Instance.characterPrefab, ((MyNetworkManager)NetworkManager.singleton).startPositions[0]);
        newCharacter.AddComponent(typeof(Character));
        NetworkServer.Spawn(newCharacter);
        //newCharacter.AddComponent(playerCharacter.GetClass());
        controller.Possess(newCharacter.GetComponent<Character>());
    }

    [Server]
    public int RegisterNewCharacter(Character character)
    {
        Logger.Log("Registering new character: " + character);
        int assignedID = nextCharacterID;
        nextCharacterID++;

        GameManager.Instance.GetGameState().AddCharacter(assignedID, character);

        return assignedID;
    }

    //TODO: this isn't called
    [Server]
    public bool RemoveCharacter(int id)
    {
        return GameManager.Instance.GetGameState().characters.Remove(id);
    }

    [Server]
    public int RegisterNewPlayer(PlayerController controller)
    {
        Logger.Log("Registering new Controller: " + controller);
        int assignedID = nextPlayerID;
        nextPlayerID++;

        GameManager.Instance.GetGameState().AddController(assignedID, controller);
        CreateNewCharacter(controller);

        return assignedID;
    }

    //TODO: this isn't called
    [Server]
    public bool RemovePlayer(int id)
    {
        return GameManager.Instance.GetGameState().activeControllers.Remove(id);
    }
}
