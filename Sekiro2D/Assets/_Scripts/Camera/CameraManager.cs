using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerPosition;
    public Transform cusorPosition;

    private float dis;
    private void Update()
    {
        float mPosX = cusorPosition.transform.position.x - playerPosition.position.x;
        mPosX = Mathf.Clamp(mPosX, -2, +2);
        /*if (limit >= 1)
            limit = 1;
        else if (limit <= -1)
            limit = -1;*/

        dis = playerPosition.transform.position.x + mPosX;
        transform.position = new Vector3(dis, transform.position.y, transform.position.z);
    }
}
