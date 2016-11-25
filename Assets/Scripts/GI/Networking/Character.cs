using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    PlayerState state;
    PlayerController controller;

    void Awake()
    {
        gameObject.AddComponent(GameManager.Instance.GetGameMode().playerState.GetType());
        state.SetID(GameManager.Instance.GetGameMode().RegisterNewCharacter(this));
    }

	// Use this for initialization
	void Start () {
          
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Do not call this (call possess on the player controller).
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

    public void UnPossess()
    {
        controller = null;
    }
}
