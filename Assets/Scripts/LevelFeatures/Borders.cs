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


    private float width;
    private float height;
    private bool isGameActive;

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
    }
    private void OnDestroy()
    {
        _signalBus.Unsubscribe<GameStartSignal>(OnStart);
        _signalBus.Unsubscribe<GameRestartSignal>(OnRestart);
    }
    private void FixedUpdate()
    {
        width = Camera.main.orthographicSize * Screen.width / Screen.height;
        height = Camera.main.orthographicSize * Screen.height / Screen.width / 2;
        if (bordersLocations == BordersLocations.Left)
        {
            transform.position = Camera.main.transform.position - new Vector3(width, 0);
        }
        else if (bordersLocations == BordersLocations.Right)
        {
            transform.position = Camera.main.transform.position + new Vector3(width, 0);
        }
        else if (bordersLocations == BordersLocations.Down)
        {
            if (_playerController.IsControlable)
            {
                transform.position = Camera.main.transform.position - new Vector3(0, height, 0);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball && isGameActive) 
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
        isGameActive = true;
    }
    private void OnRestart()
    {
        isGameActive = false;
    }
}

