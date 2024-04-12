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

    public CoinBehaviour coinBehaviour;
    public int backSteps;

    [SerializeField]
    public GameObject gameEndingScreen;
    [SerializeField]
    public GameObject gameUI;
    [SerializeField]
    public TextMeshProUGUI textEnding;
    [SerializeField]
    public TextMeshProUGUI newRecordLabel;

    public LevelBehaviour levelBehaviour;
    public SwipeController swipeController;

    public SkinnedMeshRenderer meshPlayer;
    [SerializeField]
    public CapsuleCollider colliderPlayer;
    public Rigidbody rb;

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
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("ProceduralTerrain"))
        {
            canJump = true; 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coinBehaviour.coinAmount += 1;
            other.gameObject.SetActive(false);
            coinBehaviour.DisplayText();
        }
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Obstacle"))
        {
            rb.isKinematic = true;
            canJump = false;
            meshPlayer.enabled = false;
            colliderPlayer.enabled = false;
            swipeController.enabled = false;

            gameEndingScreen.SetActive(true);
            textEnding.text = "Total coins: " + coinBehaviour.coinAmount + "\nTotal steps: " + levelBehaviour.steps;
            gameUI.SetActive(false);
            if (levelBehaviour.newRecord)
            {
                newRecordLabel.text = "Nuevo record!";
            }
            else
            {
                newRecordLabel.text = "Record: " + levelBehaviour.record;
            }
        }
    }

}
