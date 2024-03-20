using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehaviour : MonoBehaviour
{
    public SwipeController swipeEvent;
    public GameObject player;
    public float timeAnim = 0.4f;

    public bool canJump;

    public static RaycastHit m_RaycastDirection;
    private void Awake()
    {
        player = this.gameObject;
    }


    public void Start()
    {
        SwipeController.instance.OnSwipe += MoveTarget;

    }

    public void OnDestroy()
    {
        SwipeController.instance.OnSwipe -= MoveTarget;
    }

    void MoveTarget(Vector3 direction)
    {
        if(canJump)
        {
            RaycastHit m_Hitinfo;
            Vector3 m_MoveDirection = direction.normalized;
            if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), m_MoveDirection, out m_Hitinfo, 1f))
            {
                Debug.Log("Hit Something, Restricting Movement");
                m_RaycastDirection = m_Hitinfo;
                if (m_MoveDirection.x != 0)
                {
                    m_MoveDirection.x = 0;
                }
            }

            if (m_MoveDirection != Vector3.zero)
            {
                if (m_MoveDirection.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 90f, 0);
                }
                else if (m_MoveDirection.x < 0)
                {
                    transform.eulerAngles = new Vector3(0, -90f, 0);
                }
                else if (m_MoveDirection.z > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (m_MoveDirection.z < 0)
                {
                    transform.eulerAngles = new Vector3(0, -180f, 0);
                }

            LeanTween.move(player, player.transform.position + (new Vector3(direction.x, 0, 0) + Vector3.up) / 2, timeAnim / 2).setOnComplete(() =>
            {
                LeanTween.move(player, player.transform.position + (new Vector3(direction.x, 0, 0) - Vector3.up) / 2, timeAnim / 2);
            });
            canJump = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("ProceduralTerrain"))
        {
            canJump = true; 
        }
    }
}
