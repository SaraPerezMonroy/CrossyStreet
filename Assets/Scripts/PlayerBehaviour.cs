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

    public ParticleSystem waterParticles;

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
        SwipeController.instance.OnSwipe += MoveTarget; // Suscribimos el método
    }

    public void OnDestroy()
    {
        SwipeController.instance.OnSwipe -= MoveTarget; // Desuscribimos el método cuando el script del swipr controller se desactiva
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
                if (directionMove.x != 0) // Si es cero, ajustamos realmente a cero, evitar diagonales
                {
                    directionMove.x = 0;
                }
            }

            if (directionMove != Vector3.zero) // Si nos movemos en alguna dirección rotamos al jugador
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

                if (direction.normalized.z < 0 && backSteps < 3)  // Si nos movemos hacia atrás, sumamos a backsteps, recordar que solo puede caminar 3 hacia atrás
                {
                    backSteps++;
                }

                if (direction.normalized.z > 0 && backSteps == 0 && levelBehaviour.stopAddingSteps == false) // Si los backsteps son 0, dejamos que vuelva a sumar pasos adelante
                {
                    steps++;
                    UI.instance.UpdateTextSteps(steps);
                }
                if (direction.normalized.z > 0 && backSteps > 0) // Si nos movemos hacia delante después de habernos movido hacia atrás, se van restando estos pasos atrás
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
            canJump = true; // Si tocamos un tag de esos podremos saltar 
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            platformTransform = collision.transform; // Al chocar con un tronco, se hace hijo de este para que pueda arrastrarlo
            transform.SetParent(platformTransform); // Convertir al jugador en hijo de la plataforma
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin")) // Añadir 1 coin
        {
            UI.instance.coinAmount += 1;
            other.gameObject.SetActive(false);
            UI.instance.DisplayText();
            coinSound.Play();
        }
        if(other.gameObject.CompareTag("Enemy")) // Nos mata tren o coche
        {
            UI.instance.GameEnding(hitEffect);
            this.gameObject.SetActive(false);
            playerIsDead = true;
        }
        if (other.gameObject.CompareTag("Water")) // Nos caemos al agua
        {
            waterParticles.transform.position = transform.position;
            waterParticles.Play();
            UI.instance.GameEnding(waterEffect);
            this.gameObject.SetActive(false);
            playerIsDead = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform")) // Si sale del contacto con la plataforma
        {
            platformTransform = null;
            transform.SetParent(null); // Convertir al jugador en hijo del mundo 
            canJump = false; // Para que no pueda moverse hasta estar en contacto con otro coso
        }
    }
}
