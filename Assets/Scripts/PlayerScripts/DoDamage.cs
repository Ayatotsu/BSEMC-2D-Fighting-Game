using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    StateManager states;

    public HandleDamageColliders.DamageType damageType;

    void Start()
    {
        states = GetComponent<StateManager>();       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<StateManager>()) 
        {
            StateManager oState = other.GetComponent<StateManager>();

            if (oState != states) 
            {
                if (!oState.currentlyAttacking) 
                {
                    oState.TakeDamage(5, damageType);
                }
                
            }
        }
    }
}
