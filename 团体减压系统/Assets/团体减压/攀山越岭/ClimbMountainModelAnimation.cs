using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class ClimbMountainModelAnimation : MonoBehaviour
{
    public Transform flag;

    public Transform HandJoint;
    public void SetParent() 
    {
        flag.transform.SetParent(HandJoint);
    }
}
