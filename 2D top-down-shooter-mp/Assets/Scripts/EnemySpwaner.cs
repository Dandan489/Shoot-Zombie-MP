using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
    public GameObject zombie;
    public GameObject bigZombie;
    private int alreadySpawn = 0;
    private float nextspawn = 0f;
    public float spawnCooldown = 1f;
    private int spawnscnt = 0;
    private int[] enemyperwave = {5, 10, 15, 20, 25, 30, 35, 40, 45, 55};
    private List<Transform> SpawnLocations = new List<Transform>();

    private void Start()
    {
        foreach(Transform child in gameObject.transform)
        {
            spawnscnt++;
            SpawnLocations.Add(child.transform);
        }
    }

    private void Update() {
        if(GameManager.instance.ongoingwave == true && Time.time >= nextspawn){
            Spawn();
            nextspawn = Time.time+spawnCooldown;
        }
    }

    private void Spawn()
    {
        alreadySpawn++;
        GameManager.instance.enemycnt++;
        SpawnLocation(zombie);
        if (GameManager.instance.wave >= 5 && alreadySpawn % 5 == 0)
        {
            SpawnLocation(bigZombie);
        }
        if(alreadySpawn == enemyperwave[GameManager.instance.wave - 1])
        {
            GameManager.instance.ongoingwave = false;
            alreadySpawn = 0;
        }
    }

    private void SpawnLocation(GameObject enemyprefab)
    {
        int location = Random.Range(0, spawnscnt);
        Instantiate(enemyprefab, SpawnLocations[location].position,  new Quaternion(0, 0, 0, 0));
    }
}
