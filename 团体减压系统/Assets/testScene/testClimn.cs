using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*转为UTF-8*/
public class testClimn : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    RaycastHit hit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //从鼠标位置发射一条射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //如果射线碰撞到地面
                navMeshAgent.SetDestination(hit.point);
            }
        }
    }

}
