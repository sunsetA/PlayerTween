using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class SwimManager : GameLogic
{

    /// <summary>
    /// 游泳速度
    /// </summary>
    [SerializeField]
    private float swimSpeed;

    public float SwimSpeed
    {
        get { return swimSpeed; }
        set
        {
            if (value == 0)
            {
                if (swimSpeed == 0)
                {
                    //无事发生
                }
                else
                {
                    //TODO:停止游泳动画
                    //停止游泳
                }
            }
            else 
            {
                if (swimSpeed==0)
                {
                    //TODO:播放动画
                }
                else
                {
                    //无事发生
                }
            }
            swimSpeed = value;
        }
    }


    protected override void Instance_OnCompetitiveBreathValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        base.Instance_OnCompetitiveBreathValueChangeEvent(list);
        var breathValue = list.Find(x => x.userID == UserStateDetect.Instance.userInfo.userID).breathRate;

        SwimSpeed= GetEventDependBreathRate(breathValue);

    }


    /// <summary>
    /// 根据呼吸率获取事件对应的游泳速度
    /// </summary>
    /// <param name="breathRate">呼吸频率</param>
    /// <returns></returns>
    private float GetEventDependBreathRate(float breathRate)
    {
        float result = 0;
        if (breathRate < 10)
        {
            //为红色，人物不动
            result = 0;
        }
        else if (breathRate < 20)
        {
            //正常速度，往前游泳
            result = 1;
        }
        else if (breathRate < 30)
        {
            //速度慢，只有正常速率的1/2，往前游泳
            result = 0.5f;
        }
        else 
        {
            //还是31以上，人物不动，表示游不动 
            result = 0;
        }
        return result;
    }
}
