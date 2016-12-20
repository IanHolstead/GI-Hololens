using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Character : NetworkBehaviour {

    private PlayerState state;
    private PlayerController controller;

    public PlayerState State
    {
        get
        {
            return state;
        }
    }

    void Awake()
    {
        state = (PlayerState)gameObject.AddComponent(typeof(PlayerState));
        state.Character = this;
    }

    void Start()
    {
        //state = (PlayerState)gameObject.AddComponent(((MyNetworkManager)NetworkManager.singleton).playerState.GetClass());
        if (isServer)
        {
            State.SetID(GameManager.Instance.GetGameMode().RegisterNewCharacter(this));
            state.RpcSetID(state.ID);
        }
    }

    void Update () {
		
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerControllerID">ID of controller possessing this character. use id=-1 to unpossess</param>
    [ClientRpc]
    private void RpcSetController(int playerControllerID)
    {
        if (playerControllerId == -1)
        {
            controller = null;
            return;
        }
        controller = GameManager.Instance.GetGameState().GetPlayerController(playerControllerId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>false if already possessed</returns>
    public bool CanPossess()
    {
        if (this.controller != null)
        {
            //already possessed
            return false;
        }
        return true;
    }

    [Server]
    public bool Possess(PlayerController controller)
    {
        if (CanPossess())
        {
            this.controller = controller;
            transform.position = controller.transform.position;
            RpcSetController(controller.ID);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Used to unpossess this actor if you don't know which controller possesses it. 
    /// Safely removes it from the controller too.
    /// </summary>
    [Server]
    public void UnPossess()
    {
        controller.UnPossess(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controller"></param>
    [Server]
    public void UnPossess(PlayerController controller)
    {
        this.controller = null;
        RpcSetController(-1);
    }
}
