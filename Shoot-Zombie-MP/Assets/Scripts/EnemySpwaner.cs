using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemySpwaner : NetworkBehaviour
{
    public static EnemySpwaner instance;
    public GameObject zombie;
    public GameObject bigZombie;

    private int alreadySpawn = 0;
    private float nextspawn = 0f;
    public float spawnCooldown = 1f;
    private int spawnscnt = 0;
    public int enemycnt = 0;

    private int[] enemyperwave = {5, 10, 15, 20, 25, 30, 35, 40, 45, 55};
    private List<Transform> SpawnLocations = new List<Transform>();

    //wave
    private IEnumerator coroutine2;
    public int wave = 0;
    public bool ongoingwave = false;
    public bool ongoingwaveupdate = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach(Transform child in gameObject.transform)
        {
            spawnscnt++;
            SpawnLocations.Add(child.transform);
        }
        NextWave();
    }

    private void Update() {
        if (ongoingwave == true && Time.time >= nextspawn)
        {
            Spawn();
            nextspawn = Time.time + spawnCooldown;
        }
        if (ongoingwaveupdate == false && enemycnt == 0)
        {
            Debug.Log("Wave " + wave + " ended");
            NextWave();
        }
    }

    private void Spawn()
    {
        alreadySpawn++;
        enemycnt++;
        SpawnLocation(zombie);
        if (wave >= 5 && alreadySpawn % 5 == 0)
        {
            SpawnLocation(bigZombie);
        }
        if(alreadySpawn == enemyperwave[wave - 1])
        {
            ongoingwave = false;
            alreadySpawn = 0;
        }
    }

    private void SpawnLocation(GameObject enemyprefab)
    {
        int location = Random.Range(0, spawnscnt);
        GameObject enemy = Instantiate(enemyprefab, SpawnLocations[location].position,  new Quaternion(0, 0, 0, 0));
        NetworkServer.Spawn(enemy);
    }

    public void NextWave()
    {
        ongoingwaveupdate = true;
        wave++;
        if (wave == 11)
        {
            Application.Quit();
        }
        coroutine2 = BetweenWave();
        StartCoroutine(coroutine2);
    }

    IEnumerator BetweenWave()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Wave " + wave + " start");
        TitleText.instance.RpcShow("Wave " + wave);
        ongoingwave = true;
        ongoingwaveupdate = false;
    }
}
