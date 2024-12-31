using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor.Animations;
using UnityEngine;
/*转为UTF-8*/
public class StrengthChallange : GameLogic
{
    public float StandardSpeed=0.2f;

    public Transform mainObject;

    private void Update()
    {
        Instance_OnCompetitiveHeartValueChangeEvent(null);
    }
    protected override void Instance_OnCompetitiveHeartValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        //分组后，比较心率大小
        //var groupOne= list.FindAll(x => x.userID < 3);
        //var groupTwo = list.FindAll(x => x.userID >= 3);

        float averageHeartOne = 86;
        float averageHeartTwo = 110;

        mainObject.position+= mainObject.forward*(averageHeartOne-averageHeartTwo)/100f*StandardSpeed*Time.deltaTime;


    }

    protected override void Instance_OnGameStartEvent()
    {
        base.Instance_OnGameStartEvent();

    }

    protected override void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        base.Instance_OnUserLenghChangeEvent(list, notifyCollectionChangedEventArgs);
    }


    private void SetAnimators(bool isStart)
    {
        for (global::System.Int32 i = 0; i < UserModelList.Count; i++)
        {
            UserModelList[i].GetComponent<Animator>().SetBool("Start", isStart);
        }
    }
}
