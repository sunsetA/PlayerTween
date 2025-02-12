using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*转为UTF-8*/
public class BlowBalloon : GameLogic
{

    public float BalloonScale = 0f;
    [Header("桌子上的空气球")]
    public List<GameObject> EmptyBalloons;

    [Header("桌子上的满气球")]
    public List<GameObject> FullyBalloons;

    [SerializeField]
    private SkinnedMeshRenderer _balloonMeshRender;
  
    private int currentBalloonsIndex = 0;

    private int totalBallsCount = 0;
    public override void Start()
    {
        base.Start();
        StartCoroutine(DelayData());
    }

    private void Update()
    {

        if (!isGameStart)
        {
            return;
        }
        float currentValue = _balloonMeshRender.GetBlendShapeWeight(0);

        float lerpValue = Mathf.Lerp(currentValue, BalloonScale,Time.deltaTime); ;
        Debug.LogFormat("插值当前值为{0},目标值为{1},插值后为{2}", currentValue, BalloonScale, lerpValue);
        _balloonMeshRender.SetBlendShapeWeight(0, lerpValue);
    }
    IEnumerator DelayData()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            isGameStart = true;
            EmptyBalloons[currentBalloonsIndex].gameObject.SetActive(false);
            List<UserStateDetect.UserData> obj = new List<UserStateDetect.UserData>();

            UserStateDetect.UserData userData = new UserStateDetect.UserData(1);
            userData.breathRate = Random.Range(0,40);
            UserStateDetect.UserData userData2 = new UserStateDetect.UserData(2);
            userData2.breathRate = Random.Range(0, 40);
            obj.Add(userData);
            obj.Add(userData2);

            Debug.Log("平均心率"+ obj.Average(item=>item.breathRate));
            Instance_OnBreathValueChangeEvent(obj);
        }

    }
    protected override void Instance_OnBreathValueChangeEvent(List<UserStateDetect.UserData> obj)
    {
        base.Instance_OnBreathValueChangeEvent(obj);
        float averageBreathValue = obj.Average(x => x.breathRate);
        if (averageBreathValue < 10)
        {
            BalloonScale+= 0;
        }
        else if (averageBreathValue >= 10 && averageBreathValue < 20)
        {
            BalloonScale += 10f;
        }
        else if (averageBreathValue >= 20 && averageBreathValue < 30)
        {
            BalloonScale += 3f;
        }
        else
        {
            BalloonScale += 0f;
        }
        if (BalloonScale>=100)
        {
            BalloonScale -= 100;
            //直接设置气球大小为0，并且在桌面上放置一个完整的气球
            _balloonMeshRender.SetBlendShapeWeight(0, 0);
            FullyBalloons[currentBalloonsIndex].gameObject.SetActive(true);

            currentBalloonsIndex++;
            EmptyBalloons[currentBalloonsIndex].gameObject.SetActive(false);
        }
    }


    protected override void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> list)
    {
        base.Instance_OnUserLenghChangeEvent(list);
        for (int i = 0; i < EmptyBalloons.Count; i++)
        {
            //根据用户数量显示气球
            bool ballActiveState = i < list.Count;
            EmptyBalloons[i].gameObject.SetActive(ballActiveState);
        }
        totalBallsCount = list.Count;
    }

    protected override void Instance_OnGameStartEvent()
    {
        base.Instance_OnGameStartEvent();
    }
}
