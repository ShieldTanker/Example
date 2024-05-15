using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBattle : MonoBehaviour
{
    // �ִϸ��̼�
    public Animator playerAnim;

    // ����
    private AudioSource audioSource;
    public AudioClip[] farrySound;
    public AudioClip guardSound;


    //�÷��̾� ����
    public PlayerState pState;
    PlayerState lastPBS;
    private Rigidbody2D rb;
    private bool isGround;

    // ü��
    public float playerHp;
    public bool playerHit;

    // ���� ����
    public float atkDamage;
    public bool isAttack;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    public int attackCombo = 0;

    // ���� ���� ����
    public Transform battlePoint;
    public Vector2 battleBoxSize;
    public LayerMask enemyLayer;
    public Collider2D[] enemyObj;

    // ���,�и�
    public bool inputGuard;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;


    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        isGround = PlayerMovement1.Ground;
       
        pState = PlayerManager.PManager.PlState;

        KeyInput();

        BattleAnimUpdate(pState);

        //���� ���� �ð��� ���� �ð� ���� ������
        ResetAttackComboTimeCount(resetComboTime);

    }

    public void KeyInput()
    {
        if (pState != PlayerState.Hit &&
            pState != PlayerState.Die)
        {
            // ����
            if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
            {
                Attack();
            }
            else if (!isAttack)
            {
                //���
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    inputGuard = true;
                    StartCoroutine(GuardOrFarry());
                }
                else if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    playerAnim.SetBool("idleGuard", false);
                    SetStateIdle();
                }
            }
        }
    }

    public void Attack()
    {
        isAttack = true;

        // ���ݰ��� �ð��� ���� ������ �ð����� ������
        if (attackTimeCount > delayAttackTime)
        {
            attackTimeCount = 0;

            //���ݸ��
            attackCombo = ActionCombo(attackCombo,2);

            // �÷��̾� ����
            PlayerManager.PManager.PlState = PlayerState.Attack;
        }
    }
    private void ResetAttackComboTimeCount(float resetTime)
    {
        if (attackTimeCount < resetTime)
            attackTimeCount += Time.deltaTime;
        else
        {
            attackCombo = 0;
        }
    }

    // Attack1, Attack2 �ִϸ��̼� ���� Add Event �� ȣ��
    private int ActionCombo(int currentCombo, int maxCombo)
    {
        //���ݸ��
        currentCombo++;

        if (currentCombo > maxCombo)
        {
            currentCombo = 1;
        }
        return currentCombo;
    }

    public void AttackEnemy()
    {
        enemyObj = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f, enemyLayer);

        foreach (Collider2D col in enemyObj)
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            enemy.enemyHurt(atkDamage);
        }
    }


    IEnumerator GuardOrFarry()
    {
        SetAnimationGuard();
        PlayerManager.PManager.PlState = PlayerState.Farrying;

        yield return new WaitForSeconds(resetFarryTime);

        if (inputGuard)
        {
            PlayerManager.PManager.PlState = PlayerState.Guard;
        }
    }
    public void Farryed()
    {
        // �÷��̾� �и� �ִϸ��̼� ���
        farryCount = ActionCombo(farryCount,2);
        playerAnim.SetTrigger("isFarry" + farryCount);

        // ����� ���
        int randomIdx = Random.Range(0, 2);
        audioSource.clip = farrySound[randomIdx];

        audioSource.Play();
    }
    public void Guarded()
    {
        audioSource.clip = guardSound;
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
        if (pState == PlayerState.Die)
            return;

        SetStateIdle();
        
        playerHp -= damage;

        if (playerHp > 0)
        {
            // pState = PlayerState.Hit;
            PlayerManager.PManager.PlState = PlayerState.Hit;
        }
        else
        {
            // pState = PlayerState.Die;
            PlayerManager.PManager.PlState = PlayerState.Die;
        }
    }
    public IEnumerator KnockBack(Transform enemy, float knockBackForce, float knockBackTime)
    {
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


    void BattleAnimUpdate(PlayerState pbs)
    {
        if (lastPBS == pbs)
            return;

        // �÷��̾� �ִϸ��̼� ���
        switch (pbs)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Attack:
                playerAnim.SetTrigger("isAttack" + attackCombo);
                break;
            case PlayerState.Guard:
                break;
            case PlayerState.Farrying:
                break;
            case PlayerState.Hit:
                playerAnim.SetTrigger("isHurt");
                break;
            case PlayerState.Die:
                playerAnim.SetTrigger("isDie");
                break;
            default:
                break;
        }

        lastPBS = pbs;
    }

    // �ִϸ��̼� Add Event �� �־���
    public void SetStateIdle()
    {
        PlayerManager.PManager.PlState = PlayerState.Idle;

        Debug.Log("SetStateIdle");

        isAttack = false;
        Debug.Log("isAttack False");
        inputGuard = false;
    }

    void StartSetting()
    {
        PlayerManager.PManager.PlState = PlayerState.Idle;
        audioSource = GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(battlePoint.position, battleBoxSize);
    }
}
