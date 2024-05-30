using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCleared : MonoBehaviour
{
    public GameObject actionKeyText;
    
    //public bool isActive;
    
    public PlayerBattle playerBattle;

    public GameObject gotoTitleBtn;
    public GameObject exitBtn;

    private void Start()
    {
        playerBattle = GameObject.Find("Player").GetComponent<PlayerBattle>();
    }

    private void Update()
    {
        if (GameManager.StopCam)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                gotoTitleBtn.SetActive(true);
                exitBtn.SetActive(true);
                actionKeyText.SetActive(false);

                playerBattle.playerHp = playerBattle.playerMaxHP;
                playerBattle.ChangeHpBarValue();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player");
            actionKeyText.SetActive(true);
            GameManager.StopCam = true;
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
        }
    }
}
