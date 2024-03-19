using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    public SwipeController swipeEvent;
    public GameObject terrain;
    public float timeAnim = 0.4f;
    public GameObject player;

    public PlayerBehaviour playerBehaviour;

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
        if (playerBehaviour.canJump && playerBehaviour != null)
        {
            LeanTween.move(terrain, terrain.transform.position + new Vector3(0, 0, -direction.normalized.z), timeAnim).setEase(LeanTweenType.easeOutCirc);
        }
    }


}