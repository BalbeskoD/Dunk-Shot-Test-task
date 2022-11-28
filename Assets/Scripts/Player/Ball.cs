using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    private Rigidbody2D ballRb;
    private bool isAttached;
    private CircleCollider2D ballCollider;
    private SpawnManager _spawnManager;

    public bool IsAttached => isAttached;

    public Rigidbody2D BallRb => ballRb;

    public void Construct(SpawnManager spawnManager)
    {
        _spawnManager = spawnManager;
    }

    private void Awake()
    {
        ballRb = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<CircleCollider2D>();
    }

    public void ToggleAttachBall(bool toggle)
    {
        isAttached = toggle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var basket = collision.gameObject.GetComponent<Basket>();
        if (basket)
        {
            basket.DisactivateClearBasket();
        }
    }
}
