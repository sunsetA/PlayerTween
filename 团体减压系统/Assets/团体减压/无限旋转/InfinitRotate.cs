using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/

/// <summary>
/// 合作训练无限旋转，用户心率变化
/// </summary>
public class InfinitRotate : GameLogic
{
    /// <summary>
    /// 用户动画
    /// </summary>
    public Animator playerAnimator;

    /// <summary>
    /// 桥梁动画
    /// </summary>
    public Animator BridgeAnimator;

    /// <summary>
    /// 粒子特效集合，当动画不在播放时，隐藏这个粒子特效
    /// </summary>
    public GameObject Particles;

    private float SpeedMultiple = 1.0f;

    private float originHumanPushingSpeed = 1f;
    private float originBridgeRotateSpeed = 0.15f;


    public override void Start()
    {
        base.Start();
        m_OnGameStartEvent.AddListener(OnGameStartCallback);
    }
    protected override void Instance_OnHeartValueChangeEvent(List<UserStateDetect.UserData> obj)
    {
        if (!isGameStart)
        {
            return;
        }
        base.Instance_OnHeartValueChangeEvent(obj);
        SpeedMultiple=GetSpeedMultiple(obj.Find(item => item.userID==UserStateDetect.Instance.userInfo.userID).heartRate);

        bool isPlaying = SpeedMultiple > 0;
        Particles.SetActive(isPlaying);
        if (!isPlaying)
        {
            playerAnimator.Play("TugIdle");
        }
        else
        {
            playerAnimator.SetTrigger("Pushing");
        }
        playerAnimator.speed = SpeedMultiple*originHumanPushingSpeed;
        BridgeAnimator.speed = SpeedMultiple*originBridgeRotateSpeed;


    }

    /// <summary>
    /// 游戏规则，根据心率变化调整动画速度
    /// </summary>
    /// <param name="heartRate">心率值</param>
    /// <returns>速度的系数</returns>
    private float GetSpeedMultiple(float heartRate)
    {
        if (heartRate < 60)
        {
            return 0;
        }
        else if (heartRate < 90)
        {
            return 1;
        }
        else if (heartRate < 100)
        {
            return 0.5f;
        }
        else if (heartRate < 115)
        {
            return 0.33f;
        }
        else if (heartRate < 130)
        {
            return 0.25f;
        }
        else
        {
            return 0f;
        }
    }


    private void OnGameStartCallback()
    {
        //DOTO
        BridgeAnimator.enabled = true;
    }
}
