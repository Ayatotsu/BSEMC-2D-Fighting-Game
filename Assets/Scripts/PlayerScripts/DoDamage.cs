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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<StateManager>()) 
        {
            StateManager oState = other.GetComponentInParent<StateManager>();

            if (oState != states) 
            {
                
                oState.TakeDamage(15, damageType);
                
                
            }
        }
    }
}
