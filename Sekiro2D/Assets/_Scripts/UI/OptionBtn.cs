using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionBtn : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject[] buttons;


    // 옵션 버튼
    public void OptionBtnClicked()
    {
        settingPanel.SetActive(true);

        foreach (var item in buttons)
        {
            item.SetActive(false);
        }
    }

    public void CloseOptionBtnClicked()
    {
        settingPanel.SetActive(false);

        foreach (var item in buttons)
        {
            item.SetActive(true);
        }
    }
}
