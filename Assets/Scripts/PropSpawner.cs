using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    public List<GameObject> propList = new List<GameObject>(); 
    public List<GameObject> inactiveProps = new List<GameObject>(); 

    private GameObject activeObject; 
    public GameObject spawn; 

    void Start()
    {
        foreach (GameObject obj in propList) //  Se desactivan todos los GameObjects en propList y se agregan a inactiveProps
        {
            obj.SetActive(false);
            inactiveProps.Add(obj);
        }

        // Spawnear el primer objeto
        SpawnRandomObject();
    }

    void OnTriggerExit(Collider other)
    {
        // Cuando el objeto activo sale del trigger, spawnear otro objeto
        if (other.gameObject == activeObject)
        {
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()
    {
        // Si hay objetos inactivos disponibles
        if (inactiveProps.Count > 0)
        {
            // Obtener un índice aleatorio
            int randomIndex = Random.Range(0, inactiveProps.Count);

            // Activar un objeto inactivo aleatorio
            activeObject = inactiveProps[randomIndex];
            activeObject.SetActive(true);

            // Establecer la posición del objeto spawn
            activeObject.transform.position = spawn.transform.position;

            // Remover el objeto activado de la lista de objetos inactivos
            inactiveProps.RemoveAt(randomIndex);
        }
    }
}
