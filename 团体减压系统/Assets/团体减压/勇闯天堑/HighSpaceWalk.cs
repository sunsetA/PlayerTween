using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class HighSpaceWalk : GameLogic
{
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private float multipleSpeed = 0f;
    public override void Start()
    {
        base.Start();
    }

    protected override void Instance_OnCompetitiveBrainWaveValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        if (!isGameStart)
        {
            return;
        }
        base.Instance_OnCompetitiveBrainWaveValueChangeEvent(list);
        float brainValue = list.Find(item => item.userID == UserStateDetect.Instance.userInfo.userID).brainWave;
        float targetSpeed = GetSpeed(brainValue);

        bool iswalk = targetSpeed > 0;

        if (iswalk)
        {
            if (multipleSpeed>0)
            {
                //动画不用切换，改变速度

                playerAnimator.speed = targetSpeed;
            }
            else
            {
                playerAnimator.SetTrigger("Walk");   
            }
            multipleSpeed = targetSpeed;
        }
        else
        {
            //由运动到静止
            if (multipleSpeed>0)
            {
                playerAnimator.SetTrigger("Idle");
            }
            multipleSpeed = 0;
        }

    }

    private float GetSpeed(float brainValue)
    {
        float _speed = 0;
        if (brainValue < 21)
        {
            //红色，不动  
            _speed = 0;
        }
        else if (brainValue < 41)
        {
            //橙色，走慢1/3
            _speed = 0.33f;
        }
        else if (brainValue < 61)
        {
            //粉蓝，走慢1/2      
            _speed = 0.5f;
        }
        else if (brainValue < 81)
        {
            //浅绿，正常速度 
            _speed = 1.0f;
        }
        else
        {
            //果绿，比正常速度*2倍的速度
            _speed = 2.0f;
        }
        return _speed;
    }
}
