using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Zenject;
using Zenject.Signals;

public class PlayerController : MonoBehaviour
{
    private Ball _ball;
    private SpawnManager _spawnManager;
    private SignalBus _signalBus;
    
    private Vector2 _mousePos;
    private Vector2 _startMousePos;
    private Vector2 _totalForce;
    
    private float _screenHeight;
    private float _totalScale;
    private readonly float _playerLaunchSpeed = 2.0f;
    
    private bool _isControlable;
    

    public Vector2 MousePos => _mousePos;
    public Vector2 StartMousePos => _startMousePos;
    public float TotalScale => _totalScale;
    public bool IsControlable => _isControlable;

    [Inject]
    public void Constructor(Ball ball, SpawnManager spawnManager, SignalBus signalBus)
    {
        _ball = ball;
        _spawnManager = spawnManager;
        _signalBus = signalBus;
    }

    private void Awake()
    {
        _screenHeight = Screen.height/2.5f;
        _signalBus.Subscribe<GoalSignal>(ActiveControl);
        _signalBus.Subscribe<ClearGoalSignal>(ActiveControl);
        _signalBus.Subscribe<BallReturnSignal>(ActiveControl);
        _signalBus.Subscribe<GameStartSignal>(OnStart);
        _signalBus.Subscribe<GameRestartSignal>(OnRestart);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<GoalSignal>(ActiveControl);
        _signalBus.Unsubscribe<ClearGoalSignal>(ActiveControl);
        _signalBus.Unsubscribe<BallReturnSignal>(ActiveControl);
        _signalBus.Unsubscribe<GameStartSignal>(OnStart);
        _signalBus.Unsubscribe<GameRestartSignal>(OnRestart);
    }

    private void Update()
    {
        if (!_ball.IsAttached) return;
        
        _ball.BallRb.transform.position = _spawnManager.BasketPull[_spawnManager.ActiveBasket].BallPoint.transform.position;
        _ball.BallRb.rotation = 0;
        
        if (!_isControlable) return;
        
        if (Input.GetMouseButtonDown(0))
        {
                _totalScale = 0;
                UpdateMousePosition();
                _startMousePos = _mousePos;
        }
        
        if (Input.GetMouseButton(0)) 
            OnMouseAction();
        
        if (Input.GetMouseButtonUp(0) && _totalScale > 1.2f) 
            OnMouseUpAction();
       
    }

    private void FixedUpdate()
    {
        
        
    }

    private void UpdateMousePosition()
    {
        _mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    
    private void OnMouseAction()
    {
        var activeBasket = _spawnManager.BasketPull[_spawnManager.ActiveBasket];
        
        UpdateMousePosition();

        var tempAngle = Mathf.Atan2(_startMousePos.y - _mousePos.y, _startMousePos.x - _mousePos.x) * Mathf.Rad2Deg;
        if (tempAngle != 0)
        {
            float angle = tempAngle - 90;
            activeBasket.gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        var tempScreenVector = Mathf.Abs(Mathf.Max(_startMousePos.y - _mousePos.y, _startMousePos.x - _mousePos.x));
        if (tempScreenVector != 0) _totalScale = (1 + (tempScreenVector / _screenHeight));
        
        if (_totalScale >= 1.8)
        {
            activeBasket.BasketDown.transform.localScale = new Vector3(1, 1.8f, 1);
            activeBasket.SetBallPointPosY(-0.47f);
        }
        else
        {
            activeBasket.BasketDown.transform.localScale = new Vector3(1, _totalScale, 1);
            activeBasket.SetBallPointPosY(_totalScale/-4f);
        }
    }

        
    
    private async void OnMouseUpAction()
    {
        var activeBasket = _spawnManager.BasketPull[_spawnManager.ActiveBasket];
        var activeBasketCollider = _spawnManager.BasketPull[_spawnManager.ActiveBasket].BasketDown.GetComponent<PolygonCollider2D>();
        var height = Vector2.ClampMagnitude(new Vector2((_startMousePos.x - _mousePos.x), (_startMousePos.y - _mousePos.y)), _totalScale);
        
            
        _ball.BallRb.isKinematic = false;
        _ball.ToggleAttachBall(false);
        
        _totalForce =  _totalScale < 1.8 ? (_playerLaunchSpeed * _totalScale * height) : (_playerLaunchSpeed * 1.8f * height);
       
        _ball.BallRb.AddForce(_totalForce, ForceMode2D.Impulse);
        
        Sequence mySequence = DOTween.Sequence();
        mySequence
            .Append(activeBasket.BasketDown.transform.DOScale(Vector3.one, 0.05f));

        activeBasket.SetBallPointPosY(-0.2f);
        activeBasketCollider.enabled = false;
        activeBasket.OnClearBasket();
        
        _signalBus.Fire<ShotSignal>();

        await UniTask.Delay(100);
        
        activeBasketCollider.enabled = true;
        DisActiveControl();
    }

    private void DisActiveControl()
    {
        _isControlable = false;
    }
    
    private void ActiveControl()
    {
        _isControlable = true;
    }

    private void OnStart()
    {
        _ball.BallRb.isKinematic = false;
    }
    
    private void OnRestart()
    {
        _ball.BallRb.isKinematic = true;
    }
}
