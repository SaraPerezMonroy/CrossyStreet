using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    public SwipeController swipeEvent;
    public GameObject terrain;
    public float timeAnim = 0.4f;
    public GameObject player;

    private void Awake()
    {
        terrain = this.gameObject;
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
        if (direction.z > 0)
        {
            player.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction.z < 0)
        {
            player.transform.eulerAngles = new Vector3(0, -180f, 0);
        }
        LeanTween.move(terrain, terrain.transform.position + new Vector3(0,0, -direction.z) / 2, timeAnim / 2).setEase(LeanTweenType.easeOutCirc);
    }
    }