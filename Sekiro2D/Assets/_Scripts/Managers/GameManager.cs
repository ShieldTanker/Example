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

    // ���� ���� ��ư
    public void StartBtn()
    {
        SceneManager.LoadScene("Play");
    }
    public void ExitBtn()
    {
        Application.Quit();
    }

    // �ɼ� ��ư
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

        Debug.Log("���� ����");
        lastVol = volumeSlider.value;
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
}
