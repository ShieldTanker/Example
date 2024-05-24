using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyBattleState
{
    Idle,
    Attack,
    Guard,
    Farryed,
    Hurt,
    Die
}

public class EnemyBattle : MonoBehaviour
{
    // 목적지
    public Transform[] wayPoints;
    int pointIdx;
    public float disForPlayer;

    public Transform enemyRayPos;
    public Transform playerRayPos;


    // 플레이어
    public GameObject player;
    PlayerBattleState pBState;

    // 본인 오브젝트
    public float moveSpeed;

    float delayAttack;
    public float initAttackDelay;

    public EnemyBattleState enemyBattleState;
    EnemyBattleState lastEBS;

    // 애니메이션 관련
    public Animator enemyAnim;

    //오디오 관련
    private AudioSource audioSource;
    public AudioClip hurtSound;

    // 전투 범위 관련
    public Collider2D[] atkColl;
    public Transform enemyAtkPoint;
    public GameObject enemy;
    public LayerMask playerLayer;
    public Vector2 atkBoxSize;
    
    // 최대감지 거리
    public float maxDistance;
    // 현재거리
    float currentDistance;
    // 목표와의 거리
    float waypointDistance;
    // 정지 거리
    public float stopPos;

    public Vector2 checkDir;
    RaycastHit2D hit;

    // 공격 관련
    public float atkDamage;
    public float enemyAtkTime;
    public float knockBackForce;
    public float playerKnocBackTime;

    // 전투 상태 관련
    public static bool isFarryed;
    public float FarryDelay;

    //체력 관련
    public float enemyHp;
    public float enemyMaxHp;
    public Slider hpBar;
    public GameObject hpCanvas;

    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        if (lastEBS == EnemyBattleState.Die)
        {
            return;
        }
        TimeCheck();

        DistanceCheck();

        CheckPlayer();

        AttackDelay();

