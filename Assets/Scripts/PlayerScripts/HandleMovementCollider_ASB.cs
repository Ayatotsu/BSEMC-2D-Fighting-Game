using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMovementCollider_ASB : StateMachineBehaviour
{
    StateManager states;

    public int index;

    //ONStateEnter is call when transition parts and state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null) 
            states = animator.transform.GetComponentInParent<StateManager>();

        states.CloseMovementCollider(index);
    }

    //OsStateUpdate is called on each update frame between OnstateEnter and OnstateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    //OnStateExit is called when transition ends and state machine finished evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
            states = animator.transform.GetComponent<StateManager>();

        states.OpenMovementCollider(index);
    }
}
