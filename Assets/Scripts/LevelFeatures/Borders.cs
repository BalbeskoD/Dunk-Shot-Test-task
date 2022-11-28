using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Zenject;
using Zenject.Signals;

public class Borders : MonoBehaviour
{
    [SerializeField] private BordersLocations bordersLocations;
    
    private PlayerController _playerController;
    private PointCounter _pointCounter;
    private SpawnManager _spawnManager;
    private SignalBus _signalBus;
    private CameraController _cameraController;

    private float width;
    private float height;

    public void Construct(SpawnManager spawnManager, PlayerController playerController, PointCounter pointCounter, SignalBus signalBus, CameraController cameraController)
    {
        _spawnManager = spawnManager;
        _playerController = playerController;
        _pointCounter = pointCounter;
        _signalBus = signalBus;
        _cameraController = cameraController;
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
        if (ball && _pointCounter.PointCount > 0) 
        {
            _signalBus.Fire<FinishSignal>();
        }
        else if (ball)
        {
            _spawnManager.SpawnBall();
        }
    }
}

