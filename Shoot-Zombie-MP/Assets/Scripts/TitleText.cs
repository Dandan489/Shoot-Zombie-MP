using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class TitleText : NetworkBehaviour
{
    public static TitleText instance;
    public TMP_Text txt;
    private float lastShown;
    private float duration = 3f;
    private bool active = false;

    private void Awake()
    {
        txt = GameObject.Find("UniversalCanvas").transform.Find("WaveTitle").GetComponent<TMP_Text>();
        instance = this;
        GameObject.Find("UniversalCanvas").transform.Find("WaveTitle").gameObject.SetActive(false);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        GameObject.Find("UniversalCanvas").transform.Find("WaveTitle").gameObject.SetActive(false);
    }

    private void Update()
    {
        if(active && ((Time.time - lastShown) >= duration))
        {
            Hide();
        }
    }

    [ClientRpc]
    public void RpcShow(string a)
    {
        Debug.Log(a);
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

