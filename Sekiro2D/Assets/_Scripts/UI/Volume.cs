using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Volume : MonoBehaviour
{
    public Slider volumeSlider;
    float lastVol;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    public void SaveVolume()
    {
        if (lastVol == volumeSlider.value)
            return;

        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();


        Debug.Log("º¼·ý ÀúÀå");
        lastVol = volumeSlider.value;
    }
}
