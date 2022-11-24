using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    private Rigidbody2D ballRb;
    private bool isAttached;
    private CircleCollider2D ballCollider;

    public bool IsAttached => isAttached;

    public Rigidbody2D BallRb => ballRb;

    private void Awake()
    {
        ballRb = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<CircleCollider2D>();
    }

    public void ToggleAttachBall(bool toggle)
    {
        isAttached = toggle;
    }

}
