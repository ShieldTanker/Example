using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    public Transform cusorPos;

    public Animator playerAnim;

    // 플레이어 상태
    public PlayerState plState;

    // 움직임
    Rigidbody2D rb;
    public float jumpForce;
    public float moveSpeed;
    private float moveX;
    private float inputX;

    // 지형 체크
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

    // 마우스 위치 바라보기
    void LookCusorRotation()
    {
        float cusorX = cusorPos.position.x - transform.position.x;

        // 마우스 오른쪽
        if (cusorX > 0)
        {
            LookAHead(new Vector3(1, 1, 1));

            // 오른쪽으로 이동
            if (inputX > 0)
                WalkFowardAnim();
            //왼쪽으로 이동
            else if (inputX < 0)
                WalkBackAnim();
        }
        // 마우스가 왼쪽
        else if (cusorX < 0)
        {
            LookAHead(new Vector3(-1, 1, 1));

            // 오른쪽으로 이동
            if (inputX > 0)
                WalkBackAnim();

            //왼쪽으로 이동
            else if (inputX < 0)
                WalkFowardAnim();
        }
    }
    
    // 바라보는 방향
    void LookAHead(Vector3 lookAHead)
    {
        transform.localScale = lookAHead;
    }

    //앞으로 걷기 뒤로걷기
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
