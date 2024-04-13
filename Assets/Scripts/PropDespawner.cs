using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDespawner : MonoBehaviour
{
    public PropSpawner propSpawner;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prop"))
        {
            // Si el objeto que entra en el otro collider es un "prop", añadirlo a la lista de prefabs a spawnear
            propSpawner.inactiveProps.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

}