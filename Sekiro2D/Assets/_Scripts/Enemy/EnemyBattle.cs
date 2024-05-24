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
    // ������
    public Transform[] wayPoints;
    int pointIdx;
    public float disForPlayer;

    public Transform enemyRayPos;
    public Transform playerRayPos;


    // �÷��̾�
    public GameObject player;
    PlayerBattleState pBState;

    // ���� ������Ʈ
    public float moveSpeed;

    float delayAttack;
    public float initAttackDelay;

    public EnemyBattleState enemyBattleState;
    EnemyBattleState lastEBS;

    // �ִϸ��̼� ����
    public Animator enemyAnim;

    //����� ����
    private AudioSource audioSource;
    public AudioClip hurtSound;

    // ���� ���� ����
    public Collider2D[] atkColl;
    public Transform enemyAtkPoint;
    public GameObject enemy;
    public LayerMask playerLayer;
    public Vector2 atkBoxSize;
    
    // �ִ밨�� �Ÿ�
    public float maxDistance;
    // ����Ÿ�
    float currentDistance;
    // ��ǥ���� �Ÿ�
    float waypointDistance;
    // ���� �Ÿ�
    public float stopPos;

    public Vector2 checkDir;
    RaycastHit2D hit;

    // ���� ����
    public float atkDamage;
    public float enemyAtkTime;
    public float knockBackForce;
    public float playerKnocBackTime;

    // ���� ���� ����
    public static bool isFarryed;
    public float FarryDelay;

    //ü�� ����
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

        // �ݶ��̴��� �÷��̾� �±׸� ������ ������
        if (playerColl != null)
        {
            PlayerBattle pB = playerColl.GetComponent<PlayerBattle>();
            PlayerBattleState pS = PlayerManager.PManager.PlBattleState;

            if (pS == PlayerBattleState.Farrying)
            {   // �÷��̾ �и������϶�
                pB.Farryed();
                enemyBattleState = EnemyBattleState.Farryed;

                isFarryed = true;
                Invoke("FalseFarryed", FarryDelay);

            }
            else if (pS == PlayerBattleState.Guard)
            {   // �÷��̾ ��������϶�
                StartCoroutine(pB.KnockBack(enemy.transform, knockBackForce / 2, playerKnocBackTime));
                pB.Guarded();
            }
            else
            {   // ����,�и� ���°� �ƴҶ�
                StartCoroutine(pB.KnockBack(enemy.transform, knockBackForce, playerKnocBackTime));
                pB.TakeDamage(atkDamage);
            }
        }
    }
    void FalseFarryed()
    {
        isFarryed = false;
    }

    // ���ݽ� ���� ��ü Ȯ��
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

    // �� �ǰ�
    public void enemyHurt(float damage)
    {
        enemyHp -= damage;
        hpBar.value = enemyHp / enemyMaxHp;

        audioSource.clip = hurtSound;
        audioSource.Play();

        if (enemyHp > 0)
        {   // �ǰ�
            enemyBattleState = EnemyBattleState.Hurt;
        }
        else
        {   // ���
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

    // Ÿ�̸�
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

    // üũ
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

    // �̵�
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

    // ���� ���
    void CalcVec(Transform vecWay)
    {
        float vecX = vecWay.position.x - transform.position.x;

        if (vecX > 1)
            transform.localScale = new Vector3(1, 1, 1);
        else if (vecX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
