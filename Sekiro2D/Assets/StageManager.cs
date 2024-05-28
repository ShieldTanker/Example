using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject gameOverPanel;

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
}
