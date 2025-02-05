using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*转为UTF-8*/
public class DriveBoatMGR : MonoBehaviour
{
    public Transform boat;

    public Transform boatTarget;


    private Camera mainCamera;

    public ParticleSystem particles;

    [Header("标准速度值")]
    [SerializeField]
    private float _StandardSpeed = 10;
    [SerializeField]
    private float m_boadSpeed = 0;

    [SerializeField]
    private MeshRenderer m_FlagMeshRender;

    private Color temp_Color;
    private void Start()
    {
        mainCamera = Camera.main;
        mainCamera.transform.parent = boat;
        UserStateDetect.Instance.OnBrainWaveValueChangeEvent += Instance_OnRelaxValueChangeEvent;
    }


    private void Instance_OnRelaxValueChangeEvent(List<UserStateDetect.UserData> _realtimeRelaxValue)
    {
        float realtimeRelaxValue= _realtimeRelaxValue.Average(x => x.brainWave);
        float temp_Speed = 0;
        if (realtimeRelaxValue <= 20)
        {
            //红色，旗帜成红色，快艇没有移动，或者移动过程中，停止下来
            temp_Speed =0;
            temp_Color = Color.red;
            particles.emissionRate =0f;
        }
        else if (realtimeRelaxValue > 20 && realtimeRelaxValue <= 40)
        {
            //橙色，旗帜成橙色，移动速度慢1/3
            temp_Speed = _StandardSpeed*0.33f;
            temp_Color = new Color(0.9f,0.45f,0.1f,1f);
            particles.emissionRate = 50 * 0.33f;
        }
        else if (realtimeRelaxValue>40&&realtimeRelaxValue<=60)
        {
            //粉蓝，旗帜成粉蓝，移动速度慢1/2 
            temp_Speed = _StandardSpeed * 0.5f;
            temp_Color = new Color(1f,0.25f,0.62f,1f);
            particles.emissionRate = 50 * 0.5f;
        }
        else if (realtimeRelaxValue > 60 && realtimeRelaxValue <= 80)
        {
            //浅绿，旗帜成浅绿，正常移动速度 
            temp_Speed = _StandardSpeed * 1f;
            temp_Color = new Color(0.65f,1f,0.61f,1f);
            particles.emissionRate = 50f;
        }
        else
        {
            temp_Speed = _StandardSpeed * 2f;
            temp_Color = new Color(0,0.6f,0f,1f);
            particles.emissionRate = 50 * 2f;
            //果绿，旗帜成果绿，移动速度 * 2
        }
        m_boadSpeed = temp_Speed;
        m_FlagMeshRender.sharedMaterial.SetColor("_Color", temp_Color);
    }

    private void Update()
    {
        boat.transform.position = Vector3.MoveTowards(boat.transform.position, new Vector3(boatTarget.position.x, boat.transform.position.y, boatTarget.position.z),Time.deltaTime* m_boadSpeed);
    }
}
