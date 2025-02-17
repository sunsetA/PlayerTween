using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class GameLogic : BaseGameLogic
{
    public override void Start()
    {
        base.Start();
        m_OnGameEndEvent.AddListener(() => 
        {
            CelebrateParticleSystem?.gameObject.SetActive(true);
        });
    }
    protected override void Instance_OnBrainWaveValueChangeEvent(List<UserStateDetect.UserData> obj)
    {

    }

    protected override void Instance_OnBreathValueChangeEvent(List<UserStateDetect.UserData> obj)
    {

    }
    protected override void Instance_OnHeartValueChangeEvent(List<UserStateDetect.UserData> obj)
    {

    }

    protected override void Instance_OnCompetitiveBrainWaveValueChangeEvent(List<UserStateDetect.UserData> list)
    {

    }

    protected override void Instance_OnCompetitiveBreathValueChangeEvent(List<UserStateDetect.UserData> list)
    {

    }

    protected override void Instance_OnCompetitiveHeartValueChangeEvent(List<UserStateDetect.UserData> list)
    {

    }

    protected override void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list)
    {
        for (int i = 0; i < UserModelList.Count; i++)
        {
            bool isActive = false;
            foreach (var item in list)
            {
                if (item.userID == i)
                {
                    isActive = true;
                }
            }
            UserModelList[i].gameObject.SetActive(isActive);
        }
    }

    protected override void Instance_OnGameStartEvent()
    {
        isGameStart = true;
        m_OnGameStartEvent?.Invoke();
    }

    protected override void Instance_OnGameEndEvent()
    {
        m_OnGameEndEvent?.Invoke();
    }
}
