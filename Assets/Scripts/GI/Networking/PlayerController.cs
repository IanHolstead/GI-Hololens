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
    int id;

    public int ID
    {
        get
        {
            return id;
        }
    }

    public void SetID(int id)
    {
        if (id == -1)
        {
            this.id = id;
        }
    }

    GamePadWrapper.UpdateStateDel gamePadStateUpdater;
    GamePadWrapper gamePad;
     
    void Awake () {
        charactersPossessed = new HashSet<Character>();
        id = ((MyNetworkManager)NetworkManager.singleton).RegisterNewPlayer(this);
        //TODO, need to have a gamePad manager or something
        gamePad = new GamePadWrapper(0);
        gamePadStateUpdater = gamePad.UpdateState;
        gamePadStateUpdater(0f);
    }

    // Update is called once per frame
    void Update () {
        //gamePadStateUpdater(Time.deltaTime);
	}

    /// <summary>
    /// call this is take control of a character using controller
    /// </summary>
    /// <param name="controller"></param>
    /// <returns>false if already possessed</returns>
    public bool Possess(Character character)
    {
        if (charactersPossessed.Count == 0 || allowMultiPossess)
        {
            if (character.Possess(this))
            {
                charactersPossessed.Add(character);
                character.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
                //todo
            }
        }
        return false;
    }

    public void UnPossess(Character character)
    {
        if (charactersPossessed.Contains(character))
        {
            charactersPossessed.Remove(character);
            character.UnPossess(this);
            character.GetComponent<NetworkIdentity>().RemoveClientAuthority(connectionToClient);
        }
        return;
    }
}
