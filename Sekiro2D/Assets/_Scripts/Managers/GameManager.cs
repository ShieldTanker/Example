using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameManager gManager;
    public GameManager GManager { get { return gManager; } set { gManager = value; }}

    public GameObject startBtn;
    public GameObject exitBtn;
    public GameObject settingImg;

    public Slider volumeSlider;


    public void StartBtn()
    {
        SceneManager.LoadScene("Play");
    }
    public void ExitBtn()
    {
        Application.Quit();
    }

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

    private void Awake()
    {
        if (gManager == null) { gManager = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }
}
