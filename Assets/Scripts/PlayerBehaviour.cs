using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerBehaviour : MonoBehaviour
{
    public SwipeController swipeEvent;
    public static PlayerBehaviour instance;

    [SerializeField]
    public GameObject player;
    public float timeAnim = 0.4f;

    public bool canJump;
    public bool playerIsDead = false;

    public static RaycastHit rayCast;

    public int backSteps;
    public LevelBehaviour levelBehaviour;

    private Transform platformTransform;

    public AudioSource coinSound;
    public int steps = 0;

    public AudioSource waterEffect;
    public AudioSource hitEffect;

    private void Awake()
    {
        player = this.gameObject;
        steps = 0;
        if (PlayerBehaviour.instance == null)
        {
            PlayerBehaviour.instance = this;
        }
        else
        {
            Destroy(this);
        }

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

                if (direction.normalized.z < 0 && backSteps < 3)  //abajo y que se sumen los stepsback
                {
                    backSteps++;
                }

                if (direction.normalized.z > 0 && backSteps == 0 && levelBehaviour.stopAddingSteps == false) //arriba y que sume si los steps back son cero 
                {
                    steps++;
                    UI.instance.UpdateTextSteps(steps);
                }
                if (direction.normalized.z > 0 && backSteps > 0)
                {
                    backSteps--;
                }
                canJump = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("ProceduralTerrain") || collision.gameObject.CompareTag("Platform"))
        {
            canJump = true;
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            platformTransform = collision.transform;
            transform.SetParent(platformTransform); // Convertir al jugador en hijo de la plataforma
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            UI.instance.coinAmount += 1;
            other.gameObject.SetActive(false);
            UI.instance.DisplayText();
            coinSound.Play();
        }
        if(other.gameObject.CompareTag("Enemy"))
        {
            UI.instance.GameEnding(hitEffect);
            this.gameObject.SetActive(false);
            playerIsDead = true;
        }
        if (other.gameObject.CompareTag("Water"))
        {
            UI.instance.GameEnding(waterEffect);
            this.gameObject.SetActive(false);
            playerIsDead = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            platformTransform = null;
            transform.SetParent(null); // Convertir al jugador en hijo del mundo 
            canJump = false;
        }
    }
}
