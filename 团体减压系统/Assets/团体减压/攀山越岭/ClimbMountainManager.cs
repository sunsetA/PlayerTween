using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class ClimbMountainManager : MonoBehaviour
{
    public List<Animator> m_Animator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAnimation(m_Animator[0],true);
            //铺好地形，以及准备人物的状态机逻辑代码
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            PlayAnimation(m_Animator[0],false);
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            PlayAnimation(m_Animator[1], true);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            PlayAnimation(m_Animator[1], false);
        }
        else
        {
            
        }
    }

    private void PlayAnimation(Animator animator, bool run) 
    {
        if (run)
        {
            animator.SetTrigger("Climb");
        }
        else
        {
            animator.Play("End");
        }
    }
}
