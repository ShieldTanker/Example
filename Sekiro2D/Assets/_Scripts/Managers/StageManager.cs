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

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ExitGame()
    {
        GameManager.GManager.ExitBtn();
    }

    public void RestartStage()
    {
        GameManager.GManager.RestartBtn();
    }

    public void BossClear()
    {
        bossCleared.SetActive(true);
    }
}
