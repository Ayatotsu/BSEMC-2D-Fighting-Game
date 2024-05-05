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
        onGround = isOnGround();

        if (healthSlider != null) 
        {
            healthSlider.value = health * 0.01f;
        }

        if (health <= 0) 
        {
            if (LevelManager.GetInstance().countdown)  //fight is still running
            {
                LevelManager.GetInstance().EndTurnFunction(); //end the turn

                //handleAnim.anim.Play("Dfeat");
            }
        }
    }

    bool isOnGround() 
    {
        bool retVal = false;

        LayerMask layer = ~(1 << gameObject.layer | 1 << 3);
        retVal = Physics2D.Raycast(transform.position, -Vector2.up, 0.1f, layer);

        return retVal;
    }

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

    public void TakeDamage(int damage, HandleDamageColliders.DamageType damageType) 
    {
        if (!getHit) 
        {
            switch (damageType) 
            {
                case HandleDamageColliders.DamageType.light:
                    StartCoroutine(CloseImmortality(0.3f));
                    break;
                case HandleDamageColliders.DamageType.heavy:
                    handleMovement.AddVelocityOnCharacter(((!lookRight) ? Vector3.right * 1 : Vector3.right * -1) + Vector3.up, 0.5f);


                    StartCoroutine(CloseImmortality(1));
                    break;
            }
        }
    }

    IEnumerator CloseImmortality(float timer) 
    {
        yield return new WaitForSeconds(timer);
        getHit = false;
    }




}
