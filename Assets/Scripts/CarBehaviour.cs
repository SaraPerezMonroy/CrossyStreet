using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public Transform[] waypoints;
    public float propSpeed = 5f;

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
    }
}