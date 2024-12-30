using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class StoneItem : MonoBehaviour
{
    private Transform m_Stone;

    [SerializeField]
    private float m_Speed=0.01f;

    public Transform ComparedFallDownPoint;

    public List<GameObject> subItems;

    bool subMove = true;

    private void Awake()
    {
        m_Stone = this.transform;
    }

    private void Start()
    {
        UserStateDetect.Instance.OnBrainWaveValueChangeEvent += OnRelaxValueChanged;
        UserStateDetect.Instance.OnUserLenghChangeEvent += Instance_OnUserLenghChangeEvent;
        Debug.Log(ComparedFallDownPoint.transform.position.y);
    }

    private void Instance_OnUserLenghChangeEvent(List<UserStateDetect.UserData> obj, System.Collections.Specialized.NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        for (int i = 1; i < subItems.Count; i++)
        {
            subItems[i].SetActive(false);
        }
        foreach (var item in obj)
        {
            subItems[item.userID].gameObject.SetActive(true);
        }
        if (obj.Count==0)
        {
            ChangeStoneSpeed(0);
            Debug.LogError("现在没有用户了");
        }
    }

    private void ChangeStoneSpeed(float SpeedMultiply)
    {
        m_Speed = 0.01f * SpeedMultiply;
    }
    private void Update()
    {
        Vector3 offset = Camera.main.transform.forward * m_Speed;
        m_Stone.position = m_Stone.position + offset;
        SubItemsMoved(offset);
    }


    private void OnRelaxValueChanged(float relaxValue)
    {
        float speedMultiply = 0;
        if (relaxValue<=20)
        {
            //红色，词条成红色，石头没有移动，或者移动过程中，停止下来  
            speedMultiply = 0;
        }
        else if (relaxValue>20&&relaxValue<=40)
        {
            //橙色，词条成橙色，推动速度慢1/3     
            speedMultiply = 0.33f;
        }
        else if (relaxValue > 20 && relaxValue <= 40)
        {
            //粉蓝，词条成粉蓝，推动速度慢1/2 
            speedMultiply = 0.5f;
        }
        else if (relaxValue > 20 && relaxValue <= 40)
        {
            speedMultiply = 1;
            //浅绿，词条成浅绿，正常推动速度
        }
        else 
        {
            speedMultiply = 1.2f;
            //果绿，词条成果绿，推动速度*2
        }
        ChangeStoneSpeed(speedMultiply);
    }


    private void SubItemsMoved(Vector3 offset)
    {

        if (ComparedFallDownPoint.position.y<336f)
        {
            subMove = false;
        }
        else
        {
            if (subMove) 
            {
                foreach (var item in subItems)
                {
                    item.transform.position += offset;
                }
            }
        }
    }
}
