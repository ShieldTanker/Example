using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFarry : MonoBehaviour
{
    Collider2D enemyAttack;
    public Transform playerPos;
    Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyAttack = collision;
    }

    public void Guard()
    {
        rb = enemyAttack.gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up);
    }

    public void Farry()
    {
        float dis = playerPos.transform.position.x - enemyAttack.transform.position.x;

        rb = enemyAttack.gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(playerPos.position.x + dis, enemyAttack.transform.position.y));
    }
}
