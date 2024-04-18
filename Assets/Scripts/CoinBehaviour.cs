using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public GameObject coin;
    public float rotationSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        coin = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        coin.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); 
    }
}
