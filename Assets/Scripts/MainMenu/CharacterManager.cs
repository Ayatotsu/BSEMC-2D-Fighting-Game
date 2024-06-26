using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int numberOfUsers;

    public List<PlayerBase> players = new List<PlayerBase>();

    // number of players based on its id and their corresponding prefab;
    public List<CharacterBase> characterList = new List<CharacterBase>();

    //finds characters from its ids;
    public CharacterBase ReturnCharacterWithId(string id) 
    {
        CharacterBase retVal = null;
        for (int i = 0; i < characterList.Count; i++) 
        {
            if (string.Equals(characterList[i].charId, id)) 
            {
                retVal = characterList[i];
                break;
            }
        }
        return retVal;
    }


    // returns the player from his created character and states.
    public PlayerBase ReturnPlayerFromStates(StateManager states) 
    {
        PlayerBase retVal = null;
        for (int i = 0; i < players.Count; i++) 
        {
            if (players[i].playerStates == states) 
            {
                retVal = players[i];
                break;
            }
        }
        return retVal;
    }

    public PlayerBase ReturnOppositePlayer(PlayerBase pl) 
    {
        PlayerBase retVal = null;

        for (int i = 0; i < players.Count; i++) 
        {
            if (players[i] != pl) 
            {
                retVal = players[i];
                break;
            }
        }

        return retVal;
    }

    public static CharacterManager instance;
    public static CharacterManager GetInstance() 
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}

[System.Serializable]
public class CharacterBase 
{
    public string charId;
    public GameObject prefab;
}

[System.Serializable]
public class PlayerBase 
{
    public string playerId;
    public string inputId;
    public PlayerType playerType;
    public bool hasCharacter;
    public GameObject playerPrefab;
    public StateManager playerStates;
    public int score;

    public enum PlayerType 
    {
        user, //players
        ai, //AI itself
        simulation //for multiplayer over network
    }
}
