using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressedBehaviour : StateMachineBehaviour
{
    public static Dictionary<string, System.Action> buttonFunctionTable;//按键功能表 <按键名，按钮的功能函数>
    private void Awake()
    {
        buttonFunctionTable = new Dictionary<string, System.Action>();
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIInput.Instance.DisableAllInputs();//按下按钮后禁用所有操作以免重复按键
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        buttonFunctionTable[animator.gameObject.name]?.Invoke();//动画结束后才调用按键的功能，避免按键动画显示不完全
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
