using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
/*转为UTF-8*/
public class CatchTree : GameLogic
{
    protected override void Instance_OnCompetitiveHeartValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        
    }

    protected override void Instance_OnGameStartEvent()
    {

    }

    protected override void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list, System.Collections.Specialized.NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        base.Instance_OnUserLenghChangeEvent(list, notifyCollectionChangedEventArgs);
    }
}
