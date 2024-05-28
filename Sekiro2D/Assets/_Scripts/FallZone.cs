using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour
{
    public bool isFall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isFall = true;
            collision.gameObject.SetActive(false);
        }

    }
}
