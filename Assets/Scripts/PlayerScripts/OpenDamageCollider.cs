using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDamageCollider : StateMachineBehaviour

{
    StateManager states;
    public HandleDamageColliders.DamageType damageType;
    public HandleDamageColliders.DCType dcType;
    public float delay;

    //OnStateEnter is called when a transition starts and state machine starts to evaluate the state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null) 
            states = animator.transform.GetComponent<StateManager>();

        states.handleDC.OpenCollider(dcType, delay, damageType);
        
    }

    //OnStateUpdate is called when a transition ends and state machine finished evaluating the state
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    //OnStateExit is called when transition ends and state machine finished evaluating the state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
            states = animator.transform.GetComponent<StateManager>();

        states.handleDC.CloseColliders();
    }

    //OnStateMove is called right after Animator.OnAnimationMove(). - it processes and affects root motion should be implemented here
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);
    }

    //OnStateIK is called right after Animator.OnAnimatorIK(). - it sets up animation IK(inverse kinematics) should be implemented here
    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateIK(animator, stateInfo, layerIndex);
    }
}
