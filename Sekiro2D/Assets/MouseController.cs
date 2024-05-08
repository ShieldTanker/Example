using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Transform mousePos;


    private void Update()
    {
        Vector2 mPos = Input.mousePosition;
        mousePos.position = mPos;
    }
}
