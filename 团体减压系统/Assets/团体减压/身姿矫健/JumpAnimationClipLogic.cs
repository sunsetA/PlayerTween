using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/

/// <summary>
/// 跳绳记圈脚本，挂载在状态机上；当播放完一遍后计数，并调用一个函数
/// </summary>
public class JumpAnimationClipLogic : StateMachineBehaviour
{
    private int currentPlayedTimes;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int index = Mathf.FloorToInt(stateInfo.normalizedTime) ;
        if (index!= currentPlayedTimes)
        {
            currentPlayedTimes = index;
            var manager = FindObjectOfType<JumpRopeManager>();
            manager.OnJumpRopeCallback(animator);
        }
    }

}
