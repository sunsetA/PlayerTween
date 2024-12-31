using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
/*转为UTF-8*/
public class jumpControl : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //调试根据动画状态机的状态来判断是否在地面上
        //并给出慢放和快放的速度
    }
}
