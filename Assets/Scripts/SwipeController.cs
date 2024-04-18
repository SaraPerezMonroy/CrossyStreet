using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SwipeController : MonoBehaviour
{
    public static SwipeController instance;

    Vector3 clickInicial;
    Vector3 alSoltarClick;
    Vector3 click;

    [SerializeField]
    public float offset = 100f; // Para poder moverse de verdad tiene que ser mayor a 100, por si se hace clic sin querer
   
    public delegate void Swipe(Vector3 direction);
    public event Swipe OnSwipe;
    private void Awake()
    {
        if (SwipeController.instance == null)
        {
            SwipeController.instance = this;
        }
        else
        {
            Destroy(this);
        }

    }
 
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            clickInicial = Input.mousePosition;
            click = Vector3.forward;
        }

        if(Input.GetMouseButtonUp(0))
        {
            alSoltarClick = Input.mousePosition;
            Vector3 diferencia = alSoltarClick - clickInicial;
            if(Mathf.Abs(diferencia.magnitude) > offset) // Tengo un número calculado para ver si es mayor que el offset
            {
                diferencia = diferencia.normalized; // Generamos un vector donde los números están del 1 al 0
                diferencia.z = diferencia.y;

                if(Mathf.Abs(diferencia.x) > Mathf.Abs(diferencia.z)) 
                {
                    diferencia.z = 0.0f;
                }
                else
                {
                    diferencia.x = 0.0f;
                }

                diferencia.y = 0.0f;

                if (OnSwipe != null)
                {
                    OnSwipe(diferencia);
                }
            }
               
            else
            {
              Vector3 clickVect = click;
              if (OnSwipe != null)
              {
                   OnSwipe(clickVect);
              }
                }
            }
        }
 }