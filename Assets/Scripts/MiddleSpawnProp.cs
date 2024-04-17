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
        foreach (GameObject prefab in objectsListMiddle)
        {
            prefab.SetActive(false);
            inactiveObjectsMiddle.Add(prefab);
        }

        SpawnRandomPrefab();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == activeObjectMiddle)
        {
            SpawnRandomPrefab();
        }
    }

    public void SpawnRandomPrefab()
    {
        if (inactiveObjectsMiddle.Count > 0)
        {
            int randomIndex = Random.Range(0, inactiveObjectsMiddle.Count);

            activeObjectMiddle = inactiveObjectsMiddle[randomIndex];
            activeObjectMiddle.SetActive(true);
            GameObject Coin = activeObjectMiddle.transform.GetChild(0).gameObject;
            Coin.SetActive(true);

            activeObjectMiddle.transform.position = spawnPoint.transform.position;

            inactiveObjectsMiddle.RemoveAt(randomIndex);

            activeObjectMiddle.transform.parent = propParent.transform;

            propsActivated++;
        }
    }
}