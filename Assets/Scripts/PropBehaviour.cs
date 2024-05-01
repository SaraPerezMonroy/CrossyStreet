using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBehaviour : MonoBehaviour
{

    public Transform[] waypoints;
    public float propSpeed = 5f;


    void FixedUpdate()
    {
        if (transform.position != waypoints[0].position) // Para que se mueva de un waypoint al otro
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0].position, propSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = waypoints[1].position;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float centerZ = transform.position.z; 
            Vector3 playerPosition = collision.transform.position;
            playerPosition.z = centerZ; // Calcular el centro del tronco en el eje z y establecerlo para el jugador
            collision.transform.position = playerPosition; // Convertir la posición del player en la del objeto con el que colisionó
            collision.transform.SetParent(this.transform); // Emparentarlo
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // Quitarle el parent
        }
    }
}