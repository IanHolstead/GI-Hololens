using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMode : NetworkBehaviour
{
    public SpawnLocation spawnLocation;
    
    // Use this for initialization
    void Awake () {
        spawnLocation = FindObjectOfType<SpawnLocation>();
        
    }


    // Update is called once per frame
    void Update () {
		
	}
}
