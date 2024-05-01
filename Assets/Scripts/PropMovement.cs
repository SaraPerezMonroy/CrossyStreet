using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropMovement : MonoBehaviour 
{
    // Este script es parecido al del level behaviour, porque también necesitamos que se mueva con la misma lógica 
    public float animationDuration = 0.25f;
    public bool stopAddingSteps = false;

    public GameObject terrain;
    public int backSteps;

    public void Awake()
    {
        terrain = this.gameObject;
    }

    public void Start()
    {
        SwipeController.instance.OnSwipe += MoveTarget;
    }

    public void OnDisable()
    {
        SwipeController.instance.OnSwipe -= MoveTarget;
    }
    public void MoveTarget(Vector3 direction)
    {
        RaycastHit hitInfo = PlayerBehaviour.rayCast;
        if (PlayerBehaviour.instance != null && PlayerBehaviour.instance.canJump) // Todo lo estamos usando del player
        {
            if (Physics.Raycast(PlayerBehaviour.instance.transform.position + new Vector3(0, 1f, 0), direction, out hitInfo, 1f))
            {
                if (hitInfo.collider.tag != "ProceduralTerrain")
                {
                    if (direction.z != 0)
                    {
                        direction.z = 0;
                    }
                }
                if (hitInfo.collider.tag == "Obstacle")
                {
                    stopAddingSteps = true;
                }
            }
            else
            {
                stopAddingSteps = false;
            }

            if (direction.z < 0 && PlayerBehaviour.instance.backSteps < 3)
            {
                LeanTween.move(terrain, terrain.transform.position + new Vector3(0, 0, -direction.normalized.z), animationDuration).setEase(LeanTweenType.easeOutQuad);
            }
            if (direction.z > 0)
            {
                LeanTween.move(terrain, terrain.transform.position + new Vector3(0, 0, -direction.normalized.z), animationDuration).setEase(LeanTweenType.easeOutQuad);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerBehaviour.instance.canJump = true;
        }
    }

}