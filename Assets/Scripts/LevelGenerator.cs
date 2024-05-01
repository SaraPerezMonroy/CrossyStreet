using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] initialTerrain;
    public GameObject spawn;
    void Start()
    {
        int randomIndex = Random.Range(0, initialTerrain.Length); // RNG que escoge de los 4 terrenos principales
        initialTerrain[randomIndex].transform.position = spawn.transform.position; // Movemos el terreno escogido al spawn
    }

}
