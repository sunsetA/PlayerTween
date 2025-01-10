using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class jumpControl : StateMachineBehaviour
{
    public float process;

    protected float jumpHeight = 9.998163f;

    public float addHeight = 0;
    /// <summary>
    /// 下落事件
    /// </summary>
    public event Action OnFailingDownEvent;

    /// <summary>
    /// 落地事件
    /// </summary>
    public event Action OnDownEvent;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //调试根据动画状态机的状态来判断是否在地面上
        //并给出慢放和快放的速度
        process=stateInfo.normalizedTime%1;
        if (process>=0.35f&&process<0.55f)
        {
            //animator.speed=0.05f;
            //OnJumpUp?.Invoke();
            //animator.transform.position += Vector3.up * 0.002f;
            float Targetheight=jumpHeight+addHeight;
            float currentHeight = jumpHeight+ addHeight * ((process - 0.35f) / (0.55f - 0.35f));
            Vector3 currentPos = animator.transform.position;
            animator.transform.position = new Vector3(currentPos.x,currentHeight,currentPos.z);
        }
        else if (process >= 0.55f && process < 0.8f)
        {
            //animator.speed = 0.05f;
            //OnJumpDown?.Invoke();
            //animator.transform.position -= Vector3.up * 0.008f;
            //14*12
            float currentHeight =(addHeight+jumpHeight)- addHeight * ((process - 0.55f) / (0.8f - 0.55f));
            Vector3 currentPos = animator.transform.position;
            animator.transform.position = new Vector3(currentPos.x, currentHeight, currentPos.z);
            OnFailingDownEvent?.Invoke();
        }
        //else if(process>0.86f)
        //{

        //    OnDownEvent?.Invoke();
        //    //animator.speed = 0.3f;
        //}
    }
}
