using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDespawner : MonoBehaviour
{
    public PropSpawner propSpawner;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prop"))  // A�ade el prop que entra en el otro collider a la lista de los prefabs que spawnear�an
        {
            propSpawner.inactiveProps.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}