using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UiManager : MonoBehaviour
{
    #region 싱글톤 설정

    static UiManager uiManager;
    public static UiManager UIManager { get { return uiManager; } set { uiManager = value; } }

    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public Button optionBtn;
    public Button closeBtn;

    public Slider lifeBar;

    public GameObject gameOverPanel;

    PlayerBattle pB;
    PlayerBattleState pBState;

    [Tooltip("보스 처치시 화톳불")]
    public GameObject bossCleared;
    [Tooltip("보스 클리어 텍스트")]
    public GameObject bossClearLabel;

    ObjectAudio[] objectAudios;


/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        TimePlay();
        pB = GameObject.FindWithTag("Player").GetComponent<PlayerBattle>();
    }

    private void Update()
    {
        GamePauseBtn();
    }

/*-----------------------------------------------------------------------------------------------------------------------------------*/

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

    // 체력 변화 있을시
    public void PlayerHpBarChange(float playerHp, float maxHp)
    {
        lifeBar.value = playerHp / maxHp;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void LoadVolume()
    {
        objectAudios = FindObjectsOfType<ObjectAudio>();

        foreach (ObjectAudio audio in objectAudios)
            audio.VolumeSetting();
        
    }

    #region 게임매니저의 옵션 관련

    public void GoToTitle()
    {
        GameManager.GM.GoToTitle();
    }
    public void ExitGame()
    {
        GameManager.GM.ExitGame();
    }
    public void RestartStage()
    {
        GameManager.GM.RestartGame();
    }

    // 일시정지, 해제
    public void TimePause()
    {
        GameManager.GM.TimePause();
    }
    public void TimePlay()
    {
        GameManager.GM.TimePlay();
    }

    // 카메라 움직임 제한
    public void CamCanMove()
    {
        GameManager.StopCam = false;
    }
    public void CamCantMove()
    {
        GameManager.StopCam = true;
    }

    #endregion

    #region 보스 관련

    // 보스 처치
    public void BossClear()
    {
        bossCleared.SetActive(true);
        StartCoroutine(BossClearTxt());
    }

    /// <summary>
    /// 보스 처치 텍스트 활성화 및 설정 시간 후 비활성
    /// </summary>
    /// <returns></returns>
    IEnumerator BossClearTxt()
    {
        bossClearLabel.SetActive(true);

        yield return new WaitForSeconds(3f);

        bossClearLabel.SetActive(false);
    }

    #endregion
}
