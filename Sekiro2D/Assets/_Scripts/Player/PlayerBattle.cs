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
    // �ִϸ��̼�
    public Animator playerAnim;

    // ����
    private AudioSource audioSource;
    public AudioClip[] farrySound;


    //�÷��̾� ����
    public static PlayerBattleState playerBattleState;
    PlayerBattleState lastPBS;
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
    private bool inputGuard;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;


    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        isGround = gameObject.GetComponent<PlayerMovement>().grounded;

        KeyInput();

        BattleAnimUpdate(playerBattleState);

        //���� ���� �ð��� ���� �ð� ���� ������
        ResetAttackComboTimeCount(resetComboTime);

    }

    public void KeyInput()
    {
        if (playerBattleState != PlayerBattleState.Hit && playerBattleState != PlayerBattleState.Die)
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
                    SetStateIdle();
                    playerAnim.SetBool("idleGuard", false);
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
            playerBattleState = PlayerBattleState.Attack;
        }
    }
    private void ResetAttackComboTimeCount(float resetTime)
    {
        if (attackTimeCount < resetTime)
            attackTimeCount += Time.deltaTime;
        else
            attackCombo = 0;
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
        playerBattleState = PlayerBattleState.Farrying;

        yield return new WaitForSeconds(resetFarryTime);

        if (inputGuard)
        {
            playerBattleState = PlayerBattleState.Guard;
        }
    }
    public void Farryed()
    {
        // �÷��̾� �и� �ִϸ��̼� ���
        farryCount = ActionCombo(farryCount,2);
        playerAnim.SetTrigger("isFarry" + farryCount);

        // ����� ���
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
        
        playerHp -= damage;

        if (playerHp > 0)
        {
            playerBattleState = PlayerBattleState.Hit;
        }
        else
        {
            playerBattleState = PlayerBattleState.Die;
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


    void BattleAnimUpdate(PlayerBattleState pbs)
    {
        if (lastPBS == pbs)
            return;

        // �÷��̾� �ִϸ��̼� ���
        switch (pbs)
        {
            case PlayerBattleState.Idle:
                break;
            case PlayerBattleState.Attack:
                playerAnim.SetTrigger("isAttack" + attackCombo);
                break;
            case PlayerBattleState.Guard:
                break;
            case PlayerBattleState.Farrying:
                break;
            case PlayerBattleState.Hit:
                playerAnim.SetTrigger("isHurt");
                break;
            case PlayerBattleState.Die:
                playerAnim.SetTrigger("isDie");
                break;
            default:
                break;
        }

        lastPBS = playerBattleState;
    }

    // �ִϸ��̼� Add Event �� �־���
    public void SetStateIdle()
    {
        playerBattleState = PlayerBattleState.Idle;
        isAttack = false;
        inputGuard = false;
    }

    void StartSetting()
    {
        playerBattleState = PlayerBattleState.Idle;
        audioSource = GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(battlePoint.position, battleBoxSize);
    }
}
