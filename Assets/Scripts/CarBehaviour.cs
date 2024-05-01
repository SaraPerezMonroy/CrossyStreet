using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public Transform[] waypoints;
    public float propSpeed = 5f;

    public GameObject car;
    public float distanciaMinima = 10f;

    public AudioSource carBeep;


    bool cantBeep = false;
    private void Start()
    {
        car = this.gameObject;
    }

    void FixedUpdate()
    {
        if (transform.position != waypoints[0].position) // Para que vaya de un waypoint a otro
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0].position, propSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = waypoints[1].position;
        }

        float distancia = Vector3.Distance(this.transform.position, PlayerBehaviour.instance.transform.position);
        if (distancia < distanciaMinima && !cantBeep) // Si está más cerca de la distancia mínima y puede pitar
        {
            if(Random.Range(0.0f,1.0f) < 0.002f) // Si el número aleatorio es menor que 0.002f, es true, se hacen estas cosas
            {
                carBeep.PlayOneShot(carBeep.clip);
                cantBeep = true;
                StartCoroutine(CanBeep());
            }
        }
    }
    IEnumerator CanBeep()
    {
        yield return new WaitForSeconds(5);
        cantBeep = false;
    }
}