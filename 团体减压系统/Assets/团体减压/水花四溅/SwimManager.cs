using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class SwimManager : GameLogic
{

    [SerializeField]
    public class SwimModel
    {
        public int AnimatorIndex = 0;
        /// <summary>
        /// 游泳速度
        /// </summary>
        [SerializeField]
        private float swimSpeed;

        public float SwimSpeed
        {
            get { return swimSpeed; }
            set
            {
                if (value == 0)
                {
                    if (swimSpeed == 0)
                    {
                        //无事发生
                    }
                    else
                    {
                        //TODO:停止游泳动画
                        //停止游泳
                        swimAnimator?.Play("Idle");
                    }
                }
                else
                {
                    if (swimSpeed == 0)
                    {
                        //TODO:播放动画
                        swimAnimator?.Play("Swim");
                    }
                    else
                    {
                        //无事发生
                    }
                }
                swimSpeed = value;
            }
        }

        public Animator swimAnimator;

        public Vector3 Destination;
    }
    public List<SwimModel> SwimModels = new List<SwimModel>();

    public GameObject ArrowItem;
    public override void Start()
    {
        base.Start();
        for (int i = 0; i < Animators.Count; i++)
        {
            SwimModel _swimModel = new SwimModel();
            _swimModel.AnimatorIndex = i;
            _swimModel.swimAnimator = Animators[i];
            _swimModel.SwimSpeed = 0;
            _swimModel.Destination = Animators[i].transform.position + Animators[i].transform.forward * 48;
            _swimModel.swimAnimator.Play("Idle");
            SwimModels.Add(_swimModel);
        }
    }

    private void LateUpdate()
    {
        foreach (var item in SwimModels)
        {
            item.swimAnimator.transform.position=Vector3.MoveTowards(item.swimAnimator.transform.position, item.Destination, Time.deltaTime * item.SwimSpeed * 0.5f);
            //item.swimAnimator.transform.position = Vector3.Lerp(item.swimAnimator.transform.position, item.Destination, Time.deltaTime * item.SwimSpeed * 0.02f);
            if (Vector3.Distance(item.swimAnimator.transform.position, item.Destination) < 0.2f)
            {
                Instance_OnGameEndEvent();
            }
        }
    }


    protected override void Instance_OnCompetitiveBreathValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        base.Instance_OnCompetitiveBreathValueChangeEvent(list);
        foreach (var item in list)
        {
            int userID= item.userID;

            var _breathValue = item.breathRate;
            SwimModels[userID].SwimSpeed = GetEventDependBreathRate(_breathValue);
        }

    }

    protected override void Instance_OnGameStartEvent()
    {
        base.Instance_OnGameStartEvent();
        Camera.main.transform.SetParent(Animators[UserStateDetect.Instance.userInfo.userID].transform );
        ArrowItem.transform.SetParent(Animators[UserStateDetect.Instance.userInfo.userID].transform);
        ArrowItem.transform.localPosition = Vector3.zero;

    }

    /// <summary>
    /// 根据呼吸率获取事件对应的游泳速度
    /// </summary>
    /// <param name="breathRate">呼吸频率</param>
    /// <returns></returns>
    private float GetEventDependBreathRate(float breathRate)
    {
        float result = 0;
        if (breathRate < 10)
        {
            //为红色，人物不动
            result = 0;
        }
        else if (breathRate < 20)
        {
            //正常速度，往前游泳
            result = 1;
        }
        else if (breathRate < 30)
        {
            //速度慢，只有正常速率的1/2，往前游泳
            result = 0.5f;
        }
        else 
        {
            //还是31以上，人物不动，表示游不动 
            result = 0;
        }
        return result;
    }



}
