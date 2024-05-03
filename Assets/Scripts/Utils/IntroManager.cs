using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject startText; //"Press Start" reference

    float timer;
    bool loadingLevel;
    bool init;

    public int activeElement;
    public GameObject menuObj; //MainMenu Object
    public ButtonRef[] menuOptions;

    void Start()
    {
        menuObj.SetActive(false); //hides the Scene from the start of the game.
    }

    
    void Update()
    {
        if (!init)
        {
            //Flicks "Press Start" Text
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                timer = 0;
                startText.SetActive(!startText.activeInHierarchy); //this will enable and disable the Text of "Press Start"

            }
            // is Space Key is Pressed while on "Press Start" is active
            if (Input.GetKeyUp(KeyCode.Space))
            {
                init = true;
                startText.SetActive(false); //this will hide the text after pressing the Space key.
                menuObj.SetActive(true); //this will open the menu after hiding the startText.
            }
        }
        else
        {
            if (!loadingLevel) //if it is not already loading the level.
            {
                //indicate the selected option.
                menuOptions[activeElement].selected = true;

                //it will change the selected option based on input.
                if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    menuOptions[activeElement].selected = false;

                    if (activeElement > 0)
                    {
                        activeElement--;
                    }
                    else 
                    {
                        activeElement = menuOptions.Length - 1; //resets the active element
                    }
                }
                if (Input.GetKeyUp(KeyCode.DownArrow)) 
                {
                    menuOptions[activeElement].selected = false;

                    if (activeElement < menuOptions.Length - 1)
                    {
                        activeElement++;
                    }
                    else 
                    {
                        activeElement = 0;
                    }
                }
                //load next scene
                if (Input.GetKeyUp(KeyCode.Space)) 
                {
                    //once pressed, it will load the level.
                    Debug.Log("loading");
                    loadingLevel = true;
                    StartCoroutine(LoadLevel());
                    menuOptions[activeElement].transform.localScale *= 1.2f;


                }
            }
        }
    }

    void HandleSelectedOption() 
    {
        switch (activeElement) 
        {
            case 0:
                CharacterManager.GetInstance().numberOfUsers = 1; //single player
                break;
            case 1:
                CharacterManager.GetInstance().numberOfUsers = 2; //2 players
                CharacterManager.GetInstance().players[1].playerType = PlayerBase.PlayerType.user;
                break;
        }
    }

    IEnumerator LoadLevel() 
    {
        HandleSelectedOption();
        yield return new WaitForSeconds(0.5f);
        //startText.SetActive(false);
        //yield return new WaitForSeconds(1.5f);
        
        
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
}
