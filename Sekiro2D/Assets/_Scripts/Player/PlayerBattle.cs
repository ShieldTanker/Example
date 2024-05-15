using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBattle : MonoBehaviour
{
    // 애니메이션
    public Animator playerAnim;

    // 사운드
    private AudioSource audioSource;
    public AudioClip[] farrySound;
    public AudioClip guardSound;


    //플레이어 상태
    public PlayerState pState;
    PlayerState lastPBS;
    private Rigidbody2D rb;
    private bool isGround;

    // 체력
    public float playerHp;
    public bool playerHit;

    // 공격 관련
    public float atkDamage;
    public bool isAttack;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    public int attackCombo = 0;

    // 공격 범위 관련
    public Transform battlePoint;
    public Vector2 battleBoxSize;
    public LayerMask enemyLayer;
    public Collider2D[] enemyObj;

    // 방어,패링
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

        //공격 가능 시간이 리셋 시간 보다 작을때
        ResetAttackComboTimeCount(resetComboTime);

    }

    public void KeyInput()
    {
        if (pState != PlayerState.Hit &&
            pState != PlayerState.Die)
        {
            // 공격
            if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
            {
                Attack();
            }
            else if (!isAttack)
            {
                //방어
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

        // 공격가능 시간이 공격 딜레이 시간보다 많을때
        if (attackTimeCount > delayAttackTime)
        {
            attackTimeCount = 0;

            //공격모션
            attackCombo = ActionCombo(attackCombo,2);

            // 플레이어 상태
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

    // Attack1, Attack2 애니메이션 에서 Add Event 로 호출
    private int ActionCombo(int currentCombo, int maxCombo)
    {
        //공격모션
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
        // 플레이어 패링 애니메이션 재생
        farryCount = ActionCombo(farryCount,2);
        playerAnim.SetTrigger("isFarry" + farryCount);

        // 오디오 재생
        int randomIdx = Random.Range(0, 2);
        audioSource.clip = farrySound[randomIdx];

        audioSource.Play();
    }
    public void Guarded()
    {
        audioSource.clip = guardSound;
        audioSource.Play();
    }

    // 가드 애니메이션 활성화
    private void SetAnimationGuard()
    {
        playerAnim.SetTrigger("isGuard");
        playerAnim.SetBool("idleGuard", true);
    }


    // 플레이어 데미지 입음
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

        // 플레이어 애니메이션 재생
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

    // 애니메이션 Add Event 에 넣어짐
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
