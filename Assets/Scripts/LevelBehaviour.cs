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
    public int backSteps;

    public bool canMove = true;

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

    void MoveTarget(Vector3 direction)
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
        textSteps.text = "Score: " + steps.ToString() + "\nRecord: " + record.ToString();
    }
}