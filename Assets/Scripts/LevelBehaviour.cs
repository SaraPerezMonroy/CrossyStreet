using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelBehaviour : MonoBehaviour
{
    public float animationDuration = 0.25f;
    public bool stopAddingSteps = false;

    GameObject terrain;
    public int backSteps;
    public bool canMove = true;

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
        if (PlayerBehaviour.instance != null && PlayerBehaviour.instance.canJump && canMove)
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
                stopAddingSteps=false;
            }
        
           if(direction.z < 0 && PlayerBehaviour.instance.backSteps < 3)
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canMove = false;
        }
    }

}