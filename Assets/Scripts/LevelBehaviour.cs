using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelBehaviour : MonoBehaviour
{
    public float animationDuration = 0.25f;
    public bool stopAddingSteps = false;

    GameObject terrain;
    public int backSteps;

    public void Awake()
    {
        terrain = this.gameObject;
    }

    public void Start()
    {
        SwipeController.instance.OnSwipe += MoveTarget; // Suscribimos el m�todo
    }

    public void OnDisable()
    {
        SwipeController.instance.OnSwipe -= MoveTarget; // Desuscribimos el m�todo cuando el script del swipr controller se desactiva
    }
    public void MoveTarget(Vector3 direction)
    {
        RaycastHit hitInfo = PlayerBehaviour.rayCast;
        if (PlayerBehaviour.instance != null && PlayerBehaviour.instance.canJump) // Si existe el player, podemos saltar y adem�s podemos movernos
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
                if (hitInfo.collider.tag == "Obstacle") // Deja de saltar si tenemos un obst�culo delante (�rboles, arbustos, piedras, troncos, casas...)
                {
                    stopAddingSteps = true;
                }
            }
            else
            {
                stopAddingSteps=false;
            }
        
           if(direction.z < 0 && PlayerBehaviour.instance.backSteps < 3) // Solo nos podemos mover 3 veces hacia atr�s como en el juego real, aunque aqu� no nos come un buitre :(
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
            PlayerBehaviour.instance.canJump = true; // De esta manera conseguimos que el jugador no pueda volar, ni saltar demasiado r�pido
        }
    }

}