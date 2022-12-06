using UnityEngine;

public class Ball : MonoBehaviour
{
    
    private Rigidbody2D ballRb;
    private bool isAttached;
    private AudioController audioController;
    private AudioSource audioSource;
    private static readonly string basketTopTag = "BasketTop";
    private static readonly string leftBorderTag = "LeftBorder";
    private static readonly string rightBorderTag = "RightBorder";

    public bool IsAttached => isAttached;

    public Rigidbody2D BallRb => ballRb;



    private void Awake()
    {
        ballRb = GetComponent<Rigidbody2D>();
        ballRb.isKinematic = true;
        audioController = GetComponent<AudioController>();
        audioSource = GetComponent<AudioSource>();
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

        if(collision.gameObject.CompareTag(basketTopTag) || collision.gameObject.CompareTag(leftBorderTag)|| collision.gameObject.CompareTag(rightBorderTag))
        {
            audioController.PlayKnokAudio();
        }
    }

    public void ToggleAudio(bool toggle)
    {
        audioSource.mute = !toggle;
    }
}
