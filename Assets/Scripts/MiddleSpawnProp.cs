using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleSpawnProp : MonoBehaviour
{
    public List<GameObject> objectsListMiddle;
    public List<GameObject> inactiveObjectsMiddle = new List<GameObject>();
    public GameObject activeObjectMiddle;

    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject propParent;

    private int propsActivated = 0;


    private void Start()
    {
        foreach (GameObject prefab in objectsListMiddle) // Para cada objeto que esté en la lista
        {
            prefab.SetActive(false); // Lo desactivamos
            inactiveObjectsMiddle.Add(prefab); // Lo movemos a inactivos
        }

        SpawnProp();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == activeObjectMiddle) // Cuando sale de este trigger, se lo pasamos a la lista de activos 
        {
            SpawnProp();
        }
    }

    public void SpawnProp()
    {
        if (inactiveObjectsMiddle.Count > 0)
        {
            int randomIndex = Random.Range(0, inactiveObjectsMiddle.Count);

            activeObjectMiddle = inactiveObjectsMiddle[randomIndex];
            activeObjectMiddle.SetActive(true);
            GameObject Coin = activeObjectMiddle.transform.GetChild(0).gameObject; // Cada vez que desactivamos el prop, activamos su primer hijo (la moneda)
            Coin.SetActive(true);

            activeObjectMiddle.transform.position = spawnPoint.transform.position;

            inactiveObjectsMiddle.RemoveAt(randomIndex);

            activeObjectMiddle.transform.parent = propParent.transform;

            propsActivated++;
        }
    }
}