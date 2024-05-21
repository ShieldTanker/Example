using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{

    // �÷��̾� ����
    PlayerState plState;
    PlayerState lastPlState;
    PlayerBattleState plBattleState;
    bool isWallJump;
    public bool isWallSlide;

    // ������
    Rigidbody2D rb;
    public float jumpForce;
    public float wallJumpForce;
    public float wallJumpTime;

    public float moveSpeed;
    private float moveX;
    public static float inputX;

    public float fallingSpeed;

    // ���� üũ
    public LayerMask grdCheckLayerMask;
    public Transform grdCheckPoint;
    public float grdCheckSize;
    private static bool ground;

    // ������ ����
    public Transform senseTopRight;
    public static bool rTopWall;
    public Transform senseLowRight;
    public static bool rLowWall;

    // ���� ����
    public Transform senseTopLeft;
    public static bool lTopWall;
    public Transform senseLowLeft;
    public static bool lLowWall;

    public LayerMask wallLayer;
    public float wallSensorSize;


    // ������ �Ӽ�
    public static bool RightTopSensor { get { return rTopWall; } }
    public static bool RightLowSensor { get { return rLowWall; } }
    public static bool LeftTopSensor { get { return lTopWall; } }
    public static bool LeftLowSensor { get { return lLowWall; } }


    public static bool Ground { get { return ground;} }

    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        if (plBattleState != PlayerBattleState.Die)
        {
            UpdateParameter();

            KeyInput();

            CheckUpdate();

            if (lastPlState != plState)
                lastPlState = plState;
        }
    }

    public void KeyInput()
    {
        if (plBattleState == PlayerBattleState.Attack || plBattleState == PlayerBattleState.Farrying)
            rb.velocity = Vector2.zero;
        else
        {
            MovePosX();

            // �Է��� ������
            if (Input.GetButton("Horizontal"))
            {
                if (!isWallJump)
                {
                    rb.velocity = new Vector2(moveX, rb.velocity.y);
                    PlayerManager.PManager.PlState = PlayerState.Move;
                }
            }
            else if(ground)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                PlayerManager.PManager.PlState = PlayerState.Idle;
            }
            // ������
            InputWallJump();
            WallSlideSpeed();

            InputJump();
        }
    }

    // ��������
    void InputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ground)
        {
            PlayerManager.PManager.PlState = PlayerState.Jump;
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    void InputWallJump()
    {
        if (!ground)
        {
            // ������ ���϶�
            if (plState == PlayerState.WallSlideRight)
                WallJump(Vector2.left);

            // ���� ���϶�
            else if (plState == PlayerState.WallSlideLeft)
                WallJump(Vector2.right);
        }
    }
    void WallJump(Vector2 wayVec)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isWallJump = true;
            Invoke("FalseWallJump", wallJumpTime);

            isWallSlide = false;
            PlayerManager.PManager.PlState = PlayerState.Jump;

            Vector2 wallJump = (Vector2.up + wayVec) * jumpForce;
            wallJump.Normalize();

            rb.velocity = wallJump * wallJumpForce;
        }
    }
    
    // ������ �������� ����� �޼ҵ�
    void FalseWallJump()
    {
        isWallJump = false;
    }
    
    // ���� ������� �ӵ� ����
    private void WallSlideSpeed()
    {
        if (isWallSlide)
        {
            float slowY = rb.velocity.y * fallingSpeed;
            rb.velocity = new Vector2(rb.velocity.x, slowY);
        }
    }

    // ������
    void MovePosX()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        moveX = inputX * moveSpeed;
    }

    void CheckUpdate()
    {
        GroundCheck();
        WallCheck();
        LimitCheck();
        CheckWallSlide();
        CheckFallingState();

    }

    // ���� üũ �޼ҵ��
    void LimitCheck()
    {
        if ((inputX > 0 && (rTopWall || rLowWall)) ||
            (inputX < 0 && (lTopWall || lLowWall)))
        {
            moveX = 0f;
        }
    }
    public void GroundCheck()
    {
        ground = Physics2D.OverlapCircle(
            grdCheckPoint.position, grdCheckSize, grdCheckLayerMask);
    }
    bool CheckPosition(bool wallCheck, Transform senssPos)
    {
        wallCheck = Physics2D.OverlapCircle(senssPos.position, wallSensorSize, wallLayer);
        return wallCheck;
    }
    public void WallCheck()
    {
        rTopWall= CheckPosition(rTopWall,senseTopRight);
        rLowWall = CheckPosition(rLowWall, senseLowRight);
        lTopWall = CheckPosition(lTopWall, senseTopLeft);
        lLowWall = CheckPosition(lLowWall, senseLowLeft);
    }
    void CheckFallingState()
    {
        if (!ground && plState != PlayerState.Jump &&
            (plState != PlayerState.WallSlideLeft || plState != PlayerState.WallSlideRight))
        {
            isWallSlide = false;
            PlayerManager.PManager.PlState = PlayerState.Falling;
        }
    }
    void CheckWallSlide()
    {
        if (plState == PlayerState.WallSlideRight || plState == PlayerState.WallSlideLeft)
            isWallSlide = true;
        else
            isWallSlide = false;
    }
    
    // ���� �޼ҵ�
    void StartSetting()
    {
        rb = GetComponent<Rigidbody2D>();
        moveX = transform.position.x;
    }
    void UpdateParameter()
    {
        plState = PlayerManager.PManager.PlState;
        plBattleState = PlayerManager.PManager.PlBattleState;
    }
    // ���� Ȯ�ο�
    private void OnDrawGizmos()
    {
        DrawGizmo(senseTopRight, wallSensorSize);
        DrawGizmo(senseLowRight, wallSensorSize);
        DrawGizmo(senseTopLeft, wallSensorSize);
        DrawGizmo(senseLowLeft, wallSensorSize);

        DrawGizmo(grdCheckPoint, grdCheckSize);
    }
    void DrawGizmo(Transform point, float wallSensorSize)
    {
        Gizmos.DrawWireSphere(point.position, wallSensorSize);
    }
}
