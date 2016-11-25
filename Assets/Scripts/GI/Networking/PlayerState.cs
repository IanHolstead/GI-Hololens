using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
