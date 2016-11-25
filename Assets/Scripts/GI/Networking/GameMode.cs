using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMode : NetworkBehaviour
{
    public UnityEditor.MonoScript gameState;
    public UnityEditor.MonoScript playerState;
    public UnityEditor.MonoScript playerCharacter;
    public UnityEditor.MonoScript playerController;

    public GameObject playerPrefab;

    public SpawnLocation spawnLocation;

    private int nextPlayerID = 0;
    private int nextCharacterID = 0;


    // Use this for initialization
    void Awake () {
        gameObject.AddComponent(gameState.GetType());
        spawnLocation = FindObjectOfType<SpawnLocation>();
    }

    public void CreateNewCharacter(PlayerController controller)
    {
        GameObject newCharacter = Instantiate(playerPrefab, spawnLocation.GetSpawnLocation());
        newCharacter.AddComponent(playerCharacter.GetType());
        controller.Possess(newCharacter.GetComponent<Character>());
    }

    public int RegisterNewCharacter(Character character)
    {
        int assignedID = nextCharacterID;
        nextCharacterID++;

        GameManager.Instance.GetGameState().characters.Add(assignedID, character);

        return assignedID;
    }

    public bool RemoveCharacter(int id)
    {
        if (GameManager.Instance.GetGameState().characters.ContainsKey(id))
        {
            GameManager.Instance.GetGameState().characters.Remove(id);
            return true;
        }
        return false;
    }

    public int RegisterNewPlayer(PlayerController controller)
    {
        int assignedID = nextPlayerID;
        nextPlayerID++;

        GameManager.Instance.GetGameState().activeControllers.Add(assignedID, controller);
        CreateNewCharacter(controller);

        return assignedID;
    }

    public bool RemovePlayer(int id)
    {
        if (GameManager.Instance.GetGameState().activeControllers.ContainsKey(id))
        {
            GameManager.Instance.GetGameState().activeControllers.Remove(id);
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
