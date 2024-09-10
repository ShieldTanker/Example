using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour
{
    public bool IsFall { get; set; }

    PlayerBattle pB;

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        pB = GameObject.FindWithTag("Player").GetComponent<PlayerBattle>();
        IsFall = false;
    }

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsFall = true;

            pB.TakeDamage(pB.maxHp * 2);

            collision.gameObject.SetActive(false);
        }
    }
}
