using TMPro;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    public SwipeController swipeController;
    public LevelGenerator levelGenerator;
    public PlayerBehaviour playerBehaviour;

    public float offset = 100f;
    public float animationDuration = 0.25f;

    [SerializeField]
    GameObject terrain;
    [SerializeField] 
    GameObject player;

    private int record = 0;
    public int steps = 0;
    [SerializeField] 
    TextMeshProUGUI textSteps;

    public void Awake()
    {
        terrain = this.gameObject;
    }

    public void Start()
    {
        steps = PlayerPrefs.GetInt("Steps: ", 0);
        record = PlayerPrefs.GetInt("Record: ", 0);
        UpdateTextSteps();
    }

    public void Update()
    {
        PlayerPrefs.SetInt("Steps", steps);
        PlayerPrefs.Save();

        if (steps > record)
        {
            record = steps;
            PlayerPrefs.SetInt("Record", record);
            PlayerPrefs.Save();
        }
        UpdateTextSteps();
    }

    public void OnEnable()
    {
        swipeController.OnSwipe += MoveTarget;
    }

    public void OnDisable()
    {
        swipeController.OnSwipe -= MoveTarget;
    }

    public void MoveTarget(Vector3 t_Direction)
    {
        if (Mathf.Abs(t_Direction.x) > Mathf.Abs(t_Direction.z))
        {
            return;
        }

        RaycastHit t_HitInfo = PlayerBehaviour.rayCast;

        if (playerBehaviour.canJump)
        {
            if (Physics.Raycast(player.transform.position + new Vector3(0, 1f, 0), t_Direction, out t_HitInfo, 1f))
            {
                Debug.Log("Hit Something, Restricting Movement");
                if (t_HitInfo.collider.tag != "ProceduralTerrain")
                {
                    if (t_Direction.z != 0)
                    {
                        t_Direction.z = 0;
                    }
                }

                Debug.DrawRay(transform.position + new Vector3(0, 1f, 0), transform.forward * t_HitInfo.distance, Color.red);
            }

            if (t_Direction != Vector3.zero)
            {
                LeanTween.move(terrain, terrain.transform.position + new Vector3(0, 0, -t_Direction.normalized.z), animationDuration).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
                {
                    if (t_Direction.z >= -3)
                    {
                        steps += 1;
                    }
                });
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

    private void UpdateTextSteps()
    {
        textSteps.text = "Score: " + steps.ToString() + "/" + "Record: " + record.ToString();
    }
}