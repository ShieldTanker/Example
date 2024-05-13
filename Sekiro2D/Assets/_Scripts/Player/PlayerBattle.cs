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
>>>>>>> parent of 2a81b13 (패링 사운드 추가)

    //�÷��̾� ����
    public static PlayerBattleState playerBattleState;
    bool isGround;
<<<<<<< HEAD
    private Rigidbody2D rb;
=======

    public float testForce;

    public Collider2D[] enemyCollider;
    public Collider2D[] enemyObj;
>>>>>>> parent of 81b66e7 (패링 판정 변경 및 적 애니메이션 수정)


    // ����
    public Transform battlePoint;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    public static int attackCombo = 0;
    private bool isAttack;
    public Vector2 battleBoxSize;

    // ���
    public bool chkEnemyAttack;
    public bool inputGuard;
    private bool checkGuard = false;
    public bool isGuard;

    public bool isFarrying;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;

    // ü��
    public float playerHp;
    public bool playerHit;

<<<<<<< HEAD
    // ���� ����
    static public bool isAttack;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    private int attackCombo = 0;

    // ���� ���� ����
    public Transform battlePoint;
    public Vector2 battleBoxSize;
    public LayerMask enemyLayer;
    public Collider2D[] enemyObj;

    // ���,�и�
    private bool inputGuard;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;
<<<<<<< HEAD
=======
>>>>>>> parent of 81b66e7 (패링 판정 변경 및 적 애니메이션 수정)
=======
>>>>>>> parent of 2a81b13 (패링 사운드 추가)

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
        // ����
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        {
            isAttack = true;
        }
        
        //���
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
            // ���ݰ��� �ð��� ���� ������ �ð����� ������
            if (attackTimeCount > delayAttackTime)
            {
                //���ݸ��
<<<<<<< HEAD
                AttackCombo(2);
=======
                attackCombo++;
                if (attackCombo > 2)
                {
                    attackCombo = 1;
                    attackTimeCount = 0;
                }

>>>>>>> parent of 81b66e7 (패링 판정 변경 및 적 애니메이션 수정)
                // �÷��̾� ����
                playerBattleState = PlayerBattleState.Attack;
                // �÷��̾� �ִϸ��̼� ���
                playerAnim.SetTrigger("isAttack" + attackCombo);
                
                attackTimeCount = 0;
            }
        }

        //���� ���� �ð��� ���� �ð� ���� ������
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
            // ó�� ���� �ø���
            if (checkGuard)
            {
                playerBattleState = PlayerBattleState.Guard;
                playerAnim.SetTrigger("isGuard");
                playerAnim.SetBool("idleGuard", true);

                // �и� Ÿ�̹� Ȯ�� �κ�
                if (farryTime > 0)
                {
                    // ���� ���� �żҵ� ȣ��
                }
            }

            // �и�Ÿ�ֿ̹� ���ݰ���
            if (chkEnemyAttack)
            {
                isFarrying = true;

                // �и� ���
                farryCount++;
                if (farryCount > 2)
                    farryCount = 1;
                // �÷��̾� ����
                playerBattleState = PlayerBattleState.Farrying;
                // �÷��̾� �ִϸ��̼�
                playerAnim.SetTrigger("isFarry" + farryCount);

                farryTime = resetFarryTime;

                chkEnemyAttack = false;

                inputGuard = false;
                checkGuard = false;
            }

            // �и� Ÿ�̹� �� �ƴϰų� ���ݰ��� �ƴҶ�
            else if(farryTime <= 0 && checkGuard)
            {
                checkGuard = false;

                isGuard = true;

                playerBattleState = PlayerBattleState.Guard;
                playerAnim.SetTrigger("isGuard");
                playerAnim.SetBool("idleGuard", true);
            }
