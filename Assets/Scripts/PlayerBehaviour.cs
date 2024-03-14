using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehaviour : MonoBehaviour
{
    public SwipeController swipeEvent;
    public GameObject player;
    public float timeAnim = 0.4f;

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
        if (direction.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 90f, 0);
        }
        else if(direction.x < 0)
        {
            transform.eulerAngles = new Vector3(0, -90f, 0);
        }
       
        LeanTween.move(player, player.transform.position + (direction + Vector3.up) / 2, timeAnim / 2).setOnComplete(() =>
        {
            LeanTween.move(player, player.transform.position + (direction - Vector3.up) / 2, timeAnim / 2);
        });
    }
}
