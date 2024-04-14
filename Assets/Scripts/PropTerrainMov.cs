using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SwipeController;

public class PropTerrainMov : MonoBehaviour
{
    Vector3 clickInicial;
    Vector3 alSoltarClick;
    Vector3 click;
    public float offset = 100f;

    [SerializeField] 
    GameObject player;
    [SerializeField] 
    GameObject prop;


    public float timeAnim = 0.1f;
    public void Awake()
    {
        prop = this.gameObject;
    }
    private void Update()
    {
        RaycastHit hitInfo = PlayerBehaviour.rayCast;
        if (Input.GetMouseButtonDown(0) && !PlayerBehaviour.instance.playerIsDead && PlayerBehaviour.instance.moveLevel)
        {
            clickInicial = Input.mousePosition;
            click = Vector3.forward;
        }

        if (Input.GetMouseButtonUp(0) && !PlayerBehaviour.instance.playerIsDead)
        {
            alSoltarClick = Input.mousePosition;
            Vector3 diferencia = alSoltarClick - clickInicial;

            if (Mathf.Abs(diferencia.magnitude) < offset)
            {
                Vector3 forwardDirection = prop.transform.forward; 
                if (Physics.Raycast(PlayerBehaviour.instance.transform.position + new Vector3(0, 1f, 0), forwardDirection, out hitInfo, 1f))
                {
                    if (hitInfo.collider.tag != "Terrain")
                    {
                        forwardDirection = Vector3.zero; 
                    }
                }

                LeanTween.move(prop, prop.transform.position + new Vector3(0, 0, -forwardDirection.normalized.z), 0.25f).setEase(LeanTweenType.easeOutQuad);
            }
            else 
            {
                diferencia = diferencia.normalized;
                diferencia.z = diferencia.y;

                if (Mathf.Abs(diferencia.x) > Mathf.Abs(diferencia.z))
                {
                    diferencia.z = 0.0f;
                }
                else
                {
                    diferencia.x = 0.0f;
                }

                diferencia.y = 0.0f;
                if (Physics.Raycast(PlayerBehaviour.instance.transform.position + new Vector3(0, 1f, 0), diferencia, out hitInfo, 1f))
                {
                    if (hitInfo.collider.tag != "Terrain")
                    {
                        if (diferencia.z != 0)
                        {
                            diferencia.z = 0;
                        }
                    }
                }

                if (diferencia.normalized.z >= 0)
                {
                    LeanTween.move(prop, prop.transform.position + new Vector3(0, 0, -diferencia.normalized.z), 0.25f).setEase(LeanTweenType.easeOutQuad);
                }
                if (diferencia.normalized.z < 0 && PlayerBehaviour.instance.backSteps < 3)
                {
                    LeanTween.move(prop, prop.transform.position + new Vector3(0, 0, -diferencia.normalized.z), 0.25f).setEase(LeanTweenType.easeOutQuad);
                }
            }
        }
    }
}
