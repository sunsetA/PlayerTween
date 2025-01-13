using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class flowManager : MonoBehaviour
{
    public List<Transform>  TableLegs;
    public List<Transform> TablePoint;

    public Transform table;
    private void Update()
    {
        for (int i = 0; i < TableLegs.Count; i++)
        {
            TableLegs[i].transform.LookAt(TablePoint[i]);
            float dis=Vector3.Distance(TableLegs[i].position, TablePoint[i].position);
            TableLegs[i].localScale=new Vector3(0.2f,0.2f,dis-0.1f);
        }
    }
}
