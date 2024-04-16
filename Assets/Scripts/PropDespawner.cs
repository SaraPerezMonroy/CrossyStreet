using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDespawner : MonoBehaviour
{
    public Prop1Spawner prop1Spawn;
    public MiddleSpawnProp middleSpawnProp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prop"))
        {
            // Si el objeto que entra en el otro collider es un "prop", añadirlo a la lista de prefabs a spawnear
            prop1Spawn.inactiveObjects.Add(other.gameObject);
            other.gameObject.SetActive(false);
            other.gameObject.transform.parent = null;
        }
        if (other.CompareTag("PropMiddle"))
        {
            middleSpawnProp.inactiveObjectsMiddle.Add(other.gameObject);
            other.gameObject.SetActive(false);
            other.gameObject.transform.parent = null;
        }
    }
}