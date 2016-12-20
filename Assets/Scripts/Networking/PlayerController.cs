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

    private int id = -1;

    public int ID
    {
        get
        {
            return id;
        }
        private set
        {
            Logger.Log("PC: RPC " + ID + " current ID: " + ID);
            if (ID == -1)
            {
                Logger.Log("Setting ID to :" + ID);
                id = value;
                GameManager.Instance.GetGameState().AddController(ID, this);
            }
        }
    }

    [ClientRpc]
    private void RpcSetID(int id)
    {
        ID = id;
    }

    [TargetRpc]
    public void TargetSetID(NetworkConnection target, int id)
    {
        ID = id;
    }

    GamePadWrapper.UpdateStateDel gamePadStateUpdater;
    GamePadWrapper gamePad;

    protected void Awake() {
        charactersPossessed = new HashSet<Character>();
    }

    protected virtual void Start()
    {
        if (isServer)
        {
            Logger.Log("PC: Server");
            id = GameManager.Instance.GetGameMode().RegisterNewPlayer(this);
            Logger.Log("PC: Server. ID: " + id);
            RpcSetID(ID);

            //this means that a new (non-host) player is joining
            if (!isLocalPlayer)
            {
                GameManager.Instance.GetGameMode().UpdateConnectingClient(this);
            }
        }
        
    }

    // Update is called once per frame
 //   void Update ()
 //   {
        
	//}

    [ClientRpc]
    private void RpcSetCharacter(int characterID, bool possess)
    {
        Character character = GameManager.Instance.GetGameState().GetPlayerCharacter(characterID);
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
