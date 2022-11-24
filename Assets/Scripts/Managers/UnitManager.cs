using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] GameObject meleeEnemyPrefab;
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
            var enemy = Instantiate(meleeEnemyPrefab, sp.transform);
            enemy.GetComponent<MeleeEnemyController>().CurrentHP = Random.Range(hpRange[0], hpRange[1] + 1);
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
