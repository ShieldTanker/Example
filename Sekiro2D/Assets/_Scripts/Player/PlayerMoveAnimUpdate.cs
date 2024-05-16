using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMoveAnimUpdate : MonoBehaviour
{
    public Transform cusorPos;
    public Animator playerAnim;

    public PlayerBattle pB;

    // �÷��̾� ����
    public PlayerState plState;
    public PlayerBattleState plBattleState;

    // ������
    private float inputX;
    private bool lastGrd;

    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        if (PlayerManager.PManager.PlBattleState != PlayerBattleState.Die)
        {
            plState = PlayerManager.PManager.PlState;
            plBattleState = PlayerManager.PManager.PlBattleState;
            inputX = PlayerMovement.inputX;

            GroundAnimCheck(PlayerMovement.Ground);

            WallAnimCheck(PlayerMovement.RightSensor, PlayerMovement.LeftSensor);

            PlayerAnimUpdate(plState);

            if (plBattleState != PlayerBattleState.Attack &&
                plBattleState != PlayerBattleState.Hit)
                LookCusorRotation();
        }
    }

    // ���콺 ��ġ �ٶ󺸱�
    void LookCusorRotation()
    {
        float cusorX = cusorPos.position.x - transform.position.x;

        // ���콺 ������
        if (cusorX > 0)
        {
            LookAHead(new Vector3(1, 1, 1));

            // ���������� �̵�
            if (inputX > 0)
                WalkFowardAnim();
            //�������� �̵�
            else if (inputX < 0)
                WalkBackAnim();
        }
        // ���콺�� ����
        else if (cusorX < 0)
        {
            LookAHead(new Vector3(-1, 1, 1));

            // ���������� �̵�
            if (inputX > 0)
                WalkBackAnim();

            //�������� �̵�
            else if (inputX < 0)
                WalkFowardAnim();
        }
    }
    
    // �ٶ󺸴� ����
    void LookAHead(Vector3 lookAHead)
    {
        transform.localScale = lookAHead;
    }

    //������ �ȱ� �ڷΰȱ�
    void WalkFowardAnim()
    {
        playerAnim.SetFloat("runSpeed", 1);
    }
    void WalkBackAnim()
    {
        playerAnim.SetFloat("runSpeed", -1);
    }

    void StartSetting()
    {
        playerAnim = GetComponent<Animator>();
    }

    private void GroundAnimCheck(bool grdCheck)
    {
        if (lastGrd == grdCheck)
            return;

        playerAnim.SetBool("isGround", grdCheck);

        lastGrd = grdCheck;
    }

    private void WallAnimCheck(bool rightSensor, bool leftSensor)
    {
        if (rightSensor && !PlayerMovement.Ground)
        {
            PlayerManager.PManager.PlState = PlayerState.WallSlide;
        }
        if (leftSensor && !PlayerMovement.Ground)
        {
            PlayerManager.PManager.PlState = PlayerState.WallSlide;
        }
    }

    private void PlayerAnimUpdate(PlayerState plMove)
    {

        switch (plMove)
        {
            case PlayerState.Idle:
                playerAnim.SetBool("isMove", false);
                break;
            case PlayerState.Move:
                playerAnim.SetBool("isMove", true);
                break;
            case PlayerState.Jump:
                playerAnim.SetTrigger("isJump");
                break;

            case PlayerState.WallSlide:
                playerAnim.SetBool("isWall", true);
                break;
            default:
                break;
        }
    }
    
    public void SetStateIdle()
    {
        pB.SetStateIdle();
    }
}
