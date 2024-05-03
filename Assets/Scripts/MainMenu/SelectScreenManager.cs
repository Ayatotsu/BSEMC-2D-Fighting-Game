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
    public int maxY;

    PortraitInfo[,] charGrid; //select entries we made on grids


    public GameObject portraitCanvas; //canvas that holds all portraits;


    bool loadLevel; //if loading the level
    public bool bothPlayerIsSelected;

    CharacterManager charManager;

    #region Singleton
    public static SelectScreenManager instance;
    public static SelectScreenManager GetInstance() 
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
    #endregion


    void Start()
    {
        //gets all refs to character manager
        charManager = CharacterManager.GetInstance();
        numberOfPlayers = charManager.numberOfUsers;

        //create grid
        charGrid = new PortraitInfo[maxX, maxY];

        int x = 0;
        int y = 0;

        //assigned every components (GetComponents instead of GetComponent)
        portraitPrefabs = portraitCanvas.GetComponentsInChildren<PortraitInfo>();

        //go into all portraits
        for (int i = 0; i < portraitPrefabs.Length; i++) 
        {
            // assigns a grid position
            portraitPrefabs[i].posX += x;
            portraitPrefabs[i].posY += y;

            charGrid[x, y] = portraitPrefabs[i];

            if (x < maxX - 1)
            {
                x++;
            }
            else 
            {
                x = 0; y++;
            }
        }
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
