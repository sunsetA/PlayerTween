using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
/*转为UTF-8*/

public abstract class BaseGameLogic : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject>UserModelList;

    protected List<Animator> Animators = new List<Animator>();
    /// <summary>
    /// 服务端在所有用户已经做好准备，点击的游戏开始事件
    /// </summary>
    [SerializeField]
    protected UnityEvent m_OnGameStartEvent;
    [SerializeField]
    protected bool isGameStart = false;

    [SerializeField]
    protected GameObject CelebrateParticleSystem;


    [SerializeField]
    protected UnityEvent m_OnGameEndEvent;
    public virtual void Start()
    {
        UserStateDetect.Instance.OnBrainWaveValueChangeEvent += Instance_OnBrainWaveValueChangeEvent;
        UserStateDetect.Instance.OnHeartValueChangeEvent += Instance_OnHeartValueChangeEvent;
        UserStateDetect.Instance.OnBreathValueChangeEvent += Instance_OnBreathValueChangeEvent;
        UserStateDetect.Instance.OnUserLenghChangeEvent += Instance_OnUserLenghChangeEvent;

        UserStateDetect.Instance.OnCompetitiveBrainWaveValueChangeEvent += Instance_OnCompetitiveBrainWaveValueChangeEvent;
        UserStateDetect.Instance.OnCompetitiveBreathValueChangeEvent += Instance_OnCompetitiveBreathValueChangeEvent;
        UserStateDetect.Instance.OnCompetitiveHeartValueChangeEvent += Instance_OnCompetitiveHeartValueChangeEvent;
        UserStateDetect.Instance.OnGameStartEvent += Instance_OnGameStartEvent;
        foreach (var item in UserModelList)
        {
            Animators.Add(item.GetComponent<Animator>());
        }
    }

    /// <summary>
    /// 竞赛心率变化事件回调
    /// </summary>
    /// <param name="list"></param>
    protected abstract void Instance_OnCompetitiveHeartValueChangeEvent(List<UserStateDetect.UserData> list);


    /// <summary>
    /// 竞赛呼吸变化事件回调
    /// </summary>
    /// <param name="list"></param>
    protected abstract void Instance_OnCompetitiveBreathValueChangeEvent(List<UserStateDetect.UserData> list);

    /// <summary>
    /// 对抗脑波变化事件回调
    /// </summary>
    /// <param name="list">所有的用户数据</param>

    protected abstract void Instance_OnCompetitiveBrainWaveValueChangeEvent(List<UserStateDetect.UserData> list);


    /// <summary>
    /// 用户呼吸变化事件回调
    /// </summary>
    /// <param name="obj"></param>
    protected abstract void Instance_OnBreathValueChangeEvent(List<UserStateDetect.UserData> obj);

    /// <summary>
    /// 用户脑波变化事件回调
    /// </summary>
    /// <param name="obj"></param>
    protected abstract void Instance_OnBrainWaveValueChangeEvent(List<UserStateDetect.UserData> obj);


    /// <summary>
    /// 用户心率变化事件回调
    /// </summary>
    /// <param name="obj"></param>
    protected abstract void Instance_OnHeartValueChangeEvent(List<UserStateDetect.UserData> obj);

    /// <summary>
    /// 用户数量变化事件回调
    /// </summary>
    /// <param name="list"></param>
    /// <param name="notifyCollectionChangedEventArgs"></param>
    protected abstract void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list);

    /// <summary>
    /// 游戏开始事件回调
    /// </summary>
    protected abstract void Instance_OnGameStartEvent();


    /// <summary>
    /// 游戏结束事件回调
    /// </summary>
    protected abstract void Instance_OnGameEndEvent();
}

