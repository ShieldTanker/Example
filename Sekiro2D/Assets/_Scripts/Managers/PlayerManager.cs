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
    private static PlayerManager pManager;
    private static PlayerState plState;
    private static PlayerBattleState plBattleState;

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

    // ÇÃ·¹ÀÌ¾î »óÅÂ ÀúÀå
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

    // PlayerManager ½Ì±ÛÅæ
    public static PlayerManager PManager
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
    // PlayerManager ½Ì±ÛÅæÈ­
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
        
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
    }

    public void PlayerHpBarChange(float playerHp, float maxHp)
    {
        lifeBar.value = playerHp / maxHp;
    }
}
