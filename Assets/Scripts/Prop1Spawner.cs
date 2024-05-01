using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop1Spawner : MonoBehaviour
{
     public List<GameObject> objectsList;
    public List<GameObject> inactiveObjects = new List<GameObject>();
    public GameObject activeObject;

    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject propParent;

    private int propsActivated = 0;

    [SerializeField] GameObject middleSpawnProp;

    private void Start()
    {
        foreach (GameObject prefab in objectsList) // Para cada objeto que esté en la lista
        {
            prefab.SetActive(false); // Lo desactivamos
            inactiveObjects.Add(prefab); // Lo movemos a inactivos
        }
        SpawnProp();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == activeObject) // Cuando sale de este trigger, se lo pasamos a la lista de activos
        {
            SpawnProp();
        }
    }

    public void SpawnProp()
    {
        if (propsActivated <4 && propsActivated>2) // Si los props de 1 salto que se han activado son más de 2 y menos de 4 (3 jeje)
        {
            this.enabled = false; // Desactivamos el script
            middleSpawnProp.SetActive(true); // Activamos el otro spawner
        }
        else
        {
            if (inactiveObjects.Count > 0) // Si los inactivos son mayor a 0
            {
                int randomIndex = Random.Range(0, inactiveObjects.Count); // Cogemos uno aleatorio
                activeObject = inactiveObjects[randomIndex]; // Metemos objeto aleatorio del array al objeto activo
                activeObject.SetActive(true); // Lo activamos

                GameObject Coin = activeObject.transform.GetChild(0).gameObject; // Cada vez que desactivamos el prop, activamos su primer hijo (la moneda)
                Coin.SetActive(true);

                activeObject.transform.position = spawnPoint.transform.position; // Movemos el activo al spawn
                inactiveObjects.RemoveAt(randomIndex); // Lo quitamos de inactivos
                activeObject.transform.parent = propParent.transform; // Lo metemos en una carpetita de props
                propsActivated++; // Sumamos a los props activados 
            }
           
        }
      
}
}