using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class GameLogic : BaseGameLogic
{
    protected override void Instance_OnBrainWaveValueChangeEvent(float obj)
    {
        return;
    }

    protected override void Instance_OnBreathValueChangeEvent(float obj)
    {
        return;
    }
    protected override void Instance_OnHeartValueChangeEvent(float obj)
    {
        return;
    }

    protected override void Instance_OnCompetitiveBrainWaveValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        return; 
    }

    protected override void Instance_OnCompetitiveBreathValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        return; 
    }

    protected override void Instance_OnCompetitiveHeartValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        return;
    }

    protected override void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list, System.Collections.Specialized.NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
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
    }



}
