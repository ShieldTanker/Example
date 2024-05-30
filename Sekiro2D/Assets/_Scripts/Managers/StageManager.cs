using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    StageManager sM;
    public StageManager SM { get { return sM; } set { sM = value; } }

    private void Awake()
    {
        if (sM == null)
        {
            sM = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public Button optionBtn;
    public Button closeBtn;

    public GameObject gameOverPanel;

    public GameObject bossCleared;
    public GameObject bossClearLabel;

    private void Update()
    {
        GamePauseBtn();
    }

    void GamePauseBtn()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.GamePause)
                closeBtn.onClick.Invoke();
            else
                optionBtn.onClick.Invoke();
        }
    }

    public void GoToTitle()
    {
        GameManager.GManager.GoToTitle();
    }
    public void ExitGame()
    {
        GameManager.GManager.ExitGame();
    }
    public void RestartStage()
    {
        GameManager.GManager.RestartGame();
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    // 일시정지
    public void TimePause()
    {
        GameManager.GManager.TimePause();
    }
    public void TimePlay()
    {
        GameManager.GManager.TimePlay();
    }

    // 카메라 움직임
    public void CamCanMove()
    {
        GameManager.StopCam = false;
    }
    public void CamCantMove()
    {
        GameManager.StopCam = true;
    }

    // 보스 처치
    public void BossClear()
    {
        bossCleared.SetActive(true);
        StartCoroutine(BossClearTxt());
    }
    IEnumerator BossClearTxt()
    {
        bossClearLabel.SetActive(true);

        yield return new WaitForSeconds(3f);

        bossClearLabel.SetActive(false);
    }
}
