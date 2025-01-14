using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class JumpRopeManager : GameLogic
{

    public float StandardJumpRopeSpeed = 10;
    /// <summary>
    /// 用户跳绳次数字典
    /// </summary>
    public Dictionary<int, int> UserJumpRopeTimesDictionary = new Dictionary<int, int>();

    public override void Start()
    {
        base.Start();
        for (int i = 0; i < UserModelList.Count; i++)
        {
            UserJumpRopeTimesDictionary.Add(i,0);
        }
    }


    protected override void Instance_OnCompetitiveBreathValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        base.Instance_OnCompetitiveBreathValueChangeEvent(list);

    }


    protected override void Instance_OnGameStartEvent()
    {
        base.Instance_OnGameStartEvent();
    }

    protected override void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list)
    {
        base.Instance_OnUserLenghChangeEvent(list);
        foreach (var item in list)
        {
            Animators[item.userID].speed = GetJumpRopeSpeed(item.breathRate);
        }

    }

    private float GetJumpRopeSpeed(float value) 
    {
        if (value < 10) 
        {
            return 0;
            //人物不动
        }
        else if (value>=10&&value<20)
        {
            //正常速度跳绳
            return StandardJumpRopeSpeed;
        }
        else if (value>=20&&value<30)
        {
            //速度慢，只有正常速率的1/2，
            return StandardJumpRopeSpeed / 2;
        }
        else
        {
            return StandardJumpRopeSpeed / 3;
            //速度慢，只有正常速率1/3。等待一分钟，还是31以上，人物不动，表示跳不动
        }
    }
    /// <summary>
    /// 当完成跳绳时的回调
    /// </summary>
    /// <param name="animatorIndex">动画的Index</param>
    private void OnJumpRopeCallback(int animatorIndex)
    {
        UserJumpRopeTimesDictionary[animatorIndex]++;
    }

    public void OnJumpRopeCallback(Animator animator) 
    {
        var index = Animators.IndexOf(animator);
        Debug.Log("Current animator index is :"+index);
        OnJumpRopeCallback(index);
    }


}
