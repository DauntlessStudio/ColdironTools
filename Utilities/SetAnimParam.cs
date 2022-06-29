using UnityEngine;
using ColdironTools.EditorExtensions;

public enum ParamType
{
    Bool,
    Float,
    Int
}

public class SetAnimParam : StateMachineBehaviour
{
    [SerializeField] private string paramName;
    [SerializeField] private ParamType paramType;
    [SerializeField, ConditionalHide("paramType", 0)] private bool boolValue;
    [SerializeField, ConditionalHide("paramType", 1)] private float floatValue;
    [SerializeField, ConditionalHide("paramType", 2)] private int intValue;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (paramType)
        {
            case ParamType.Bool:
                animator.SetBool(paramName, boolValue);
                break;
            case ParamType.Float:
                animator.SetFloat(paramName, floatValue);
                break;
            case ParamType.Int:
                animator.SetInteger(paramName, intValue);
                break;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
