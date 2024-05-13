using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerBattleState
{
    Idle,
    Attack,
    Guard,
    Farrying,
    Hit,
    Die
}

public class PlayerBattle : MonoBehaviour
{
    public Animator playerAnim;
<<<<<<< HEAD
    public LayerMask enemyLayer;
=======
>>>>>>> parent of 2a81b13 (íŒ¨ë§ ì‚¬ìš´ë“œ ì¶”ê°€)

    //ÇÃ·¹ÀÌ¾î »óÅÂ
    public static PlayerBattleState playerBattleState;
    bool isGround;
<<<<<<< HEAD
    private Rigidbody2D rb;
=======

    public float testForce;

    public Collider2D[] enemyCollider;
    public Collider2D[] enemyObj;
>>>>>>> parent of 81b66e7 (íŒ¨ë§ íŒì • ë³€ê²½ ë° ì  ì• ë‹ˆë©”ì´ì…˜ ìˆ˜ì •)


    // °ø°İ
    public Transform battlePoint;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    public static int attackCombo = 0;
    private bool isAttack;
    public Vector2 battleBoxSize;

    // ¹æ¾î
    public bool chkEnemyAttack;
    public bool inputGuard;
    private bool checkGuard = false;
    public bool isGuard;

    public bool isFarrying;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;

    // Ã¼·Â
    public float playerHp;
    public bool playerHit;

<<<<<<< HEAD
    // °ø°İ °ü·Ã
    static public bool isAttack;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    private int attackCombo = 0;

    // °ø°İ ¹üÀ§ °ü·Ã
    public Transform battlePoint;
    public Vector2 battleBoxSize;
    public LayerMask enemyLayer;
    public Collider2D[] enemyObj;

    // ¹æ¾î,ÆĞ¸µ
    private bool inputGuard;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;
<<<<<<< HEAD
=======
>>>>>>> parent of 81b66e7 (íŒ¨ë§ íŒì • ë³€ê²½ ë° ì  ì• ë‹ˆë©”ì´ì…˜ ìˆ˜ì •)
=======
>>>>>>> parent of 2a81b13 (íŒ¨ë§ ì‚¬ìš´ë“œ ì¶”ê°€)

    private void Update()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        isGround = gameObject.GetComponent<PlayerMovement>().grounded;
        KeyInput();
        Attack();
        Guard();
    }

    public void KeyInput()
    {
        // °ø°İ
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        {
            isAttack = true;
        }
        
        //¹æ¾î
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            inputGuard = true;
            checkGuard = true;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            inputGuard = false;

            playerAnim.SetBool("idleGuard", false);

            // Debug.Log("ResetFarryTime");
            farryTime = resetFarryTime;
        }
    }

    public void Attack()
    {

        if (isAttack)
        {
            // °ø°İ°¡´É ½Ã°£ÀÌ °ø°İ µô·¹ÀÌ ½Ã°£º¸´Ù ¸¹À»¶§
            if (attackTimeCount > delayAttackTime)
            {
                //°ø°İ¸ğ¼Ç
<<<<<<< HEAD
                AttackCombo(2);
=======
                attackCombo++;
                if (attackCombo > 2)
                {
                    attackCombo = 1;
                    attackTimeCount = 0;
                }

>>>>>>> parent of 81b66e7 (íŒ¨ë§ íŒì • ë³€ê²½ ë° ì  ì• ë‹ˆë©”ì´ì…˜ ìˆ˜ì •)
                // ÇÃ·¹ÀÌ¾î »óÅÂ
                playerBattleState = PlayerBattleState.Attack;
                // ÇÃ·¹ÀÌ¾î ¾Ö´Ï¸ŞÀÌ¼Ç Àç»ı
                playerAnim.SetTrigger("isAttack" + attackCombo);
                
                attackTimeCount = 0;
            }
        }

        //°ø°İ °¡´É ½Ã°£ÀÌ ¸®¼Â ½Ã°£ º¸´Ù ÀÛÀ»¶§
        if (attackTimeCount < resetComboTime)
            attackTimeCount += Time.deltaTime;
        else
        {
            attackCombo = 0;

            isAttack = false;
        }
    }
