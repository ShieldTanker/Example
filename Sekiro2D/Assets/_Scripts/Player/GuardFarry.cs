using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFarry : MonoBehaviour
{
    public GameObject player;

    PlayerBattle pBattle;
    public bool isFarry;
    public bool isGuard;


    public float testForce;

    private void Awake()
    {
        isFarry = false;
        pBattle = player.GetComponent<PlayerBattle>();
    }

    private void Update()
    {
        isFarry = pBattle.isFarrying;
        isGuard = pBattle.isGuard;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFarry)
        {
            Farry(collision);
        }
        else if(isGuard)
        {
            Guard(collision);
        }
    }

    private void Guard(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * testForce);

            Debug.Log("Guard");
        }
    }

    private void Farry(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            float dis = collision.transform.position.x - player.transform.position.x;

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(player.transform.position.x + dis * testForce, collision.transform.position.y));

            Debug.Log("Farry");
        }
    }
}
