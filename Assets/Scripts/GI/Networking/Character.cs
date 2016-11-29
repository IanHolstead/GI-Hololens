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
        state = (PlayerState)gameObject.AddComponent(((MyNetworkManager)NetworkManager.singleton).playerState.GetClass());
        State.SetID(((MyNetworkManager)NetworkManager.singleton).RegisterNewCharacter(this));
    }

	// Use this for initialization
	void Start () {
          
	}
	
	// Update is called once per frame
	void Update () {
		
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

    public bool Possess(PlayerController controller)
    {
        if (CanPossess())
        {
            this.controller = controller;
            return true;
        }
        return false;
    }

    public void UnPossess(PlayerController controller)
    {
        controller = null;
    }
}
