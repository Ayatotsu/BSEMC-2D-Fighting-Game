using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public int health = 150;
    public float horizontal;
    public float vertical;
    public bool attack1;
    public bool attack2;
    public bool attack3;
    public bool crouch;

    public bool canAttack;
    public bool getHit;
    public bool currentlyAttacking;

    public bool dontMove;
    public bool onGround;
    public bool lookRight;

    public Slider healthSlider;
    SpriteRenderer sRenderer;


    [HideInInspector]
    public HandleDamageColliders handleDC;
    [HideInInspector]
    public HandleAnimations handleAnim;
    [HideInInspector]
    public HandleMovement handleMovement;

    public GameObject[] movementColliders;
    void Start()
    {
        handleDC = GetComponent<HandleDamageColliders>();
        handleAnim = GetComponent<HandleAnimations>();
        handleMovement = GetComponent<HandleMovement>();
        sRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        sRenderer.flipX = lookRight;
        ///onGround = isOnGournd();

        if (healthSlider != null) 
        {
            healthSlider.value = health * 0.01f;
        }
    }

    //bool isOnGround() {}

    public void ResetStateInputs() 
    {
        horizontal = 0;
        vertical = 0;
        attack1 = false;
        attack2 = false;
        attack3 = false;
        crouch = false;
        getHit = false;
        currentlyAttacking = false;
        dontMove = false;
    }

    public void CloseMovementCollider(int index) 
    {
        movementColliders[index].SetActive(false);
    }

    public void OpenMovementCollider(int index) 
    {
        movementColliders[index].SetActive(true);
    }

    ///public void TakeDamage(int damage, HandleDamageColliders.DamageType damageType) { }




}
