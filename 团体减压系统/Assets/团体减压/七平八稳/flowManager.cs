using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*转为UTF-8*/
public class flowManager : GameLogic
{
    public float RotateAnle = 10;

    public List<Transform>  TableLegs;
    public List<Transform> TablePoint;

    public Transform table;

    Quaternion tableRotation=new Quaternion(0,0,0,0);
    
    public override void Start()
    {
        base.Start();
        m_OnGameStartEvent.AddListener(StartShakeRotate);
    }
    private void Update()
    {
        if (!isGameStart)
        {
            return;
        }
        for (int i = 0; i < TableLegs.Count; i++)
        {
            TableLegs[i].transform.LookAt(TablePoint[i]);
            float dis=Vector3.Distance(TableLegs[i].position, TablePoint[i].position);
            TableLegs[i].localScale=new Vector3(0.2f,0.2f,dis-0.1f);
        }
        table.rotation = Quaternion.Lerp(table.rotation, tableRotation, Time.deltaTime * 0.02f);
        //tableRotation.eulerAngles = new Vector3(Mathf.Clamp(tableRotation.eulerAngles.x, -8, 8), 0, Mathf.Clamp(tableRotation.eulerAngles.z, -2, 2));


    }


    protected override void Instance_OnHeartValueChangeEvent(List<UserStateDetect.UserData> _userdata)
    {
        base.Instance_OnHeartValueChangeEvent(_userdata);
        float averageHeartValue = _userdata.Average(x => x.heartRate);
        if (averageHeartValue<60)
        {
            RotateAnle = 0;

        }
        else if (averageHeartValue>=60&&averageHeartValue<90)
        {
            RotateAnle = 0;


        }
        else if (averageHeartValue >= 90 && averageHeartValue < 99)
        {
            RotateAnle = 2f;

        }
        else if (averageHeartValue>=99&&averageHeartValue<115)
        {
            RotateAnle = 3.2f;

            //限定tableRotation转化成欧拉角后的x,z轴的角度在-10到10之间
        }
        else if (averageHeartValue >= 115 && averageHeartValue < 130)
        {
            RotateAnle = 4.4f;
        }
        else
        {
            RotateAnle = 5.6f;
        }
        tableRotation *= Quaternion.Euler(Random.Range(-RotateAnle, RotateAnle), 0, Random.Range(-RotateAnle, RotateAnle));

    }

    private void StartShakeRotate()
    {

    }
}
