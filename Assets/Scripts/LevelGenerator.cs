using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] initialTerrain;
    public GameObject[] proceduralTerrain;
    public GameObject spawn;
    public GameObject proceduralSpawn;


    // Start is called before the first frame update
    void Start()
    {

        int randomIndex = Random.Range(0, initialTerrain.Length);
        initialTerrain[randomIndex].transform.position = spawn.transform.position;
        proceduralTerrain[randomIndex].transform.position = proceduralSpawn.transform.position; 
    }

}
