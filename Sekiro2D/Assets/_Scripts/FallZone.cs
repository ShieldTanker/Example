using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour
{
    public bool isFall;

    public PlayerManager pM;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isFall = true;

            pM.PlBattleState = PlayerBattleState.Die;

            collision.gameObject.SetActive(false);
        }
    }
}
