using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/

public abstract class BaseGameLogic : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject>UserModelList;


    private void Start()
    {
        UserStateDetect.Instance.OnBrainWaveValueChangeEvent += Instance_OnBrainWaveValueChangeEvent;
        UserStateDetect.Instance.OnHeartValueChangeEvent += Instance_OnHeartValueChangeEvent;
        UserStateDetect.Instance.OnBreathValueChangeEvent += Instance_OnBreathValueChangeEvent;
        UserStateDetect.Instance.OnUserLenghChangeEvent += Instance_OnUserLenghChangeEvent;

        UserStateDetect.Instance.OnCompetitiveBrainWaveValueChangeEvent += Instance_OnCompetitiveBrainWaveValueChangeEvent;
        UserStateDetect.Instance.OnCompetitiveBreathValueChangeEvent += Instance_OnCompetitiveBreathValueChangeEvent;
        UserStateDetect.Instance.OnCompetitiveHeartValueChangeEvent += Instance_OnCompetitiveHeartValueChangeEvent;
        UserStateDetect.Instance.OnGameStartEvent += Instance_OnGameStartEvent;
    }

    protected abstract void Instance_OnCompetitiveHeartValueChangeEvent(List<UserStateDetect.UserData> list);


    protected abstract void Instance_OnCompetitiveBreathValueChangeEvent(List<UserStateDetect.UserData> list);


    protected abstract void Instance_OnCompetitiveBrainWaveValueChangeEvent(List<UserStateDetect.UserData> list);


    protected abstract void Instance_OnBreathValueChangeEvent(float obj);

    protected abstract void Instance_OnBrainWaveValueChangeEvent(float obj);

    protected abstract void Instance_OnHeartValueChangeEvent(float obj);

    protected abstract void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list, System.Collections.Specialized.NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs);

    protected abstract void Instance_OnGameStartEvent();


}

