using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCleared : MonoBehaviour
{
    public GameObject actionKeyText;
    bool isActive;

    private void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameManager.GManager.GoToTitle();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player");
            actionKeyText.SetActive(true);
            isActive = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Out");
            actionKeyText.SetActive(false);
            isActive = false;
        }
    }
}
