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

    public GameObject playerTween;

    private List<Vector3> pathPointsPosition = new List<Vector3>();



    Tween pathTween;

    private Vector3 Camera_PlayerOffset;

    private Vector3 PlayerOffset;


    Vector3 cameraOriginPos;

    float originAnimatorSpeed = 0;
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

        originAnimatorSpeed = Animators[0].speed;
        Animators[0].speed = 0;



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
                    pathTween.timeScale = 0;
                }
                else if (m_heartRate>=50&&m_heartRate<60)
                {
                    //移动速度比正常速度，低1/2
                    //navMeshAgents[item.userID].speed = NormalSpeed/2;
                    pathTween.timeScale=0.5f;
                }

                else if (m_heartRate >= 60 && m_heartRate < 90)
                {
                    //移动速度比正常速度，低1/3
                    //navMeshAgents[item.userID].speed = NormalSpeed/3;
                    pathTween.timeScale = 0.3f;
                }
                else if (m_heartRate >= 90 && m_heartRate < 100)
                {
                    //不动
                    //navMeshAgents[item.userID].speed = 0;
                    pathTween.timeScale = 0;
                }
                else if (m_heartRate >= 100 && m_heartRate < 115)
                {
                    //往下掉落，与正常上行的幅度 同等幅度
                    pathTween.timeScale = -1;
                }
                else
                {
                    //往下掉落，与正常上行的幅度 同等幅度*2 
                    pathTween.timeScale = -2;
                }
                //navMeshAgents[item.userID].speed = NormalSpeed;
            } 
        }
    }


    protected override void Instance_OnGameStartEvent()
    {
        base.Instance_OnGameStartEvent();
        if (pathTween == null)
        {
            Animators[0].speed = originAnimatorSpeed;
            pathTween = Animators[0].transform.parent.DOPath(pathPointsPosition.ToArray(), 200).SetEase(Ease.Linear).SetLookAt(0.01f).OnComplete(() =>
            {
                playerTween.SetActive(true);
                Animators[0].gameObject.SetActive(false);
                Instance_OnGameEndEvent();
            }); ; ;
        }
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
