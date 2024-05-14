using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement1 : MonoBehaviour
{

    // 플레이어 상태
    public PlayerState plState;

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
        if (GameManager.GManager.PlState != PlayerState.Die)
        {
            plState = GameManager.GManager.PlState;

            GroundCheck();

            KeyInput();
        }
    }


    public void GroundCheck()
    {
        ground = Physics2D.OverlapCircle(
            grdCheckPoint.position, 0.1f, grdCheckLayerMask);
    }

    void StartSetting()
    {
        rb = GetComponent<Rigidbody2D>();

        moveX = transform.position.x;
    }
    public void KeyInput()
    {
        if (plState != PlayerState.Attack &&
            plState != PlayerState.Farrying)
        {
            if (Input.GetButton("Horizontal"))
            {
                GameManager.GManager.PlState = PlayerState.Move;

                inputX = Input.GetAxisRaw("Horizontal");
                moveX = transform.position.x + inputX * moveSpeed * Time.deltaTime;

                transform.position = new Vector2(moveX, transform.position.y);
            }
            else
                GameManager.GManager.PlState = PlayerState.Idle;

            if (Input.GetKeyDown(KeyCode.Space) && ground)
            {
                GameManager.GManager.PlState = PlayerState.Jump;

                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
