using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*转为UTF-8*/
public class testNavMesh : MonoBehaviour
{

    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent= GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            //从摄像机发射一条射线到鼠标点击的位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //如果射线击中了地面
                if (hit.collider.name == "Terrain")
                {
                    //设置寻路的目标点
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
