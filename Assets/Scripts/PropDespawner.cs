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
            prop1Spawn.inactiveObjects.Add(other.gameObject); // Se añade a la lista de props del inicio el objeto que entra al collider, si es "prop", que son los de uno
            other.gameObject.SetActive(false); // Se desactiva 
            other.gameObject.transform.parent = null; // Se le quita el emparentado
        }
        if (other.CompareTag("PropMiddle"))
        {
            middleSpawnProp.inactiveObjectsMiddle.Add(other.gameObject); // Se añade a la lista de props de la mitad (los infinitos) el objeto que entra al collider, si es "propMiddle", que son los de dos y cuatro saltos y los descansos
            other.gameObject.SetActive(false);
            other.gameObject.transform.parent = null;
        }
    }
}