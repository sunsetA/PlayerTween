using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
/*转为UTF-8*/
public class JumpRopeManager : GameLogic
{
    public float StandardJumpRopeSpeed = 1;
    /// <summary>
    /// 用户跳绳次数字典
    /// </summary>
    public Dictionary<int, int> UserJumpRopeTimesDictionary = new Dictionary<int, int>();

    public List<PlayableDirector>  directors;

    public List<Animator> animators; 


    public override void Start()
    {
        base.Start();
        for (int i = 0; i < UserModelList.Count; i++)
        {
            UserJumpRopeTimesDictionary.Add(i, 0);
        }

        directors[0].playableGraph.GetRootPlayable(0).SetSpeed(0);
        directors[0].gameObject.SetActive(false);
        m_OnGameStartEvent.AddListener(OnGameStartCallback);

    }

    public float playbackSpeed = 1;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            List<UserStateDetect.UserData> list = new List<UserStateDetect.UserData>();
            UserStateDetect.UserData userData = new UserStateDetect.UserData(1);
            userData.breathRate = 14;
            list.Add(userData);
            Instance_OnCompetitiveBreathValueChangeEvent(list);
        }
    }
    protected override void Instance_OnCompetitiveBreathValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        if (!isGameStart)
        {
            return;
        }
        base.Instance_OnCompetitiveBreathValueChangeEvent(list);
        float m_breathRate = list.Find(x => x.userID ==UserStateDetect.Instance.userInfo.userID).breathRate;
        StandardJumpRopeSpeed= GetJumpRopeSpeed(m_breathRate);
        if (animators[0].GetCurrentAnimatorClipInfo(0)[0].clip.name== "TugIdle")
        {
            animators[0].SetTrigger("Jump");
        }
        if (!directors[0].isActiveAndEnabled)
        {
            directors[0].gameObject.SetActive(true);
        }

        animators[0].speed = StandardJumpRopeSpeed;
        directors[0].playableGraph.GetRootPlayable(0).SetSpeed(StandardJumpRopeSpeed);

    }


    protected override void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list)
    {
        //此处只需要显示一个用户的信息，所以只需要判断一个用户的信息
        //base.Instance_OnUserLenghChangeEvent(list);
        //foreach (var item in list)
        //{
        //    Animators[item.userID].speed = GetJumpRopeSpeed(item.breathRate);
        //}
    }


    private void OnGameStartCallback()
    {
        directors[0].Play();

    }

    private float GetJumpRopeSpeed(float value) 
    {
        float _StandardJumpRopeSpeed = 0;
        if (value < 10) 
        {
            _StandardJumpRopeSpeed = 0;
            //人物不动
        }
        else if (value>=10&&value<20)
        {
            _StandardJumpRopeSpeed = 1.3f;
            //正常速度跳绳

        }
        else if (value>=20&&value<30)
        {
            //速度慢，只有正常速率的1/2，
            _StandardJumpRopeSpeed = 1f;
        }
        else
        {
            _StandardJumpRopeSpeed = 0.7f;
            //速度慢，只有正常速率1/3。等待一分钟，还是31以上，人物不动，表示跳不动
        }
        return _StandardJumpRopeSpeed;
    }
    /// <summary>
    /// 当完成跳绳时的回调
    /// </summary>
    /// <param name="animatorIndex">动画的Index</param>
    private void OnJumpRopeCallback(int animatorIndex)
    {
        UserJumpRopeTimesDictionary[animatorIndex]++;
    }

    public void OnJumpRopeCallback(Animator animator) 
    {
        var index = Animators.IndexOf(animator);
        Debug.Log("Current animator index is :"+index);
        OnJumpRopeCallback(index);
    }


}
