using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSubscriber : MonoBehaviour
{


    public EventCreator eventoEnter;


    public void OnEnable()
    {
        eventoEnter.OnPresionarEnter += HaPresionadoEnter; // Se suscribe al evento
    }

    public void OnDisable()
    {
        eventoEnter.OnPresionarEnter -= HaPresionadoEnter; // Elimina la suscripción
    }


    private void HaPresionadoEnter()
    {
        Debug.Log("Ha presionado Enter");
    }
}
