using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
/*转为UTF-8*/
public class ClimbMountainManager : GameLogic
{
    [Header("正常的运动速度")]
    public float NormalSpeed = 1;

    [SerializeField]
    public List<Transform> pathPoints;

    private List<Vector3> pathPointsPosition = new List<Vector3>();


    Tween pathTween;

    private Vector3 Camera_PlayerOffset;

    private Vector3 PlayerOffset;


    Vector3 cameraOriginPos;
    public override void Start()
    {
        base.Start();
        for (int i = 0; i < UserModelList.Count; i++)
        {
            Animators.Add(UserModelList[i].GetComponent<Animator>());
        }

        for (int i = 0; i < pathPoints.Count; i++)
        {
            pathPointsPosition.Add(pathPoints[i].position);
        }


        pathTween = Animators[0].transform.parent.DOPath(pathPointsPosition.ToArray(), 200).SetEase(Ease.Linear).SetLookAt(0.01f); ; ;

        PlayerOffset = Animators[0].transform.position;
        Camera_PlayerOffset= Camera.main.transform.position - Animators[0].transform.position;
        //positionOffset = Animators[0].transform.position - Camera.main.transform.position;
        cameraOriginPos = Camera.main.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pathTween.timeScale = 0.1f;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            pathTween.timeScale = 20;
        }

        Camera.main.transform.position= (Animators[0].transform.position-PlayerOffset)/* + Camera_PlayerOffset */+ cameraOriginPos;

    }


    protected override void Instance_OnCompetitiveHeartValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        base.Instance_OnCompetitiveHeartValueChangeEvent(list);

        foreach (var item in list)
        {
            if (UserStateDetect.Instance.userInfo.userID == item.userID) 
            {
                float m_heartRate = item.heartRate;
                if (m_heartRate < 50)
                {
                    //不动
                    //navMeshAgents[item.userID].speed = 0;
                }
                else if (m_heartRate>=50&&m_heartRate<60)
                {
                    //移动速度比正常速度，低1/2
                    //navMeshAgents[item.userID].speed = NormalSpeed/2;
                }

                else if (m_heartRate >= 60 && m_heartRate < 90)
                {
                    //移动速度比正常速度，低1/3
                    //navMeshAgents[item.userID].speed = NormalSpeed/3;
                }
                else if (m_heartRate >= 90 && m_heartRate < 100)
                {
                    //不动
                    //navMeshAgents[item.userID].speed = 0;
                }
                else if (m_heartRate >= 100 && m_heartRate < 115)
                {
                    //往下掉落，与正常上行的幅度 同等幅度
                }
                else
                {
                    //往下掉落，与正常上行的幅度 同等幅度*2 
                }
                //navMeshAgents[item.userID].speed = NormalSpeed;
            } 
        }
    }


    protected override void Instance_OnGameStartEvent()
    {
        base.Instance_OnGameStartEvent();

    }


    protected override void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list)
    {
        base.Instance_OnUserLenghChangeEvent(list);
        foreach (var user in list)
        {
            Animator _animator = Animators[user.userID];
        }
    }
}
