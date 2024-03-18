using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPool : MonoBehaviour
{
    [SerializeField]
    public GameObject terrainType1;
    public int terrainCount;
    public Vector3 spawnTerrain;

    void Start()
    {
        ObjectPool.PreLoad(terrainType1, terrainCount);
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        spawnTerrain = terrainType1.transform.position;
        for (int i = 0; i < 2; i++)
        {
            GameObject terrain = ObjectPool.GetObject(terrainType1);
            terrain.transform.position = spawnTerrain + Vector3.forward * i * terrainType1.GetComponent<Renderer>().bounds.size.z;
        }
    }
    public void RecycleTerrain(GameObject terrain)
    {
        ObjectPool.RecicleObject(terrainType1, terrain);
    }

    public void AddTerrain()
    {
        GameObject terrain = ObjectPool.GetObject(terrainType1);
        terrain.transform.position = spawnTerrain + Vector3.forward * terrainType1.GetComponent<Renderer>().bounds.size.z;
    }
}