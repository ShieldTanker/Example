using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    public Animator playerAnim;

    public GameObject attackPoint;
    public GameObject guardPoint;

    public float attackTime;
    public float farryTime;

    int attackCount = 0;

    float atInputTime = 0f;
    public float waitAttackTime;
    public float reAtTime;

    IEnumerator Attack(string animation)
    {
        playerAnim.SetTrigger(animation);
        playerAnim.SetBool("isAttacking", true);
        
        attackPoint.SetActive(true);
        yield return new WaitForSeconds(attackTime);
        attackPoint.SetActive(false);
    }

    
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (attackCount == 0)
            {
                StartCoroutine(Attack("isAttack1"));
                ++attackCount;
                atInputTime = waitAttackTime;
            }
            else if (attackCount == 1)
            {
                StartCoroutine(Attack("isAttack2"));
                ++attackCount;
                atInputTime = waitAttackTime;
            }
            else if (attackCount == 2)
            {
                StartCoroutine(Attack("isAttack3"));
                ++attackCount;
                atInputTime = reAtTime;
            }
        }

        if (attackCount > 0)
        {
            atInputTime -= Time.deltaTime;

            if (atInputTime <= 0)
            {
                attackCount = 0;
                playerAnim.SetBool("isAttacking", false);
            }
        }
        
    }
}
