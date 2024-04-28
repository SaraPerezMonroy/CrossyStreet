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
    public float soundProbability;


    bool cantBeep = false;
    private void Start()
    {
        car = this.gameObject;
        soundProbability = Random.Range(0, 1);


    }

    void FixedUpdate()
    {
        if (transform.position != waypoints[0].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0].position, propSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = waypoints[1].position;
        }

        float distancia = Vector3.Distance(this.transform.position, PlayerBehaviour.instance.transform.position);

        
        if (distancia < distanciaMinima && !cantBeep)
        {
            
            if(Random.Range(0.0f,1.0f) < 0.002f)
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