<<<<<<< HEAD
    private void ResetAttackComboTimeCount(float resetTime)
    {
        if (attackTimeCount < resetComboTime)
            attackTimeCount += Time.deltaTime;
        else
        {
            attackCombo = 0;
            isAttack = false;
=======

    public void Guard()
    {
        if (farryTime > 0 && inputGuard)
            farryTime -= Time.deltaTime;

        if (inputGuard)
        {
            // Ã³À½ °¡µå ¿Ã¸®±â
            if (checkGuard)
            {
                playerBattleState = PlayerBattleState.Guard;
                playerAnim.SetTrigger("isGuard");
                playerAnim.SetBool("idleGuard", true);

                // ÆĞ¸µ Å¸ÀÌ¹Ö È®ÀÎ ºÎºĞ
                if (farryTime > 0)
                {
                    // ÀûÀÇ °ø°İ ¸Å¼Òµå È£Ãâ
                }
            }

            // ÆĞ¸µÅ¸ÀÌ¹Ö¿¡ °ø°İ°¨Áö
            if (chkEnemyAttack)
            {
                isFarrying = true;

                // ÆĞ¸µ ¸ğ¼Ç
                farryCount++;
                if (farryCount > 2)
                    farryCount = 1;
                // ÇÃ·¹ÀÌ¾î »óÅÂ
                playerBattleState = PlayerBattleState.Farrying;
                // ÇÃ·¹ÀÌ¾î ¾Ö´Ï¸ŞÀÌ¼Ç
                playerAnim.SetTrigger("isFarry" + farryCount);

                farryTime = resetFarryTime;

                chkEnemyAttack = false;

                inputGuard = false;
                checkGuard = false;
            }

            // ÆĞ¸µ Å¸ÀÌ¹Ö ÀÌ ¾Æ´Ï°Å³ª °ø°İ°¨Áö ¾Æ´Ò¶§
            else if(farryTime <= 0 && checkGuard)
            {
                checkGuard = false;

                isGuard = true;

                playerBattleState = PlayerBattleState.Guard;
                playerAnim.SetTrigger("isGuard");
                playerAnim.SetBool("idleGuard", true);
            }
>>>>>>> parent of 81b66e7 (íŒ¨ë§ íŒì • ë³€ê²½ ë° ì  ì• ë‹ˆë©”ì´ì…˜ ìˆ˜ì •)
        }
    }


    // Attack1, Attack2 ¾Ö´Ï¸ŞÀÌ¼Ç ¿¡¼­ Add Event ·Î È£Ãâ
    public void AttackEnemy()
    {
        enemyObj = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f, enemyLayer);

        foreach (Collider2D col in enemyObj)
        {
<<<<<<< HEAD
            // ¿©±â¾È¿¡ Àû ÇÇ°İ ³ÖÀ»°Í
            Debug.Log("EnemyHit");
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
<<<<<<< HEAD
        }
    }
=======
            if (col.gameObject.tag == "Enemy")
            {
                // ¿©±â¾È¿¡ Àû ÇÇ°İ ³ÖÀ»°Í
                Debug.Log("EnemyHit");
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * testForce,ForceMode2D.Impulse);
            }
        }
    }

    // Farry1, Farry2 ¾Ö´Ï¸ŞÀÌ¼Ç ¿¡¼­ Add Event ·Î È£Ãâ
    public void FarryAttack()
    {
        enemyCollider = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyCollider)
        {
            if (col.gameObject.tag == "Enemy")
            {
                // ¿©±â¾È¿¡ ÆĞ¸® ±â´É ³ÖÀ»°Í
                Debug.Log("FarryAttack");
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.right * testForce, ForceMode2D.Impulse);
            }
        }
    } 

    // ¹æ¾î ÆÇÁ¤
    public void GuardAttack()
    {
        enemyCollider = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyCollider)
        {
            if (col.gameObject.tag == "Enemy")
            {
                // ¿©±â¾È¿¡ °¡µå ±â´É ³ÖÀ»°Í
                Debug.Log("GuardAttack");

                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * testForce, ForceMode2D.Impulse);
            }
        }
    }

    // ¾Ö´Ï¸ŞÀÌ¼Ç Add Event ¿¡ ³Ö¾îÁü
    public void SetStateIdle()
    {
        playerBattleState = PlayerBattleState.Idle;
        isAttack = false;
        isFarrying = false;
        isGuard = false;
    }

>>>>>>> parent of 81b66e7 (íŒ¨ë§ íŒì • ë³€ê²½ ë° ì  ì• ë‹ˆë©”ì´ì…˜ ìˆ˜ì •)

<<<<<<< HEAD
    IEnumerator GuardOrFarry()
    {
        enemyCollider = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyCollider)
        {
            if (col.gameObject.tag == "Enemy")
            {
                Debug.Log("Farry");
                yield break;
            }
        }

        yield return new WaitForSeconds(resetFarryTime);
