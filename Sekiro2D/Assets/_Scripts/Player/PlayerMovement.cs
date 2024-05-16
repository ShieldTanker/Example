using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{

    // 플레이어 상태
    public PlayerState plState;
    public PlayerBattleState plBattleState;


    // 움직임
    Rigidbody2D rb;
    public float jumpForce;
    public float moveSpeed;
    private float moveX;
    public static float inputX;

    // 지형 체크
    public LayerMask grdCheckLayerMask;
    public Transform grdCheckPoint;
    private static bool ground;

    public Transform wallSensorRight;
    public Transform wallSensorLeft;
    // public Transform wallSensorRightTop;
    // public Transform wallSensorLeftTop;
    public static bool isRightWall;
    public static bool isLeftWall;
    public LayerMask wallLayer;

    public static bool RightSensor
    {
        get
        { return isRightWall; }
    }

    public static bool LeftSensor
    {
        get
        { return isLeftWall; }
    }

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
        if (PlayerManager.PManager.PlBattleState != PlayerBattleState.Die)
        {
            plState = PlayerManager.PManager.PlState;
            plBattleState = PlayerManager.PManager.PlBattleState;

            GroundCheck();
            WallCheck();

            KeyInput();
        }
    }
    public void KeyInput()
    {
        if (plBattleState != PlayerBattleState.Attack &&
            plBattleState != PlayerBattleState.Farrying)
        {
            if (Input.GetButton("Horizontal"))
            {
                PlayerManager.PManager.PlState = PlayerState.Move;

                inputX = Input.GetAxisRaw("Horizontal");
                moveX = transform.position.x + inputX * moveSpeed * Time.deltaTime;

                transform.position = new Vector2(moveX, transform.position.y);
            }
            else
                PlayerManager.PManager.PlState = PlayerState.Idle;

            if (Input.GetKeyDown(KeyCode.Space) && ground)
            {
                PlayerManager.PManager.PlState = PlayerState.Jump;

                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }


    public void GroundCheck()
    {
        ground = Physics2D.OverlapCircle(
            grdCheckPoint.position, 0.1f, grdCheckLayerMask);
    }

    public void WallCheck()
    {
        isRightWall = Physics2D.OverlapCircle(wallSensorRight.position, 0.1f, wallLayer);
        isLeftWall = Physics2D.OverlapCircle(wallSensorLeft.position, 0.1f, wallLayer);
        
        Debug.Log("right" + isRightWall);
    }

    void StartSetting()
    {
        rb = GetComponent<Rigidbody2D>();

        moveX = transform.position.x;
    }
}
