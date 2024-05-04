using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public string playerInput;

    float horizontal;
    float vertical;
    bool attack1;
    bool attack2;
    bool attack3;

    StateManager stateManager;
    void Start()
    {
        stateManager = GetComponent<StateManager>(); 
    }

    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal" + playerInput);
        vertical = Input.GetAxis("Vertical" + playerInput);
        attack1 = Input.GetButton("Fire1" + playerInput);
        attack2 = Input.GetButton("Fire2" + playerInput);
        attack3 = Input.GetButton("Fire3" + playerInput);

        stateManager.horizontal = horizontal;
        stateManager.vertical = vertical;
        stateManager.attack1 = attack1;
        stateManager.attack2 = attack2;
        stateManager.attack3 = attack3;
    }
}
