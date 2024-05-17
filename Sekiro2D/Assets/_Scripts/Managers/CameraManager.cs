using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerPosition;
    public Transform cusorPosition;

    public float camYPos;

    private float dis;

    private void LateUpdate()
    {
        float mPosX = cusorPosition.transform.position.x - playerPosition.position.x;
        mPosX = Mathf.Clamp(mPosX, -2, +2);

        float yDis = playerPosition.transform.position.y + camYPos;

        dis = playerPosition.transform.position.x + mPosX;
        transform.position = new Vector3(dis, yDis, transform.position.z);
    }
}
