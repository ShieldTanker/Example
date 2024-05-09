using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerPosition;
    public Transform cusorPosition;

    public float camSpeed;

    private float offset;
    private float dis;
    private float limit;

    private void Start()
    {
        offset = gameObject.transform.position.x - playerPosition.transform.position.x;
        limit = offset;
    }
    private void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        limit += xMove * Time.deltaTime * camSpeed;
        if (limit >= 1)
            limit = 1;
        else if (limit <= -1)
            limit = -1;

        dis = playerPosition.transform.position.x + (offset * limit);
        transform.position = new Vector3(dis, transform.position.y, transform.position.z);
    }
}
