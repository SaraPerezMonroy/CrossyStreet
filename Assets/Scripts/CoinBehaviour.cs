using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public GameObject coin;
    public float rotationSpeed = 100f;

    void Start()
    {
        coin = this.gameObject;
    }

    void Update()
    {
        coin.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // Para que gire jeje
    }
}
