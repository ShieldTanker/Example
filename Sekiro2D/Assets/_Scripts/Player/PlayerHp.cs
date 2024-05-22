using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHp : MonoBehaviour
{
    // ����
    private AudioSource audioSource;
    public AudioClip[] farrySound;
    public AudioClip guardSound;
    public AudioClip hurtSound;


    //�÷��̾� ����
    public PlayerBattleState pState;
    private Rigidbody2D rb;

    // ü��
    public float playerHp;


    // ���,�и�
    private bool inputGuard;



    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        pState = PlayerManager.PManager.PlBattleState;
    }

    public void Farryed()
    {
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

    // �÷��̾� ������ ����
    public void TakeDamage(float damage)
    {
        if (pState == PlayerBattleState.Die)
            return;

        playerHp -= damage;

        if (playerHp > 0)
        {
            PlayerManager.PManager.PlBattleState = PlayerBattleState.Hit;
            audioSource.clip = hurtSound;
            audioSource.Play();
        }
        else
        {
            PlayerManager.PManager.PlBattleState = PlayerBattleState.Die;
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
    }


    void StartSetting()
    {
        PlayerManager.PManager.PlState = PlayerState.Idle;
        audioSource = GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
}
