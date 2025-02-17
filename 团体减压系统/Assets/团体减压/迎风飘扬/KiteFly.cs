using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class KiteFly : GameLogic
{
    public Transform kite;

    public Transform KiteDestinationPoint;
    private Vector3 KiteDestinationPosition;

    private Vector3 originKitePos;

    public float Speed;

    public Material LineMaterial;


    public override void Start()
    {
        base.Start();
        //KiteDestinationPosition= KiteDestinationPoint.position;
        originKitePos = kite.GetChild(1).transform.localPosition;
    }

    private void Update()
    {
        kite.transform.position= Vector3.MoveTowards(kite.transform.position, KiteDestinationPoint.position, Speed * Time.deltaTime*0.2f);
        if (Vector3.Distance(kite.transform.position, KiteDestinationPoint.position)<1)
        {
            Instance_OnGameEndEvent();
        }
        kite.GetChild(1).transform.localPosition =  originKitePos ;
    }

    protected override void Instance_OnCompetitiveBrainWaveValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        if (!isGameStart)
        {
            return;
        }
        base.Instance_OnCompetitiveBrainWaveValueChangeEvent(list);
        var userBrainData = list.Find(item => item.userID == UserStateDetect.Instance.userInfo.userID).brainWave;

        SetkiteLogic(userBrainData);



    }


    private void SetkiteLogic(float brainWaveData)
    {
        if (brainWaveData<20)
        {
            //红色，风筝没放飞，或向下降落   
            Speed = 0;
            LineMaterial.SetColor("_Color",Color.red);
        }
        else if (brainWaveData < 40)
        {
            // 橙色，放飞速度慢1/3  
            Speed = 0.33f;
            LineMaterial.SetColor("_Color", new Color(1,0.5f,0,1));
        }
        else if (brainWaveData < 60)
        {
            //粉蓝，放飞速度慢1/2  
            Speed = 0.5f;
            LineMaterial.SetColor("_Color", new Color(0.4f, 0f, 1, 1));
        }
        else if (brainWaveData < 80)
        {
            //浅绿，正常放飞速度
            Speed = 1;
            LineMaterial.SetColor("_Color", new Color(0.75f, 1f, 0.5f, 1));
        }
        else
        {
            //果绿，放飞速度*2
            Speed = 2;
            LineMaterial.SetColor("_Color", new Color (0.5f, 1f, 0f, 1f));
        }

    }

}
