using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    WaitForSeconds oneSec; //mostly used on every scenario
    public Transform[] spawnPos; //spawn point for characters in game

    CharacterManager charM;
    LevelUI levelUI; //store ui elements for ease of access

    public int maxTurns = 2;
    int currentTurn = 1; //current turn, starts at 1

    //variables for countdown

    public bool countdown;
    public int maxTurnTimer = 30;
    int currentTimer;
    float internalTimer;

    void Start()
    {
        //get refs from singletons
        charM = CharacterManager.GetInstance();
        levelUI = LevelUI.GetInstance();

        //init WaitForSeconds
        oneSec = new WaitForSeconds(1);

        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        StartCoroutine("StartGame");
    }

    void FixedUpdate()
    {
        //handles player orientation in scene
        //compare x of the 1p, if it is lower, then enemy is on right

        if (charM.players[0].playerStates.transform.position.x <
            charM.players[1].playerStates.transform.position.x)
        {
            charM.players[0].playerStates.lookRight = true;
            charM.players[1].playerStates.lookRight = false;
        }
        else 
        {
            charM.players[0].playerStates.lookRight = false;
            charM.players[1].playerStates.lookRight = true;
        }
    }

    void Update()
    {
        if (countdown) //if countdown is enabled
        {
            HandleTurnTimer(); //controls the timer
        }
    }

    void HandleTurnTimer() 
    {
        levelUI.LevelTimer.text = currentTimer.ToString();

        internalTimer += Time.deltaTime; //every second (freme dependent)

        if (internalTimer > 1) 
        {
            currentTimer --; //subtracts the timer
            internalTimer = 0;
        }

        if (currentTimer <= 0) //if countdown is 0
        {
            EndTurnFunction(true); //ends the turn
            countdown = false;
        }
    }


    IEnumerator StartGame() 
    {
        //game starts

        //create player first
        yield return CreatePlayers();

        //Initate turn
        yield return InitTurn();
    }

    IEnumerator InitTurn() 
    {
        //init turn

        //disable announcer text first
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        //reset timer
        currentTimer = maxTurnTimer;
        countdown = false;

        //init players
        yield return InitPlayers();

        //enable control of each player
        yield return EnableControls();
    }

    IEnumerator CreatePlayers() 
    {
        //all players on the list
        for (int i = 0; i < charM.players.Count; i++) 
        {
            // instantiate prefabs
            GameObject go = Instantiate(charM.players[i].playerPrefab,
                spawnPos[i].position, Quaternion.identity) as GameObject;

            //assigns the need refs
            charM.players[i].playerStates = go.GetComponent<StateManager>(); ;

            charM.players[i].playerStates.healthSlider = levelUI.healthSliders[i];
        }

        yield return null;
    }

    IEnumerator InitPlayers() 
    {
        //reset hp
        for (int i = 0; i < charM.players.Count; i++) 
        {
            charM.players[i].playerStates.health = 150;
            //charM.players[i].playerStates.handleAnim.anim.Play("Locomotion"); or
            //charM.players[i].playerStates.transform.GetComponent<Animator>().Play("Taunt");
            charM.players[i].playerStates.transform.position = spawnPos[i].position;
        }
        yield return null;
    }

    IEnumerator EnableControls() 
    {
        //announcer text
        levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
        levelUI.AnnouncerTextLine1.text = "Turn " + currentTurn;
        levelUI.AnnouncerTextLine1.color = Color.white;
        yield return new WaitForSeconds(2);

        //change ui text and color as time passes
        levelUI.AnnouncerTextLine1.text = "Ready";
        levelUI.AnnouncerTextLine1.color = Color.white;

        yield return new WaitForSeconds(3);
        levelUI.AnnouncerTextLine1.color = Color.red;
        levelUI.AnnouncerTextLine1.text = "FIGHT!";


        // enable control of a player/s
        for (int i = 0; i < charM.players.Count; i++) 
        {
            //user players will enables the input handler
            if (charM.players[i].playerType == PlayerBase.PlayerType.user) 
            {
                InputHandler ih = charM.players[i].playerStates.gameObject.GetComponent<InputHandler>();
                ih.playerInput = charM.players[i].inputId;
                ih.enabled = true;
            }
        }

        //disables the announcer text
        yield return oneSec;
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        countdown = true;
    }

    void DisableControl() 
    {
        // disable the component first
        for (int i = 0; i < charM.players.Count; i++) 
        {
            //reset the variables in state manager
            charM.players[i].playerStates.ResetStateInputs();

            //for users, input handler
            if (charM.players[i].playerType == PlayerBase.PlayerType.user) 
            {
                charM.players[i].playerStates.GetComponent<InputHandler>().enabled = false;
            }
        } 
    }

    public void EndTurnFunction(bool timeOut = false) 
    {
        //automatically calls this function whenever time is out or player hp is 0
        countdown = false;

        //reset timer
        levelUI.LevelTimer.text = maxTurnTimer.ToString();

        // if timeout
        if (timeOut) 
        {
            //init announcer
            levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine1.text = "Time Out!";
            levelUI.AnnouncerTextLine1.color = Color.white;
        }
        else 
        {
            levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine1.text = "K.O.";
            levelUI.AnnouncerTextLine1.color = Color.red;
        }

        //disables the controls
        DisableControl();

        //end turn coroutine
        StartCoroutine(EndTurn());
    }

    IEnumerator EndTurn() 
    {
        //wait 3 seconds
        yield return new WaitForSeconds(3);

        //Finds who won
        PlayerBase vPlayer = FindWinningPlayer();

        if (vPlayer == null) //if function returns a null
        {
            //it means it's a draw
            levelUI.AnnouncerTextLine1.text = "Draw!";
            levelUI.AnnouncerTextLine1.color = Color.blue;

        }
        else 
        {
            //if player is the winner
            levelUI.AnnouncerTextLine1.text = vPlayer.playerId + "Wins!";
            levelUI.AnnouncerTextLine1.color = Color.red;
        }
        //waits 3 seconds
        yield return new WaitForSeconds(3);

        // checks if winner player has taken any damage
        if (vPlayer != null) 
        {
            //if not, then it's a flawless victory
            if (vPlayer.playerStates.health == 150) 
            {
                levelUI.AnnouncerTextLine2.gameObject.SetActive(true);
                levelUI.AnnouncerTextLine2.text = "Flawless Victory!";
            }
        }
        //wait for 3 seconds
        yield return new WaitForSeconds(3);

        currentTurn++; //adds to the turn counter

        bool matchOver = isMatchOver();

        if (!matchOver)
        {
            StartCoroutine(InitTurn()); //starts the loop for the next turn again
        }
        else //if match was over
        {
            for (int i = 0; i < charM.players.Count; i++) 
            {
                charM.players[i].score = 0;
                charM.players[i].hasCharacter = false; //doesn't have a character
            }
            SceneManager.LoadSceneAsync(1);
        }
    }

    bool isMatchOver() 
    {
        bool retVal = false;

        for (int i = 0; i < charM.players.Count; i++) 
        {
            if (charM.players[i].score >= maxTurns) 
            {
                retVal = true;
                break;
            }
        }
        return retVal;
    }

    PlayerBase FindWinningPlayer() 
    {
        //find who won the turn
        PlayerBase retVal = null;

        StateManager targetPlayer = null;

        //check if bothe players have equal health
        if (charM.players[0].playerStates.health != charM.players[1].playerStates.health) 
        {
            //if not, then checks who has lower health, the higher health is the winner
            if (charM.players[0].playerStates.health < charM.players[1].playerStates.health)
            {
                charM.players[1].score++;
                targetPlayer = charM.players[1].playerStates;
                levelUI.AddWinIndicator(1);
            }
            else 
            {
                charM.players[0].score++;
                targetPlayer = charM.players[0].playerStates;
                levelUI.AddWinIndicator(0);
            }

            retVal = charM.ReturnPlayerFromStates(targetPlayer);
        }
        return retVal;
    }

    public static LevelManager instance;
    public static LevelManager GetInstance() 
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

}