        EnemyBattleAnimUpdate(enemyBattleState);
    }

    public void AttackPlayer()
    {
        Collider2D playerColl = AtkCollider(enemyAtkPoint, atkBoxSize);

        // 콜라이더가 플레이어 태그를 가지고 있을때
        if (playerColl != null)
        {
            PlayerBattle pB = playerColl.GetComponent<PlayerBattle>();
            PlayerBattleState pS = PlayerManager.PManager.PlBattleState;

            if (pS == PlayerBattleState.Farrying)
            {   // 플레이어가 패링상태일때
                pB.Farryed();
                enemyBattleState = EnemyBattleState.Farryed;

                isFarryed = true;
                Invoke("FalseFarryed", FarryDelay);

            }
            else if (pS == PlayerBattleState.Guard)
            {   // 플레이어가 가드상태일때
                StartCoroutine(pB.KnockBack(enemy.transform, knockBackForce / 2, playerKnocBackTime));
                pB.Guarded();
            }
            else
            {   // 가드,패링 상태가 아닐때
                StartCoroutine(pB.KnockBack(enemy.transform, knockBackForce, playerKnocBackTime));
                pB.TakeDamage(atkDamage);
            }
        }
    }
    void FalseFarryed()
    {
        isFarryed = false;
    }

    // 공격시 닿은 물체 확인
    private Collider2D AtkCollider(Transform atkPoint, Vector2 boxSize)
    {
        Collider2D playerColl = new Collider2D();
        atkColl = Physics2D.OverlapBoxAll(atkPoint.position, boxSize, 0f, playerLayer);

        foreach (Collider2D col in atkColl)
        {
            if (col.gameObject.tag == "Player")
            {
                playerColl = col;
            }
        }

        return playerColl;
    }

    // 적 피격
    public void enemyHurt(float damage)
    {
        enemyHp -= damage;
        hpBar.value = enemyHp / enemyMaxHp;

        audioSource.clip = hurtSound;
        audioSource.Play();

        if (enemyHp > 0)
        {   // 피격
            enemyBattleState = EnemyBattleState.Hurt;
        }
        else
        {   // 사망
            enemyBattleState = EnemyBattleState.Die;
            hpBar.gameObject.SetActive(false);
        }
    }


    void EnemyBattleAnimUpdate(EnemyBattleState eBS)
    {
        if (lastEBS == eBS)
            return;

        switch (eBS)
        {
            case EnemyBattleState.Idle:
                break;

            case EnemyBattleState.Attack:
                enemyAnim.SetTrigger("EnemyAttack");
                break;

            case EnemyBattleState.Guard:
                break;

            case EnemyBattleState.Farryed:
                enemyAnim.SetTrigger("FarryedAttack");
                break;

            case EnemyBattleState.Hurt:
                enemyAnim.SetTrigger("isHurt");
                break;

            case EnemyBattleState.Die:
                enemyAnim.SetTrigger("isDie");
                enemyAnim.SetBool("enemyDied",true);
                break;

            default:
                break;
        }

        lastEBS = eBS;
    }

    void StartSetting()
    {
        audioSource = GetComponent<AudioSource>();
        enemyRayPos = GameObject.Find("ERayPos").transform;
        playerRayPos = GameObject.Find("PRayPos").transform;
        player = GameObject.FindWithTag("Player");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyAtkPoint.position, atkBoxSize);
    }

    // 타이머
    void TimeCheck()
    {
        if (delayAttack > 0)
            delayAttack -= Time.deltaTime;
    }
    void AttackDelay()
    {
        if (pBState != PlayerBattleState.Die)
        {
            if (currentDistance <= 1.8 && enemyAnim.GetBool("PlayerCheck") &&
                delayAttack <= 0 && !EnemyBattle.isFarryed && enemyBattleState != EnemyBattleState.Die)
            {
                enemyAnim.SetTrigger("EnemyAttack");
                delayAttack = initAttackDelay;
            }
        }
    }

    // 체크
    void CheckPlayer()
    {
        if (pBState != PlayerBattleState.Die)
        {
            checkDir = playerRayPos.position - enemyRayPos.position;

            Vector3 checkRay = maxDistance * checkDir.normalized;
            Debug.DrawRay(enemyRayPos.position, checkRay, Color.red);

            hit = Physics2D.Raycast(enemyRayPos.position, checkDir, maxDistance, playerLayer);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                    enemyAnim.SetBool("PlayerCheck", true);
                else
                    enemyAnim.SetBool("PlayerCheck", false);
            }
            else
                enemyAnim.SetBool("PlayerCheck", false);
        }
        else
        {
            enemyAnim.SetBool("playerDie", true);
        }
    }
    void DistanceCheck()
    {
        currentDistance = Vector2.Distance(player.transform.position, transform.position);
        enemyAnim.SetFloat("playerChkDis", currentDistance);

        waypointDistance = Vector2.Distance(transform.position, wayPoints[pointIdx].position);
        enemyAnim.SetFloat("distanceForPoint", waypointDistance);
    }

    // 이동
    public void GoToWayPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[pointIdx].position, moveSpeed * Time.deltaTime);
        CalcVec(wayPoints[pointIdx]);
    }
    public void GoToTarget()
    {
        if (pBState != PlayerBattleState.Die)
        {
            CalcVec(player.transform);

            Vector3 dir = player.transform.position;

            if (currentDistance >= stopPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, dir, moveSpeed * Time.deltaTime);
            }
        }
    }
    public void WayPointSet()
    {
        switch (pointIdx)
        {
            case 0:
                pointIdx = 1;
                break;
            case 1:
                pointIdx = 0;
                break;
            default:
                break;
        }
    }

    // 방향 계산
    void CalcVec(Transform vecWay)
    {
        float vecX = vecWay.position.x - transform.position.x;

        if (vecX > 1)
            transform.localScale = new Vector3(1, 1, 1);
        else if (vecX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
