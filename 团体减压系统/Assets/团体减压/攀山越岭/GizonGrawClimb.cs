using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class GizonGrawClimb : MonoBehaviour
{
    ClimbMountainManager climbMountainManager;
    private void OnDrawGizmos()
    {
        if (!climbMountainManager) 
        {
            climbMountainManager= FindObjectOfType<ClimbMountainManager>();
        }
        Gizmos.color = Color.red;
        for (int i = 0; i < climbMountainManager.pathPoints.Count; i++)
        {
            if (i+1>= climbMountainManager.pathPoints.Count)
            {
                break;
            }
            Gizmos.DrawLine(climbMountainManager.pathPoints[i].position, climbMountainManager.pathPoints[i+1].position);
        }
    }
}
