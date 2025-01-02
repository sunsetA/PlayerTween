using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor.Animations;
using UnityEngine;
/*转为UTF-8*/
public class CatchTree : GameLogic
{

    private Animator m_Animator;

    public GameObject Apple;
    private void Start()
    {
        m_Animator = UserModelList[0].GetComponent<Animator>();
        //m_Animator.GetBehaviour<jumpControl>().OnFailingDownEvent += CatchTree_OnJumpDown;
        //m_Animator.GetBehaviour<jumpControl>().OnDownEvent += CatchTree_OnDownEvent; ;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Jump();
        }
    }
    private void CatchTree_OnDownEvent()
    {
        Apple.SetActive(false);
    }

    private void CatchTree_OnJumpDown()
    {
        Apple.SetActive(true);
    }

    protected override void Instance_OnCompetitiveHeartValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        //var data= list.Find(x => x.userID == 0);
        //if (data!=null)
        //{
        //    if (data.heartValue>0)
        //    {
        //        JumpUp();
        //    }
        //}
        //一首凤凰于飞把记忆拉回到了那个夏天，热腾腾的牛奶，出租屋带着凉意的水泥地，傍晚昏黄的路灯
    }

    protected override void Instance_OnGameStartEvent()
    {
        base.Instance_OnGameStartEvent();
    }

    protected override void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list, System.Collections.Specialized.NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        base.Instance_OnUserLenghChangeEvent(list, notifyCollectionChangedEventArgs);
    }
    public void Jump()
    {
        m_Animator.Play("JumpPick");
    }

}
