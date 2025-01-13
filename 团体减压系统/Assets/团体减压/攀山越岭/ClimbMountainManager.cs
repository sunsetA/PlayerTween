using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class ClimbMountainManager : MonoBehaviour
{
    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        //从自身获取组件并赋值状态机
        m_Animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayAnimation(bool run) 
    {
        m_Animator.Play(run?"":"");
    }
}
