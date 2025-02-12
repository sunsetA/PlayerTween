using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class GymRope : GameLogic
{
    public AlembicDirectorControl alembicDirectorControl;

    public Animator m_Animator;

    private float SpeedMultiplier = 1;
    public override void Start()
    {
        base.Start();
        m_Animator.speed = 0;
    }

    protected override void Instance_OnCompetitiveBreathValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        base.Instance_OnCompetitiveBreathValueChangeEvent(list);
        float myBreathData = list.Find(item => item.userID == UserStateDetect.Instance.userInfo.userID).breathRate;
        SpeedMultiplier=GetSpeedDependBreathData(myBreathData);
        m_Animator.speed=SpeedMultiplier;
        alembicDirectorControl.Play(SpeedMultiplier);

    }


    private float GetSpeedDependBreathData(float breathData)
    {
        float speed = 0;
        if (breathData<10)
        {
            speed = 0;
        }
        else if (breathData<21)
        {
            speed = 1;
        }
        else if (breathData<31)
        {
            speed = 0.5f;
        }
        else
        {
            speed = 0.33f;
        }
        return speed;
    }
}
