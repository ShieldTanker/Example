using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Move,
    Jump,
    Attack,
    Guard,
    Farrying,
    Hit,
    Die
}
public class GameManager : MonoBehaviour
{

    private static GameManager gManager;
    private static PlayerState plState;

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

    // GameManager ΩÃ±€≈Ê
    public static GameManager GManager
    {
        get
        {
            if (!gManager)
            {
                gManager = FindObjectOfType(typeof(GameManager)) as GameManager;
                
                if (gManager == null)
                    Debug.Log("no Singleton obj");
            }

            return gManager;
        }
        set
        {
            gManager = value;
        }
    }

    private void Awake()
    {
        if (gManager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            gManager = this;
        }
        
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
    }
}