<<<<<<< HEAD

        if (inputGuard)
        {
            playerBattleState = PlayerBattleState.Guard;
        }
    }
    public void Farryed()
    {
        // ÇÃ·¹ÀÌ¾î ÆĞ¸µ ¾Ö´Ï¸ŞÀÌ¼Ç Àç»ı
        playerAnim.SetTrigger("isFarry" + Random.Range(1, 3));
        playerAnim.SetBool("idleGuard", false);

        int randomIdx = Random.Range(0, 3);
        audioSource.clip = farrySound[randomIdx];

        audioSource.Play();
    }
    // °¡µå ¾Ö´Ï¸ŞÀÌ¼Ç È°¼ºÈ­
    private void SetAnimationGuard()
    {
        playerAnim.SetTrigger("isGuard");
        playerAnim.SetBool("idleGuard", true);
    }

    // ÇÃ·¹ÀÌ¾î µ¥¹ÌÁö ÀÔÀ½
    public void TakeDamage(float damage)
    {
        if (playerBattleState == PlayerBattleState.Die)
            return;

        SetStateIdle();

        if (playerHp > 0)
        {
            playerHp -= damage;
            // ³ªÁß¿¡ ÇÇ°İ ¾Ö´Ï¸ŞÀÌ¼Ç ³Ö±â
        }
        else
        {
            playerBattleState = PlayerBattleState.Die;
            // ³ªÁß¿¡ »ç¸Á ¾Ö´Ï¸ŞÀÌ¼Ç ³Ö±â
        }
    }
=======
>>>>>>> parent of 2a81b13 (íŒ¨ë§ ì‚¬ìš´ë“œ ì¶”ê°€)
=======
        }
    }

>>>>>>> parent of 2a81b13 (íŒ¨ë§ ì‚¬ìš´ë“œ ì¶”ê°€)
    public IEnumerator KnockBack(Transform enemy, float knockBackForce, float knockBackTime)
    {
        playerBattleState = PlayerBattleState.Hit;

        float knockBackVecX = gameObject.transform.position.x - enemy.position.x;
        
        if (knockBackVecX > 0)
            knockBackVecX = 1;
        else if (knockBackVecX < 0)
            knockBackVecX = -1;

        Vector2 knockBackVec = new Vector2(knockBackVecX, transform.position.y);

        rb.AddForce(knockBackVec * knockBackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockBackTime);

        rb.velocity = Vector2.zero;

        SetStateIdle();
    }

    private void AttackCombo(int maxCombo)
    {
        //°ø°İ¸ğ¼Ç
        attackCombo++;
        if (attackCombo > maxCombo)
        {
            attackCombo = 1;
            attackTimeCount = 0;
        }
    }
    IEnumerator GuardOrFarry()
    {
        SetAnimationGuard();
        playerBattleState = PlayerBattleState.Farrying;
        
        yield return new WaitForSeconds(resetFarryTime);

        if (inputGuard)
        {
            playerBattleState = PlayerBattleState.Guard;
        }
    }

    // °¡µå ¾Ö´Ï¸ŞÀÌ¼Ç È°¼ºÈ­
    private void SetAnimationGuard()
    {
        playerAnim.SetTrigger("isGuard");
        playerAnim.SetBool("idleGuard", true);
    }

    // ÇÃ·¹ÀÌ¾î µ¥¹ÌÁö ÀÔÀ½
    public void TakeDamage(float damage)
    {
        if (playerBattleState == PlayerBattleState.Die)
            return;

        if (playerHp > 0)
        {
            playerHp -= damage;
            // ³ªÁß¿¡ ÇÇ°İ ¾Ö´Ï¸ŞÀÌ¼Ç ³Ö±â
        }
        else
        {
            playerBattleState = PlayerBattleState.Die;
            // ³ªÁß¿¡ »ç¸Á ¾Ö´Ï¸ŞÀÌ¼Ç ³Ö±â
        }
    }

    // ¾Ö´Ï¸ŞÀÌ¼Ç Add Event ¿¡ ³Ö¾îÁü
    public void SetStateIdle()
    {
        playerBattleState = PlayerBattleState.Idle;
        isAttack = false;
        inputGuard = false;
=======
>>>>>>> parent of 81b66e7 (íŒ¨ë§ íŒì • ë³€ê²½ ë° ì  ì• ë‹ˆë©”ì´ì…˜ ìˆ˜ì •)
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(battlePoint.position, battleBoxSize);
    }
}
