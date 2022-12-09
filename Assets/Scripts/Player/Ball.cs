using UnityEngine;
using Zenject;
using Zenject.Signals;

public class Ball : MonoBehaviour
{
    private AudioController _audioController;
    private AudioSource _audioSource;
    private SignalBus _signalBus;
    
    private const string BasketTopTag = "BasketTop";
    private const string LeftBorderTag = "LeftBorder";
    private const string RightBorderTag = "RightBorder";

    public bool IsAttached { get; private set; }
    public Rigidbody2D BallRb { get; private set; }

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Awake()
    {
        BallRb = GetComponent<Rigidbody2D>();
        BallRb.isKinematic = true;
        _audioController = GetComponent<AudioController>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void ToggleAttachBall(bool toggle)
    {
        IsAttached = toggle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var col = collision.gameObject;
        
        if (col.CompareTag(BasketTopTag))
            col.GetComponentInParent<Basket>().OffClearBasket();

        if(col.CompareTag(BasketTopTag) || col.CompareTag(LeftBorderTag)|| col.CompareTag(RightBorderTag))
            _audioController.PlayKnokAudio();
    }

    public void ToggleAudio(bool toggle)
    {
        _audioSource.mute = !toggle;
    }

    public void StarFire()
    {

        _signalBus.Fire<StarChangeSignal>();
    }
}
