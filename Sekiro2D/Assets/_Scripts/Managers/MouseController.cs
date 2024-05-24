using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Transform cusor;

    private void Start()
    {
        cusor = GameObject.Find("Cusor").transform;
    }
    private void Update()
    {
        Vector2 mPos = Camera.main.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y));
        cusor.position = mPos;
    }
}
