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



    public GameObject gameOverPanel;

    public GameObject bossCleared;
    public GameObject bossClearLabel;

    public void GoToTitle()
    {
        GameManager.GManager.GoToTitle();
    }

    public void ExitGame()
    {
        GameManager.GManager.ExitBtn();
    }

    public void RestartStage()
    {
        GameManager.GManager.RestartGame();
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

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
