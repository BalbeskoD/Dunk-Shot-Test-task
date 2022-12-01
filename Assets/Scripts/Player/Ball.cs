using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    private Rigidbody2D ballRb;
    private bool isAttached;
    private CircleCollider2D ballCollider;
    private SpawnManager _spawnManager;
    private AudioController audioController;
    private static readonly string basketTopTag = "BasketTop";
    private static readonly string sideBorderTag = "SideBorder";

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
        audioController = GetComponent<AudioController>();
    }

    public void ToggleAttachBall(bool toggle)
    {
        isAttached = toggle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(basketTopTag))
        {
            collision.gameObject.GetComponentInParent<Basket>().DisactivateClearBasket();
        }

        if(collision.gameObject.CompareTag(basketTopTag) || collision.gameObject.CompareTag(sideBorderTag))
        {
            audioController.PlayKnokAudio();
        }
    }
}
