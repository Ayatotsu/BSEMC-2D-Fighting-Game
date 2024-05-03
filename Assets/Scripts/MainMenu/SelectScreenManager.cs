using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScreenManager : MonoBehaviour
{
    public int numberOfPlayers = 1;
    public List<PlayerInterfaces> pInterfaces = new List<PlayerInterfaces>();
    public PortraitInfo[] portraitPrefabs; //All entries as prefabs
    //no of portraits on x and y.(Hard Coded)
    public int maxX; 
    public int MaxY;

    PortraitInfo[,] charGrid; //select entries we made on grids


    public GameObject portraitCanvas; //canvas that holld all portraits;


    bool loadLevel; //if loading the level
    public bool bothPlayerIsSelected;

    CharacterManager charManager;

    public static


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}


[System.Serializable]
public class PlayerInterfaces 
{
    public PortraitInfo activePortrait; //current active portrait for p1
    public PortraitInfo previewPortrait; 
    public GameObject selector; //select indicator for p1
    public Transform charVisPos; //visual position for p1
    public GameObject createdCharacter; //created character for p1
    
    //Entries for p1
    public int activeX;
    public int ActiveY;
    
    //smoothing out inputs
    public int hitInputOnce;
    public float timerReset;

    public PlayerBase playerBase;
}
