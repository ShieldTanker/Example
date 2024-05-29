using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Idle,
    Move,
    Jump,
    Falling,
    WallSlideRight,
    WallSlideLeft
}
public enum PlayerBattleState
{
    Idle,
    Attack,
    Guard,
    Farrying,
    Hit,
    Die
}
public class PlayerManager : MonoBehaviour
{
    private PlayerManager pManager;
    private PlayerState plState;
    private PlayerBattleState plBattleState;

    public StageManager sM;

    private float hp;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            if (value <= 0)
            {
                hp = 0f;
            }
            hp = value;
        }
    }

    public Slider lifeBar;

    // �÷��̾� ���� ����
    public PlayerState PlState
    {
        get
        {
            return plState;
        }
        set
        {
            plState = value;
        }
    }
    public PlayerBattleState PlBattleState
    {
        get
        {
            return plBattleState;
        }
        set
        {
            plBattleState = value;
        }
    }


    // PlayerManager �̱���
    public PlayerManager PManager
    {
        get
        {
            if (!pManager)
            {
                pManager = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
                
                if (pManager == null)
                    Debug.Log("no Singleton obj");
            }

            return pManager;
        }
        set
        {
            pManager = value;
        }
    }
    // PlayerManager �̱���ȭ
    private void Awake()
    {
        if (pManager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            pManager = this;
        }
    }



    private void Start()
    {
        lifeBar = GameObject.Find("PlayerLifeBar").GetComponent<Slider>();
    }

    private void Update()
    {
        CheckPlayerDie();
    }

    public void CheckPlayerDie()
    {
        if (plBattleState != PlayerBattleState.Die)
        {
            return;
        }
        sM.ShowGameOver();
    }

    // ü�� ��ȭ ������
    public void PlayerHpBarChange(float playerHp, float maxHp)
    {
        lifeBar.value = playerHp / maxHp;
    }
}
