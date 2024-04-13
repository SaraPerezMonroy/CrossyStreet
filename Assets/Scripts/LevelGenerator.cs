using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject terrainPrefab;
    public GameObject[] initialTerrain;
    public GameObject[] proceduralTerrain;
    public GameObject[] randomOneObstacle;
    public GameObject spawn;
    public GameObject proceduralSpawn;
    public GameObject oneObstacleSpawn;


    // Start is called before the first frame update
    void Start()
    {
        int randomIndex = Random.Range(0, initialTerrain.Length);
        int obstacleRandomIndex = Random.Range(0, randomOneObstacle.Length);
        initialTerrain[randomIndex].transform.position = spawn.transform.position;
        randomOneObstacle[obstacleRandomIndex].transform.position = oneObstacleSpawn.transform.position;
        proceduralTerrain[randomIndex].transform.position = proceduralSpawn.transform.position; 
    }

    public void RecycleTerrain(GameObject terrain)
    {

    }

}
