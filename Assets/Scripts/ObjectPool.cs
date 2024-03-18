using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance;
    static Dictionary<int, Queue<GameObject>> pool = new Dictionary<int, Queue<GameObject>>(); // Diccionario con los objetos
    static Dictionary<int, GameObject> parentsPool = new Dictionary<int, GameObject>(); // Diccionario para la jerarquía
    private void Awake() 
    {
        if (instance == null) 
        {
            instance = this;
        }
        else 
        {
            Destroy(this);
        }

    }

    public static void PreLoad(GameObject prefab, int amount) 
    {
        int id = prefab.GetInstanceID(); // Función de Unity que sirve para devolver el id del objeto, viendo que es único y almacenamos en id el orden en el que se creó el objeto
        GameObject parentPool = new GameObject(); 
        parentPool.name = prefab.name + " ParentPool"; 
        parentsPool.Add(id, parentPool); // Añadir al diccionario, el de la jerarquía, para luego poder meterle las cositas dentro y estar ordenadas

        pool.Add(id, new Queue<GameObject>());

        for (int i = 0; i < amount; i++) // Cantidad de objetos que tendremos en las piscinas
        {
            CreateObject(prefab);
        }
    }

    static void CreateObject(GameObject prefab) 
    {
        int id = prefab.GetInstanceID(); // Instancio el ID, al coger el identificador del prefab elijo en qué piscina lo meto
        GameObject copiaPrefab = Instantiate(prefab) as GameObject;
        copiaPrefab.transform.SetParent(Getparent(id).transform); // Aquí le decimos qué piscina es, hacemos padre el ParentPool y hacemos hijo al objeto que acabamos de copiar
        copiaPrefab.SetActive(false);
        pool[id].Enqueue(copiaPrefab);
    }
    static GameObject Getparent(int parentID) // Devuelve el identificador del padre y se lo pasa como un parámetro a la clave del diccionario
    {
        GameObject parent;
        parentsPool.TryGetValue(parentID, out parent); 
        return parent; // Le pasamos al diccionario el identificador
    }

    public static GameObject GetObject(GameObject prefab) // Para coger los objetos y sacarlos
    {
        int id = prefab.GetInstanceID(); 
        if (pool[id].Count == 0) // Si la pool está vacía crea un objeto, después (o si no está vacía) lo saca de la cola
        {
            CreateObject(prefab);
        }
        GameObject copiaPrefab = pool[id].Dequeue(); // Sacamos el primer objeto de la cola, es como el pop
        copiaPrefab.SetActive(true);
        return copiaPrefab;
    }
    public static void RecicleObject(GameObject prefab, GameObject objectToRecicle) 
    {
        int id = prefab.GetInstanceID();
        pool[id].Enqueue(objectToRecicle); 
        objectToRecicle.SetActive(false);
    }
    public static void ClearPool()
    {
        foreach (var m_FirstDictionary in pool)
        {
            Queue<GameObject> queue = m_FirstDictionary.Value;
            foreach (GameObject m_Obj in queue)
            {
                Destroy(m_Obj);
            }
            queue.Clear();
        }

        pool.Clear();
        foreach (var m_SecondDictionary in parentsPool)
        {
            GameObject parent = m_SecondDictionary.Value;
            Destroy(parent);
        }
        parentsPool.Clear();
    }
}

