using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerPosition;
    public Transform cusorPosition;

    public FallZone fallZone;

    public float camYPos;

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        playerPosition = GameObject.FindWithTag("Player").transform;
        cusorPosition = GameObject.FindWithTag("Cusor").transform;
        fallZone = GameObject.FindWithTag("FallZone").GetComponent<FallZone>();
    }

    private void LateUpdate()
    {
        // 낙사구간 과 카메라 멈춤이 아닐때
        if (!fallZone.IsFall && !GameManager.StopCam)
        {
            // 마우스 커서와 플레이어 와의 수평 거리 계산
            float mPosX = cusorPosition.position.x - playerPosition.position.x;
            // 수평거리 를 -2 에서 +2 로 제한
            mPosX = Mathf.Clamp(mPosX, -2, +2);

            // 따라다닐 카메라의 높이 설정
            float yDis = playerPosition.position.y + camYPos;

            // 커서 방향으로 카메라 가 이동할수 있게 계산
            float dis = playerPosition.position.x + mPosX;
            transform.position = new Vector3(dis, yDis, transform.position.z);
        }
    }
}
