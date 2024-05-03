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
        if (!loadLevel) //if not on loading level
        {
            for (int i = 0; i < pInterfaces.Count; i++) //checks all player
            {
                
                if (i < numberOfPlayers) //no of players 
                {
                    //deselect a character
                    if (Input.GetButtonUp("Fire2" + charManager.players[i].inputId))
                    {
                        //removes character from being selected
                        pInterfaces[i].playerBase.hasCharacter = false;
                    }
                    if (!charManager.players[i].hasCharacter) // if already selected the character
                    {
                        pInterfaces[i].playerBase = charManager.players[i]; //sets up the reference from playerBase

                        HandleSelectorPosition(pInterfaces[i]); //finds the active portrait
                        HandleSelectorScreenInput(pInterfaces[i], charManager.players[i].inputId); //input of player/user
                        HandleCharacterPreview(pInterfaces[i]); //visualize the character on every portrait
                    }
                }
                else 
                {
                    charManager.players[i].hasCharacter = true;
                }
            }
        }

        if (bothPlayerIsSelected)
        {
            Debug.Log("loading");
            StartCoroutine(LoadLevel());
            loadLevel = true;
        }
        else 
        {
            if (charManager.players[0].hasCharacter && charManager.players[1].hasCharacter) 
            {
                bothPlayerIsSelected = true;
            }
        }
    }

    void HandleSelectorScreenInput(PlayerInterfaces p1, string playerId) 
    {
        #region Grid Navigation
        /* To navigate grid
         * change active x and y to select what entry is active
         * smooth out the input if user keeps pressing the button
         * won't be switch more than once after a few seconds
         */

        float vertical = Input.GetAxis("Vertical" + playerId); // pressing up and down

        if (vertical != 0) 
        {
            if (!p1.hitInputOnce) 
            {
                if (vertical > 0)
                {
                    p1.activeY = (p1.activeY > 0) ? p1.activeY - 1 : maxY - 1;
                }
                else 
                {
                    p1.activeY = (p1.activeY < maxY - 1) ? p1.activeY + 1 : 0;
                }

                p1.hitInputOnce = true;
            }
        }

        float horizontal = Input.GetAxis("Horizontal" + playerId); //left and right

        if (horizontal != 0)
        {
            if (!p1.hitInputOnce)
            {
                if (horizontal > 0)
                {
                    p1.activeX = (p1.activeX > 0) ? p1.activeX - 1 : maxY - 1;
                }
                else
                {
                    p1.activeX = (p1.activeX < maxY - 1) ? p1.activeX + 1 : 0;
                }

                p1.timerReset = 0;
                p1.hitInputOnce = true;
            }
        }

        if (vertical == 0 && horizontal == 0) 
        {
            p1.hitInputOnce = false;
        }

        if (p1.hitInputOnce) 
        {
            p1.timerReset += Time.deltaTime;

            if (p1.timerReset > 0.8f) 
            {
                p1.hitInputOnce = false;
                p1.timerReset = 0;
            }
        }

        #endregion

        // if user press space, will select a character
        if (Input.GetButtonUp("Fire1" + playerId))
        {
            //reaction of character
            p1.createdCharacter.GetComponentInChildren<Animator>().Play("Taunt");

            //pass the character to the character manager so that we will know waht prefab to create in the Ingame
            p1.playerBase.playerPrefab = charManager.ReturnCharacterWithId(p1.activePortrait.characterId).prefab;

            p1.playerBase.hasCharacter = true;
        }

        


    }
    IEnumerator LoadLevel() 
    {
        //if 1 player is an AI, assigns a random character to the prefab
        for (int i = 0; i < charManager.players.Count; i++) 
        {
            if (charManager.players[i].playerType == PlayerBase.PlayerType.ai) 
            {
                if (charManager.players[i].playerPrefab == null) 
                {
                    int ranValue = Random.Range(0, portraitPrefabs.Length);
                    charManager.players[i].playerPrefab = charManager.ReturnCharacterWithId(portraitPrefabs[ranValue].characterId).prefab;
                    Debug.Log(portraitPrefabs[ranValue].characterId);
                }
            }
        }

        yield return new WaitForSeconds(2); //lods level after few seconds
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    }

    void HandleSelectorPosition(PlayerInterfaces p1) 
    {
        p1.selector.SetActive(true); //enables the selector

        p1.activePortrait = charGrid[p1.activeX, p1.activeY]; //finds the active portrait

        //place the selector over its position
        Vector2 selectorPos = p1.activePortrait.transform.localPosition;
        selectorPos = selectorPos + new Vector2(portraitCanvas.transform.localPosition.x,
            portraitCanvas.transform.localPosition.y);

        p1.selector.transform.localPosition = selectorPos;
    }

    void HandleCharacterPreview(PlayerInterfaces p1) 
    {
        // if previous portrait is not same as the active, it means we changes characters
        if (p1.previewPortrait != p1.activePortrait)
        {
            if (p1.createdCharacter != null) //delete the current if we have it
            {
                Destroy(p1.createdCharacter);
            }

            //create another one
            //GameObject go = Instantiate(CharacterManager.GetInstance().ReturnCharacterWithId(p1.activePortrait.characterId).prefab,
                //p1.charVisPos.position, Quaternion.identity) as GameObject;

            //p1.createdCharacter = go;

            p1.previewPortrait = p1.activePortrait;

            if (!string.Equals(p1.playerBase.playerId, charManager.players[0].playerId))
            {
                p1.createdCharacter.GetComponent<StateManager>().lookRight = false; //statemanager script has not yet implemented!!!
            }
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
        public int activeY;

        //smoothing out inputs
        public bool hitInputOnce;
        public float timerReset;

        public PlayerBase playerBase;
    }
}



