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
        instance = this;
    }

    //player
    [SyncVar]
    private int playercnt = 0;
    [SyncVar]
    public List<Transform> player_transform = new List<Transform>();
    public GameObject localPlayer;

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
            NetworkManager.singleton.StopHost();
        }
    }

    [ClientRpc]
    public void RpcOnEnemyDie(int coin, int score)
    {   
        localPlayer.gameObject.GetComponent<HudUpdate>().ChangeCoin(coin);
        localPlayer.gameObject.GetComponent<HudUpdate>().ChangeScore(score);
    }

    [ClientRpc]
    public void RpcDestoryZombie(GameObject zombie)
    {
        Destroy(zombie);
    }
}
