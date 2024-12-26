using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class RuningManManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject>  RunnderList;


    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private float SpeedStandard = 1.5f;


    [SerializeField]
    private float realSpeed;


    [SerializeField]
    private Transform TargetPos;


    [SerializeField]
    private GameObject CongradulationEffect;
    private void Start()
    {
        UserStateDetect.Instance.OnHeartValueChangeEvent += Instance_OnHeartValueChangeEvent;
        UserStateDetect.Instance.OnUserLenghChangeEvent += Instance_OnUserLenghChangeEvent;
    }

    private void Update()
    {
        OnPlayerMove();
    }
    private void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> obj)
    {
        foreach (var item in RunnderList)
        {
            item.SetActive(false);
        }
        for (int i = 0; i < obj.Count; i++)
        {
            RunnderList[i].SetActive(true);
        }
    }

    private void Instance_OnHeartValueChangeEvent(float realtimeRelaxValue)
    {
        float temp_Speed = 0;
        if (realtimeRelaxValue<10)
        {
            //10次一下，为红色，人物不动
            temp_Speed = 0;
        }
        else if (realtimeRelaxValue >= 10&&realtimeRelaxValue <20)
        {
            //11-21，正常速度，往前跑
            temp_Speed = SpeedStandard;
        }
        else if (realtimeRelaxValue >= 20&&  realtimeRelaxValue <30)
        {
            //22-30，速度慢1/2，往前跑
            temp_Speed = SpeedStandard*0.5f;
        }
        else
        {
            temp_Speed = SpeedStandard * 0.33f;
            //31次以上速度慢1/3，往前跑。
        }
        realSpeed= temp_Speed;
    }


    private void OnPlayerMove()
    {
        m_Animator.transform.position = Vector3.MoveTowards(m_Animator.transform.position, TargetPos.position, Time.deltaTime * realSpeed);

        float DestinationOffset = Vector3.Distance(m_Animator.transform.position,TargetPos.position);
        if (!CongradulationEffect.activeSelf)
        {
            if (DestinationOffset < 0.1f)
            {
                CongradulationEffect.gameObject.SetActive(true);
                //发送消息
            }
        }

    }
}
