using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class SingleCameraFollow : MonoBehaviour
{
    public Transform targetTransform;
    private Vector3 Camera_PlayerOffset;

    private Vector3 PlayerOffset;
    Vector3 cameraOriginPos;
    // Start is called before the first frame update
    void Start()
    {
        PlayerOffset = targetTransform.position;
        Camera_PlayerOffset = Camera.main.transform.position - targetTransform.position;
        //positionOffset = Animators[0].transform.position - Camera.main.transform.position;
        cameraOriginPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = (targetTransform.position - PlayerOffset)/* + Camera_PlayerOffset */+ cameraOriginPos;

    }
}
