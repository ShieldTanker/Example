using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager gManager;
    public static GameManager GManager { get { return gManager; } set { gManager = value; }}

    public GameObject startBtn;
    public GameObject exitBtn;
    public GameObject settingImg;

    public Slider volumeSlider;
    float lastVol;
    private void Awake()
    {
        if (gManager == null) { gManager = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    // 시작 종료 버튼
    public void StartBtn()
    {
        SceneManager.LoadScene("Play");
    }
    public void ExitBtn()
    {
        Application.Quit();
    }

    // 옵션 버튼
    public void OptionBtnClicked()
    {
        startBtn.SetActive(false);
        exitBtn.SetActive(false);
        gameObject.SetActive(false);

        settingImg.SetActive(true);
    }
    public void CloseOptionBtnClicked()
    {
        startBtn.SetActive(true);
        exitBtn.SetActive(true);
        gameObject.SetActive(true);

        settingImg.SetActive(false);
    }
    public void SaveVolume()
    {
        if (lastVol == volumeSlider.value)
            return;

        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();

        Debug.Log("볼륨 저장");
        lastVol = volumeSlider.value;
    }

    // 인게임 버튼
    public void GoToTitle()
    {
        SceneManager.LoadScene("Menu");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
