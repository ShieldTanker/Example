using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCleared : MonoBehaviour
{
    [Tooltip("상호작용 키")]
    public GameObject actionKeyText;

    [Tooltip("UI 활성화 되어있는지")]
    bool isActive;
    [Tooltip("보스 처치시 나오는 화톳불 안에 있는지 확인")]
    bool inBossClear;
    
    [SerializeField] PlayerBattle playerBattle;

    public GameObject gotoTitleBtn;
    public GameObject exitBtn;

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        playerBattle = GameObject.FindWithTag("Player").GetComponent<PlayerBattle>();
        isActive = true;
    }

    private void Update()
    {
        if (inBossClear)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                gotoTitleBtn.SetActive(isActive);
                exitBtn.SetActive(isActive);
                GameManager.StopCam = isActive;
                actionKeyText.SetActive(!isActive);

                playerBattle.Hp = playerBattle.maxHp;
                playerBattle.ChangeHpBarValue();
                isActive = !isActive;
            }
        }
    }

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player");
            actionKeyText.SetActive(true);
            inBossClear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Out");
            actionKeyText.SetActive(false);

            gotoTitleBtn.SetActive(false);
            exitBtn.SetActive(false);

            GameManager.StopCam = false;
            isActive = true;
            inBossClear =false;
        }
    }
}
