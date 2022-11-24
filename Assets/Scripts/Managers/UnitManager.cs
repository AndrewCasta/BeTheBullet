using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] GameObject meeleEnemyPrefab;
    [SerializeField] int[] hpRange = new int[2];
    GameObject[] spawnPoints;

    void Start()
    {
        GetSpawnPoints();
        SpawnUnits();
    }

    private void SpawnUnits()
    {
        foreach (var sp in spawnPoints)
        {
            var enemy = Instantiate(meeleEnemyPrefab, sp.transform);
            enemy.GetComponent<MeeleEnemyController>().CurrentHP = Random.Range(hpRange[0], hpRange[1] + 1);
        }
    }

    void Update()
    {

    }

    private void GetSpawnPoints()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("UnitSpawnPoint");
    }
}