>>>>>>> parent of 81b66e7 (패링 판정 변경 및 적 애니메이션 수정)
        }
    }


    // Attack1, Attack2 �ִϸ��̼� ���� Add Event �� ȣ��
    public void AttackEnemy()
    {
        enemyObj = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f, enemyLayer);

        foreach (Collider2D col in enemyObj)
        {
<<<<<<< HEAD
            // ����ȿ� �� �ǰ� ������
            Debug.Log("EnemyHit");
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
<<<<<<< HEAD
        }
    }
=======
            if (col.gameObject.tag == "Enemy")
            {
                // ����ȿ� �� �ǰ� ������
                Debug.Log("EnemyHit");
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * testForce,ForceMode2D.Impulse);
            }
        }
    }

    // Farry1, Farry2 �ִϸ��̼� ���� Add Event �� ȣ��
    public void FarryAttack()
    {
        enemyCollider = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyCollider)
        {
            if (col.gameObject.tag == "Enemy")
            {
                // ����ȿ� �и� ��� ������
                Debug.Log("FarryAttack");
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.right * testForce, ForceMode2D.Impulse);
            }
        }
    } 

    // ��� ����
    public void GuardAttack()
    {
        enemyCollider = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyCollider)
        {
            if (col.gameObject.tag == "Enemy")
            {
                // ����ȿ� ���� ��� ������
                Debug.Log("GuardAttack");

                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * testForce, ForceMode2D.Impulse);
            }
        }
    }

    // �ִϸ��̼� Add Event �� �־���
    public void SetStateIdle()
    {
        playerBattleState = PlayerBattleState.Idle;
        isAttack = false;
        isFarrying = false;
        isGuard = false;
    }

>>>>>>> parent of 81b66e7 (패링 판정 변경 및 적 애니메이션 수정)

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
        // �÷��̾� �и� �ִϸ��̼� ���
        playerAnim.SetTrigger("isFarry" + Random.Range(1, 3));
        playerAnim.SetBool("idleGuard", false);

        int randomIdx = Random.Range(0, 3);
        audioSource.clip = farrySound[randomIdx];

        audioSource.Play();
    }
    // ���� �ִϸ��̼� Ȱ��ȭ
    private void SetAnimationGuard()
    {
        playerAnim.SetTrigger("isGuard");
        playerAnim.SetBool("idleGuard", true);
    }

    // �÷��̾� ������ ����
    public void TakeDamage(float damage)
    {
        if (playerBattleState == PlayerBattleState.Die)
            return;

        SetStateIdle();

        if (playerHp > 0)
        {
            playerHp -= damage;
            // ���߿� �ǰ� �ִϸ��̼� �ֱ�
        }
        else
        {
            playerBattleState = PlayerBattleState.Die;
            // ���߿� ��� �ִϸ��̼� �ֱ�
        }
    }
=======
>>>>>>> parent of 2a81b13 (패링 사운드 추가)
=======
        }
    }

>>>>>>> parent of 2a81b13 (패링 사운드 추가)
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
        //���ݸ��
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

    // ���� �ִϸ��̼� Ȱ��ȭ
    private void SetAnimationGuard()
    {
        playerAnim.SetTrigger("isGuard");
        playerAnim.SetBool("idleGuard", true);
    }

    // �÷��̾� ������ ����
    public void TakeDamage(float damage)
    {
        if (playerBattleState == PlayerBattleState.Die)
            return;

        if (playerHp > 0)
        {
            playerHp -= damage;
            // ���߿� �ǰ� �ִϸ��̼� �ֱ�
        }
        else
        {
            playerBattleState = PlayerBattleState.Die;
            // ���߿� ��� �ִϸ��̼� �ֱ�
        }
    }

    // �ִϸ��̼� Add Event �� �־���
    public void SetStateIdle()
    {
        playerBattleState = PlayerBattleState.Idle;
        isAttack = false;
        inputGuard = false;
=======
>>>>>>> parent of 81b66e7 (패링 판정 변경 및 적 애니메이션 수정)
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(battlePoint.position, battleBoxSize);
    }
}
