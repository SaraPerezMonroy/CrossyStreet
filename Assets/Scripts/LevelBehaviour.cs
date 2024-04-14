using TMPro;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    public PlayerBehaviour playerBehaviour;

    public float offset = 100f;
    public float animationDuration = 0.25f;


    GameObject terrain;
    [SerializeField] 
    GameObject player;

    public int steps = 0;


    public int backSteps;

    public bool canMove = true;

    public void Awake()
    {
        terrain = this.gameObject;
        steps = 0;
    }

    public void Start()
    {
        SwipeController.instance.OnSwipe += MoveTarget;
    }

    public void Update()
    {
       
    }


    public void OnDisable()
    {
        SwipeController.instance.OnSwipe -= MoveTarget;
    }
    public void MoveTarget(Vector3 direction)
    {
        RaycastHit hitInfo = PlayerBehaviour.rayCast;
        if (playerBehaviour != null && playerBehaviour.canJump && canMove)
        {
            if (Physics.Raycast(playerBehaviour.transform.position + new Vector3(0, 1f, 0), direction, out hitInfo, 1f))
            {
                if (hitInfo.collider.tag != "ProceduralTerrain")
                {
                    if (direction.z != 0)
                    {
                        direction.z = 0;
                    }
                }
            }
            if (direction.normalized.z >= 0 && playerBehaviour.backSteps == 0)
            {
                LeanTween.move(terrain, terrain.transform.position + new Vector3(0, 0, -direction.normalized.z), animationDuration).setEase(LeanTweenType.easeOutQuad);
            }            
            if (playerBehaviour.backSteps == 0 && direction.z >= 0 && Mathf.Abs(direction.x) < Mathf.Abs(direction.z))
            {
                steps++;
                UI.instance.UpdateTextSteps(steps);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerBehaviour.canJump = true;
        }
    }


}