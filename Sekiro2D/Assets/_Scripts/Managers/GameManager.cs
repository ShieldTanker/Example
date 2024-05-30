using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject settingImg;

    private static GameManager gManager;
    static bool stopCam;
    static bool gamePause;


    public static GameManager GManager { get { return gManager; } set { gManager = value; } }
    public static bool StopCam { get { return stopCam; } set { stopCam = value; } }
    public static bool GamePause { get { return gamePause; } set { gamePause = value; } }


    private void Awake()
    {
        if (gManager == null) { gManager = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
    }


    // ���� ���� ��ư
    public void StartGame()
    {
        SceneManager.LoadScene("Play");
    }
    public void ExitGame()
    {
        Application.Quit();
    }


    // �ΰ��� ��ư
    public void GoToTitle()
    {
        SceneManager.LoadScene("Menu");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // �Ͻ�����,����
    public void TimePause()
    {
        Time.timeScale = 0f;
        GamePause = true;
        StopCam = true;
    }
    public void TimePlay()
    {
        Time.timeScale = 1f;
        GamePause = false;
        StopCam = false;
    }
}
