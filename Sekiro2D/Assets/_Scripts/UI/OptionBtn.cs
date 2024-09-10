using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionBtn : MonoBehaviour
{
    public GameObject settingPanel;
    [Tooltip("활성, 비활성 시킬 버튼들")]
    public GameObject[] buttons;

    FallZone fallZone;
    bool noFallZone;

    /*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        GameObject tmp = GameObject.FindWithTag("FallZone");
        if (tmp != null)
        {
            fallZone = tmp.GetComponent<FallZone>();

            if (fallZone == null)
                Debug.Log("FallZone 없음");
            
        }
        else
            noFallZone = true;
    }

    /*-----------------------------------------------------------------------------------------------------------------------------------*/

    /// <summary>
    /// 옵션 버튼 눌렀을때 설정창 활성, 배열 안의 있는 버튼들 비활성
    /// </summary>
    public void OptionBtnClicked()
    {
        if (noFallZone || !fallZone.IsFall)
        {
            settingPanel.SetActive(true);

            foreach (GameObject item in buttons)
            {
                item.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 옵션 버튼 눌렀을때 설정창 비활성, 배열 안의 있는 버튼들 활성
    /// </summary>
    public void CloseOptionBtnClicked()
    {
        if (noFallZone || !fallZone.IsFall)
        {
            settingPanel.SetActive(false);

            foreach (GameObject item in buttons)
            {
                item.SetActive(true);
            }
        }
    }
}
