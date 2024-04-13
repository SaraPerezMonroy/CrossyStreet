using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBehaviour : MonoBehaviour
{
    public Transform[] waypoints;
    [SerializeField]
    public float propSpeed;

    void Update()
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