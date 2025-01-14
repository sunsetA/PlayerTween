using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
/*转为UTF-8*/
public class ClimbMountainManager : GameLogic
{

    private List<NavMeshAgent> navMeshAgents = new List<NavMeshAgent>();

    [Header("正常的运动速度")]
    public float NormalSpeed = 1;
    public override void Start()
    {
        base.Start();
        for (int i = 0; i < UserModelList.Count; i++)
        {
            Animators.Add(UserModelList[i].GetComponent<Animator>());
            navMeshAgents.Add(UserModelList[i].GetComponent<NavMeshAgent>());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAnimation(Animators[0],true);
            //铺好地形，以及准备人物的状态机逻辑代码
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            PlayAnimation(Animators[0],false);
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            PlayAnimation(Animators[1], true);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            PlayAnimation(Animators[1], false);
        }
        else
        {
            
        }
    }

    private void PlayAnimation(Animator animator, bool run) 
    {
        if (run)
        {
            animator.SetTrigger("Climb");
        }
        else
        {
            animator.Play("End");
        }
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
                    navMeshAgents[item.userID].speed = 0;
                }
                else if (m_heartRate>=50&&m_heartRate<60)
                {
                    //移动速度比正常速度，低1/2
                    navMeshAgents[item.userID].speed = NormalSpeed/2;
                }

                else if (m_heartRate >= 60 && m_heartRate < 90)
                {
                    //移动速度比正常速度，低1/3
                    navMeshAgents[item.userID].speed = NormalSpeed/3;
                }
                else if (m_heartRate >= 90 && m_heartRate < 100)
                {
                    //不动
                    navMeshAgents[item.userID].speed = 0;
                }
                else if (m_heartRate >= 100 && m_heartRate < 115)
                {
                    //往下掉落，与正常上行的幅度 同等幅度
                }
                else
                {
                    //往下掉落，与正常上行的幅度 同等幅度*2 
                }
                navMeshAgents[item.userID].speed = NormalSpeed;
            } 
        }

    }


    protected override void Instance_OnGameStartEvent()
    {
        base.Instance_OnGameStartEvent();

        foreach (var item in Animators)
        {
            PlayAnimation(item,true);
        }
    }


    protected override void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list)
    {
        base.Instance_OnUserLenghChangeEvent(list);
        foreach (var user in list)
        {
            Animator _animator = Animators[user.userID];
            PlayAnimation(_animator,false);
        }
    }
}
