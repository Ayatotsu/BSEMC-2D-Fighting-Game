using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    #region Variables
    // Stored Components
    StateManager states;
    public StateManager enStates;

    public float changeStateTolerance = 3; //How clone is considered close combat

    public float normalRate = 1; //How fast Ai decide state will cycle on normal state
    float normalTimer;

    public float closeRate = 0.5f; //How fast AI decide state will cycle on close state
    float closeTimer;

    public float blockingRate = 1; //How long Ai will block
    float blockTimer;

    public float aiStateLife = 1; //How much time does it take to reset the Ai state
    float aiTimer;

    bool initiateAI; //When AI state has to run
    bool closeCombat; //if both are in clost combat

    bool gotRandom; // randomized variables every frame
    float storeRandom; //stores random float

    //blocking variables
    bool chechForBlocking;
    bool blocking;
    float BlockMultiplier; //not used in the end, but can add to the % if the character is going to block or take damage

    //no of times character will attack variables
    bool randomizeAttacks;
    int numberOfAttacks;
    int currentNumAttacks;

    //Jump Variables
    public float jumpRate = 1;
    float jRate;
    bool jump;
    float jTimer;

    #endregion

    public AttackPatterns[] attackPatterns;

    //AI states
    public enum AIState { closeState, normalState, resetAI }

    public AIState aiState;

    
    void Start()
    {
        states = GetComponent<StateManager>();
        
    }

    
    void Update()
    {
        //call functions
        CheckDistance();
        States();
        AIAgent();
    }
    //Holds States
    void States() 
    {
        //switch decides which timer to run or not
        switch (aiState) 
        {
            case AIState.closeState:
                CloseState(); break;
            case AIState.normalState:
                NormalState(); break;
            case AIState.resetAI:
                ResetAI(); break;
        }

        //independent to AI decide cycle
        Blocking();
        Jumping();
    }

    //Manages stuffs that agent's has to do
    void AIAgent() 
    {
        //if ai acts, Ai cycle will made a rull run
        if (initiateAI) 
        {
            //Start to reset AI process; not instant
            aiState = AIState.resetAI;
            //multiplier
            float multiplier = 0;

            //get random value
            if (!gotRandom) 
            {
                storeRandom = ReturnRandom();
                gotRandom = true;
            }

            //if not in close combat
            if (!closeCombat)
            {
                //30% chance to move
                multiplier += 30;
            }
            else 
            {
                //30% chance to attack
                multiplier -= 30;
            }

            //compares random value with the added modifiers
            if (storeRandom + multiplier < 50)
            {
                Attack();
            }
            else 
            {
                Movement();
            }

        }
    }

    void Attack()
    {
        //take a random value
        if (!gotRandom)
        {
            storeRandom = ReturnRandom();
            gotRandom = true;
        }

        //if (storeRandom < 75)
        //{
            //no of attacks will ai do
            if (!randomizeAttacks)
            {
                numberOfAttacks = (int)Random.Range(1, 4);
                randomizeAttacks = true;
            }

            // if haven't attacked more than maximum times
            if (currentNumAttacks < numberOfAttacks)
            {
                int attackNumber = Random.Range(0, attackPatterns.Length);

                StartCoroutine(OpenAttack(attackPatterns[attackNumber], 0));

                //increment to the times we attacked
                currentNumAttacks++;
            }
        //}
        /*
        else
        {
            if (currentNumAttacks < 1) //if we want to ai to use special once only
            {
                states.SpecialAttack = true;
                currentNumAttacks++;
            }
        }*/
    }

    void Movement() 
    {
        //takes a random value
        if (!gotRandom) 
        {
            storeRandom = ReturnRandom();
            gotRandom = true;
        }

        //90% chance moving close to the character/enemy
        if (storeRandom < 90)
        {
            if (enStates.transform.position.x < transform.position.x)
                states.horizontal = -1;
            else
                states.horizontal = 1;
        }
        else //stay away
        {
            if (enStates.transform.position.x < transform.position.x)
                states.horizontal = 1;
            else
                states.horizontal = -1;
        }
    }

    void ResetAI() 
    {
        aiTimer += Time.deltaTime;
        if (aiTimer > aiStateLife) 
        {
            initiateAI = false;
            states.horizontal = 0;
            states.vertical = 0;
            aiTimer = 0;

            gotRandom = false;

            //chance to switch ai state from normal to close state
            storeRandom = ReturnRandom();
            if (storeRandom < 50)
                aiState = AIState.normalState;
            else
                aiState = AIState.closeState;


            currentNumAttacks = 1;
            randomizeAttacks = false;
        }
    }

    void CheckDistance()
    {
        //take the distance
        float distance = Vector3.Distance(transform.position, enStates.transform.position);

        //compare with our tolerance
        if (distance < changeStateTolerance)
        {
            //if we are not in the process of reset the AI, change the state
            if (aiState != AIState.resetAI)
                aiState = AIState.closeState;

            //if we are close, close combat mode
            closeCombat = true;
        }
        else 
        {
            //if we are not in the process of reset the AI, change the state
            if (aiState != AIState.resetAI)
                aiState = AIState.normalState;

            //if we are close, we start moving away
            if (closeCombat) 
            {
                //take a random value
                if (!gotRandom) 
                {
                    storeRandom = ReturnRandom();
                    gotRandom = true;
                }

                //60% chance to follow the enemy
                if (storeRandom < 60) 
                {
                    Movement();
                }
            }

            //no longer in close combat
            closeCombat = false;
        }
    }
    

    void Blocking() 
    {
        //if we receive damage
        if (states.getHit) 
        {
            //get random value
            if (!gotRandom) 
            {
                storeRandom = ReturnRandom();
                gotRandom = true;
            }

            //50% chance to block
            if (storeRandom < 50) 
            {
                blocking = true;
                states.getHit = false;
                //states.blocking = true;
            }
        }

        //if blocking, starts counting
        if (blocking) 
        {
            blockTimer += Time.deltaTime;

            if (blockTimer > blockingRate) 
            {
                //states.blocking = false;
                blockTimer = 0;
            }
        }
    }

    
    void NormalState() 
    {
        normalTimer += Time.deltaTime;

        if (normalTimer > normalRate)
        {
            initiateAI = true;
            normalTimer = 0;
        }
    }

    void CloseState() 
    {
        closeTimer += Time.deltaTime;

        if (closeTimer > closeRate) 
        {
            closeTimer = 0;
            initiateAI = true;
        }
    }

    void Jumping() 
    {
        // if player jumps, or we wanted to jump
        if (!enStates.onGround || jump)
        {
            //add vertical input
            states.vertical = 1;
            jump = false; //we don't want to keep jumping or spamming jump
        }
        else 
        {
            //reset the vertical input, otherwise it will jump as always
            states.vertical = 0;
        }

        //jump timer determines on how many seconds it will run a check if character wants to jump
        jTimer += Time.deltaTime;

        if (jTimer > jRate * 10) 
        {
            //get a random value
            jRate = ReturnRandom();

            //50% chance of jumping or not
            if (jRate < 50)
            {
                jump = true;
            }
            else 
            {
                jump = false;
            }

            jTimer = 0;
        }
    }
    float ReturnRandom() 
    {
        float retVal = Random.Range(0, 101);
        return retVal;
    }

    IEnumerator OpenAttack(AttackPatterns a, int i) 
    {
        int index = i;
        float delay = a.attacks[index].delay;
        states.attack1 = a.attacks[index].attack1;
        states.attack2 = a.attacks[index].attack2;
        yield return new WaitForSeconds(delay);

        states.attack1 = false;
        states.attack2 = false;
        
        if (index < a.attacks.Length - 1) 
        {
            index++;
            StartCoroutine(OpenAttack(a, index));
        }
    }

    [System.Serializable]
    public class AttackPatterns
    {
        public AttacksBase[] attacks;
    }

    [System.Serializable]
    public class AttacksBase
    {
        public bool attack1;
        public bool attack2;
        public float delay;
    }
}

