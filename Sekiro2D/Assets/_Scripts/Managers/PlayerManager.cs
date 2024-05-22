using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // «√∑π¿ÃæÓ ªÛ≈¬ ¿˙¿Â
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

    // PlayerManager ΩÃ±€≈Ê
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
    // PlayerManager ΩÃ±€≈Ê»≠
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

    private void Update()
    {
        if (PlBattleState == PlayerBattleState.Die)
        {
            Debug.Log("Die");
        }
        else
        {
            Debug.Log(PlBattleState);
        }
    }
}
