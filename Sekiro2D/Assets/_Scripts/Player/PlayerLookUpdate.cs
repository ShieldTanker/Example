using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerLookUpdate : MonoBehaviour
{
    public Transform cusorPos;
    public Animator playerAnim;

    // �÷��̾� ����
    public PlayerState plState;

    // ������
    private float inputX;
    private bool lastGrd;

    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        if (GameManager.GManager.PlState != PlayerState.Die)
        {
            plState = GameManager.GManager.PlState;
            inputX = PlayerMovement1.inputX;

            GroundAnimCheck(PlayerMovement1.Ground);


            PlayerMoveAnimUpdate(plState);

            if (plState != PlayerState.Attack && plState != PlayerState.Hit)
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

    private void PlayerMoveAnimUpdate(PlayerState plMove)
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
            default:
                break;
        }
    }
}
