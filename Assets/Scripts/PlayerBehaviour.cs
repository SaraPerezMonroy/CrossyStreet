using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerBehaviour : MonoBehaviour
{
    public SwipeController swipeEvent;
    [SerializeField]
    public GameObject player;
    public float timeAnim = 0.4f;

    public bool canJump;

    public static RaycastHit rayCast;

    public int backSteps;



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
            RaycastHit hitInfo;
            Vector3 directionMove = direction.normalized;
            if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), directionMove, out hitInfo, 1f))
            {
                rayCast = hitInfo;
                if (directionMove.x != 0)
                {
                    directionMove.x = 0;
                }
            }

            if (directionMove != Vector3.zero)
            {
                if (directionMove.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 90f, 0);
                }
                else if (directionMove.x < 0)
                {
                    transform.eulerAngles = new Vector3(0, -90f, 0);
                }
                else if (directionMove.z > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (directionMove.z < 0)
                {
                    transform.eulerAngles = new Vector3(0, -180f, 0);
                }

            LeanTween.move(player, player.transform.position + (new Vector3(direction.x, 0, 0) + Vector3.up) / 2, timeAnim / 2).setOnComplete(() =>
            {
                LeanTween.move(player, player.transform.position + (new Vector3(direction.x, 0, 0) - Vector3.up) / 2, timeAnim / 2);
            });
                if (backSteps < 3 && direction.normalized.z <= 0)
                {
                    backSteps++;
                    LeanTween.move(player, player.transform.position + new Vector3(direction.x / 2, 0, direction.z / 2) + Vector3.up / 2, timeAnim / 2).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
                    {
                        LeanTween.move(player, player.transform.position + new Vector3(direction.x / 2, 0, direction.z / 2) - Vector3.up / 2, timeAnim / 2).setEase(LeanTweenType.easeOutQuad);
                    });
                }
                if (backSteps != 0 && direction.normalized.z >= 0)
                {
                    backSteps--;
                    LeanTween.move(player, player.transform.position + new Vector3(direction.x / 2, 0, direction.z / 2) + Vector3.up / 2, timeAnim / 2).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
                    {
                        LeanTween.move(player, player.transform.position + new Vector3(direction.x / 2, 0, direction.z / 2) - Vector3.up / 2, timeAnim / 2).setEase(LeanTweenType.easeOutQuad);
                    });
                }
                canJump = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("ProceduralTerrain") || collision.gameObject.CompareTag("Plataform"))
        {
            canJump = true; 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            UI.instance.coinAmount += 1;
            other.gameObject.SetActive(false);
            UI.instance.DisplayText();
        }
        if(other.gameObject.CompareTag("Enemy"))
        {
            UI.instance.GameEnding();
            this.gameObject.SetActive(false);
            SwipeController.instance.enabled = false;
        }
        if (other.gameObject.CompareTag("Plataform"))
        {
            Vector3 plataformaPosition = other.gameObject.transform.position;
            transform.position += plataformaPosition - transform.position;
        }

    }

}
