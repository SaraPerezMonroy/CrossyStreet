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
        SpawnRandomProp();
    }

    void OnTriggerExit(Collider other) // El spawner funciona si el objeto activo sale del trigger
    {
        if (other.gameObject == activeObject) 
        {
            SpawnRandomProp();
        }
    }

    void SpawnRandomProp()
    {
        if (inactiveProps.Count > 0)
        {
            int randomIndex = Random.Range(0, inactiveProps.Count); // Índice aleatorio de la lista
            activeObject = inactiveProps[randomIndex]; 
            activeObject.SetActive(true);
            activeObject.transform.position = spawn.transform.position; // Establecer la posición del spawn
            inactiveProps.RemoveAt(randomIndex); // Quita el prop activado de la lista de inactivo
        }
    }
}
