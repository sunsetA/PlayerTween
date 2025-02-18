using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class BirdFlyManager : GameLogic
{
    [SerializeField]
    private Animation birdAnimation;

    public List<Transform> PathPointList;

    private List<Vector3> pathPointsPosition = new List<Vector3>();


    private Tween birdTween;
    public Material birdMaterial;
    public override void Start()
    {
        base.Start();
        //闲置状态
        SwitchBirdAnimation(2);
        pathPointsPosition.Clear();
        foreach (var item in PathPointList)
        {
            pathPointsPosition.Add(item.transform.position);
        }
        birdMaterial.SetColor("_Color", Color.red);
    }

    private void FixedUpdate()
    {
        
    }

    protected override void Instance_OnCompetitiveBrainWaveValueChangeEvent(List<UserStateDetect.UserData> list)
    {
        base.Instance_OnCompetitiveBrainWaveValueChangeEvent(list);
        float brainWaveValue = list.Find(item => item.userID == UserStateDetect.Instance.userInfo.userID).brainWave;
        float birdSpeed = GetBirdSpeed(brainWaveValue);

        if (birdTween!=null)
        {
            birdTween.timeScale = birdSpeed;
            Debug.Log("Current time scale :"+birdSpeed);
        }


    }


    private float GetBirdSpeed(float brainWaveValue)
    {
        float _Speed = 0;
        if (brainWaveValue<20)
        {
            //红色，鹦鹉不动
            _Speed = 0;
            birdMaterial.SetColor("_Color", Color.red);
        }
        else if (brainWaveValue<40)
        {
            //橙色，飞翔速度慢1/3  
            _Speed = 0.33f;
            birdMaterial.SetColor("_Color",new Color(0.5f,0.5f,0.5f,1f));
        }
        else if (brainWaveValue<60)
        {
            //粉蓝，飞翔速度慢1/2 
            _Speed = 0.5f;
            birdMaterial.SetColor("_Color", new Color(0.8f, 0.43f, 0.43f, 1f));
        }
        else if (brainWaveValue<80)
        {
            //浅绿，正常飞翔速度
            _Speed = 1;
            birdMaterial.SetColor("_Color", Color.white);
        }
        else
        {
            _Speed = 2;
            birdMaterial.SetColor("_Color", new Color(0.8f,0.8f,0,1));
            //果绿，飞翔速度*2
        }
        return _Speed;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchBirdAnimation(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SwitchBirdAnimation(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchBirdAnimation(2);
        }
    }
    public void SwitchBirdAnimation(int birdState)
    {
        if (birdState == 0)
        {
            birdAnimation.Play("MacawFlap");
        }
        else if (birdState == 1)
        {
            birdAnimation.Play("MacawSoar");
        }
        else
        {
            birdAnimation.Play("MacawIdle");
        }
    }

    protected override void Instance_OnGameStartEvent()
    {
        base.Instance_OnGameStartEvent();
        if (birdTween==null)
        {
            birdTween = birdAnimation.transform.parent.DOPath(pathPointsPosition.ToArray(), 200, PathType.Linear).SetLookAt(-1f).SetEase(Ease.Linear).OnComplete(() => 
            {
                SwitchBirdAnimation(2);
            }); ;
        }
        Instance_OnGameEndEvent();

        SwitchBirdAnimation(0);
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < PathPointList.Count; i++)
        {
            if (i + 1 >= PathPointList.Count)
            {
                break;
            }
            Gizmos.DrawLine(PathPointList[i].position, PathPointList[i + 1].position);
        }
    }
}
