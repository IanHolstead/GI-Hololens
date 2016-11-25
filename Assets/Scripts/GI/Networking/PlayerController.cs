using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControlWrapping;

public class PlayerController : MonoBehaviour {
    
    HashSet<Character> charactersPossessed;
    /// <summary>
    /// Can this controller possess multiple characters at once?
    /// </summary>
    bool allowMultiPossess;
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
        id = GameManager.Instance.GetGameMode().RegisterNewPlayer(this);
        //TODO, need to have a gamePad manager or something
        gamePad = new GamePadWrapper(0);
        gamePadStateUpdater = gamePad.UpdateState;
    }
	
	// Update is called once per frame
	void Update () {
        gamePadStateUpdater(Time.deltaTime);
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
            if (character.CanPossess())
            {
                charactersPossessed.Add(character);
            }
            
        }
        return false;
    }

    public void UnPossess(Character character = null)
    {
        if (!allowMultiPossess && character == null)
        {
            charactersPossessed.Clear();
        }
        if (charactersPossessed.Contains(character))
        {
            charactersPossessed.Remove(character);
        }
        return;
    }
}
