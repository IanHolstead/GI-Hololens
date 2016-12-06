using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using ControlWrapping;

public class PlayerController : NetworkBehaviour {
    
    HashSet<Character> charactersPossessed;
    /// <summary>
    /// Can this controller possess multiple characters at once?
    /// </summary>
    bool allowMultiPossess = false;

    int id = -1;

    public int ID
    {
        get
        {
            return id;
        }
    }

    [ClientRpc]
    private void RpcSetID(int id)
    {
        if (id == -1)
        {
            this.id = id;
            GameManager.instance.GetGameState().AddController(id, this);
        }
    }

    GamePadWrapper.UpdateStateDel gamePadStateUpdater;
    GamePadWrapper gamePad;

    void Awake () {
        charactersPossessed = new HashSet<Character>();
        if (isServer)
        {
            id = GameManager.instance.GetGameMode().RegisterNewPlayer(this);
            RpcSetID(id);
        }
        
    }

    // Update is called once per frame
    void Update ()
    {

	}

    [ClientRpc]
    private void RpcSetCharacter(int characterID, bool possess)
    {
        Character character = GameManager.instance.GetGameState().GetPlayerCharacter(characterID);
        if (possess)
        {
            charactersPossessed.Add(character);
        }
        else if (charactersPossessed.Contains(character))
        {
            charactersPossessed.Remove(character);
        }
    }

    /// <summary>
    /// call this is take control of a character using controller
    /// </summary>
    /// <param name="controller"></param>
    /// <returns>false if already possessed</returns>
    [Server]
    public bool Possess(Character character)
    {
        if (charactersPossessed.Count == 0 || allowMultiPossess)
        {
            if (character.Possess(this))
            {
                charactersPossessed.Add(character);
                RpcSetCharacter(character.State.ID, true);
                //TODO: not sure about this one vvv
                //character.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            }
        }
        return false;
    }

    [Server]
    public void UnPossess(Character character)
    {
        if (charactersPossessed.Contains(character))
        {
            charactersPossessed.Remove(character);
            character.UnPossess(this);
            RpcSetCharacter(character.State.ID, false);
            //TODO
            //character.GetComponent<NetworkIdentity>().RemoveClientAuthority(connectionToClient);
        }
        return;
    }
}
