using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kTools.Pooling;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        GameOver
    }

    [SerializeField] float SawSpawnTime = 10f;
    [SerializeField] int SawInstanceCount = 20;
    [SerializeField] GameObject[] SawTypes;

    public static GameManager instance { get; private set; }

    private void Awake() => instance = this;

    private void Start()
    {
        foreach (var item in SawTypes)
        {
            PoolingSystem.CreatePool(item.name, item, SawInstanceCount);
        }

        InvokeRepeating("SpawnSaw", 3, SawSpawnTime);
    }

    void SpawnSaw()
    {
        string randomSaw = SawTypes[Random.Range(0, SawTypes.Length - 1)].name;

        GameObject saw;
        //if (!PoolingSystem.TryGetInstance(randomSaw, out saw)) return;
        Instantiate(SawTypes[0]);
    }

}
