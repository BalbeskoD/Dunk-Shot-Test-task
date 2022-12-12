using UnityEngine;
using Enums;
using Zenject;
using Zenject.Signals;

public class Borders : MonoBehaviour
{
    [SerializeField] private BordersLocations bordersLocations;
    
    private PlayerController _playerController;
    private SpawnManager _spawnManager;
    private SignalBus _signalBus;


    private float _width;
    private const  float DownBorderOffsetY = 4.0f;
    private bool _isGameActive;

    [Inject]
    public void Construct(SpawnManager spawnManager, PlayerController playerController, SignalBus signalBus)
    {
        _spawnManager = spawnManager;
        _playerController = playerController;
        _signalBus = signalBus;
    }

    private void Awake()
    {
        _signalBus.Subscribe<GameStartSignal>(OnStart);
        _signalBus.Subscribe<GameRestartSignal>(OnRestart);
        _width = Camera.main.orthographicSize * Screen.width / Screen.height;
    }
    private void OnDestroy()
    {
        _signalBus.Unsubscribe<GameStartSignal>(OnStart);
        _signalBus.Unsubscribe<GameRestartSignal>(OnRestart);
    }
    private void FixedUpdate()
    {
        
        if (bordersLocations == BordersLocations.Left)
        {
            transform.position = Camera.main.transform.position - new Vector3(_width, 0,10.0f);
        }
        else if (bordersLocations == BordersLocations.Right)
        {
            transform.position = Camera.main.transform.position + new Vector3(_width, 0, 10.0f);
        }
        else if (bordersLocations == BordersLocations.Down)
        {
            if (_playerController.IsControlable)
            {
                transform.position = new Vector3(Camera.main.transform.position.x, _spawnManager.BasketPull[_spawnManager.ActiveBasket].transform.position.y - DownBorderOffsetY, Camera.main.transform.position.z);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball && _isGameActive) 
        {
            _signalBus.Fire<FinishSignal>();
        }
        else if (ball)
        {
            _spawnManager.SpawnBall();
        }
    }

    private void OnStart()
    {
        _isGameActive = true;
    }
    private void OnRestart()
    {
        _isGameActive = false;
    }
}

