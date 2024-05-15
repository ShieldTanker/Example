using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    public Transform cusorPos;

    public Animator playerAnim;

    // �÷��̾� ����
    public PlayerState plState;

    // ������
    Rigidbody2D rb;
    public float jumpForce;
    public float moveSpeed;
    private float moveX;
    private float inputX;

    // ���� üũ
    public LayerMask grdCheckLayerMask;
    public Transform grdCheckPoint;
    private static bool ground;

    public static bool Ground
    {
        get
        {
            return ground;
        }
    }

    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        if (PlayerManager.PManager.PlState != PlayerState.Die)
        {
            plState = PlayerManager.PManager.PlState;

            GroundCheck();

            KeyInput();

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

    public void GroundCheck()
    {
        ground = Physics2D.OverlapCircle(
            grdCheckPoint.position, 0.1f, grdCheckLayerMask);

        playerAnim.SetBool("isGround", ground);
    }

    void StartSetting()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();

        moveX = transform.position.x;
    }
    public void KeyInput()
    {
        if (plState != PlayerState.Attack &&
            plState != PlayerState.Farrying)
        {
            if (Input.GetButton("Horizontal"))
            {
                //plMove = PlayerMoveState.Move;
                plState = PlayerState.Move;

                inputX = Input.GetAxisRaw("Horizontal");
                moveX = transform.position.x + inputX * moveSpeed * Time.deltaTime;

                transform.position = new Vector2(moveX, transform.position.y);
            }
            else
                //plMove = PlayerMoveState.Idle;
                plState = PlayerState.Idle;

            if (Input.GetKeyDown(KeyCode.Space) && ground)
            {
                //plMove = PlayerMoveState.Jump;
                plState = PlayerState.Jump;
                PlayerManager.PManager.PlState = PlayerState.Jump;

                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            }
        }
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
