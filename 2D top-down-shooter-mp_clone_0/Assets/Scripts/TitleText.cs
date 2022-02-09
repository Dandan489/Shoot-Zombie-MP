using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleText : MonoBehaviour
{
    public static TitleText instance;
    public TMP_Text txt;
    private float lastShown;
    private float duration = 3f;
    private bool active = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(active && ((Time.time - lastShown) >= duration))
        {
            Hide();
        }
    }

    public void Show(string a)
    {
        lastShown = Time.time;
        txt.text = a;
        txt.gameObject.SetActive(true);
        active = true;
    }

    public void Hide()
    {
        txt.gameObject.SetActive(false);
        active = false;
    }
}

