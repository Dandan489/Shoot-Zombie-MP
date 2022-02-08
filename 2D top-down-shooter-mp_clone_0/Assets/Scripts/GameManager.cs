using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //player
    private int playercnt = 0;
    public List<Transform> player_transform = new List<Transform>();

    public void New_Player(Transform player)
    {
        playercnt++;
        player_transform.Add(player);
    }

    public void Kill_Player(Transform player)
    {
        playercnt--;
        player_transform.Remove(player);
        if(playercnt == 0)
        {
            Debug.Log("Game Over");
            Application.Quit();
        }
    }

    public void OnEnemyDie(int coin, int score, GameObject zombie)
    {
        Debug.Log(zombie);
        RpcDestoryZombie(zombie);
        ServerDestoryZombie(zombie);
        foreach (Transform player in player_transform)
        {
            player.gameObject.GetComponent<HudUpdate>().ChangeCoin(coin);
            player.gameObject.GetComponent<HudUpdate>().ChangeScore(score);
        }
    }

    [Server]
    public void ServerDestoryZombie(GameObject zombie)
    {
        Debug.Log(zombie);
        Destroy(zombie);
    }

    [ClientRpc]
    public void RpcDestoryZombie(GameObject zombie)
    {
        Debug.Log(zombie);
        Destroy(zombie);
    }
}
