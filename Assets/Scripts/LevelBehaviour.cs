using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class LevelBehaviour : MonoBehaviour
{
    public SwipeController swipeController;
    public LevelGenerator levelGenerator;
    public PlayerBehaviour playerBehaviour;

    public float offset = 100f;
    public float animationDuration = 0.25f;    
    public bool canMove = true;
    public GameObject terrain;

    public int stepsCounter;

    private bool isRecycled = false;
    public int counter = 0;


    public void Awake()
    {
        terrain = this.gameObject;
    }


    public void OnEnable()
    {
        swipeController.OnSwipe += MoveTarget;
    }

    public void OnDisable()
    {
        swipeController.OnSwipe -= MoveTarget;
    }

    void MoveTarget(Vector3 m_Direction)
    {
        RaycastHit raycastHit = PlayerBehaviour.m_RaycastDirection;

        if (playerBehaviour != null && playerBehaviour.canJump && canMove)
        {
            if (Physics.Raycast(playerBehaviour.transform.position + new Vector3(0, 1f, 0), m_Direction, out raycastHit, 1f))
            {
                if (raycastHit.collider.tag != "ProceduralTerrain")
                {
                    if (m_Direction.z != 0)
                    {
                        m_Direction.z = 0;
                    }
                }
            }
            if (m_Direction != Vector3.zero)
            {
                LeanTween.move(terrain, terrain.transform.position + new Vector3(0, 0, -m_Direction.normalized.z), animationDuration).setEase(LeanTweenType.easeOutQuad);
            }

            Debug.Log(stepsCounter);
            if (m_Direction.normalized.z == 1)
            {
                stepsCounter++;
            }
            if (m_Direction.normalized.z == -1)
            {
                stepsCounter--;
            }
        }
    }

    public void Update()
    {
        if (counter == 2 && isRecycled == true)
        {
            counter = 0;
            isRecycled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canMove = false;
        }
    }
}