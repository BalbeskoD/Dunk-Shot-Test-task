using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Vector2 ballOffset;
    [SerializeField] private GameObject star;
    [SerializeField] private List<Basket> basketPull;
    [SerializeField] private float basketAngleOffset = 0.3f;
    [SerializeField] private Vector2 basket1SpawnPlace;
    [SerializeField] private Vector2 basket2SpawnPlace;
    [SerializeField] private Vector3 starOffset = new Vector3(0, 0.5f, 0);
    private SignalBus _signalBus;
    private Basket _ballActiveBasket;
    private Basket _ballNotActiveBasket;
    private Ball _ball;
    public List<Basket> BasketPull => basketPull;
    public int ActiveBasket { get; private set; }
    public int NotActiveBasket { get; private set; } = 1;

    public int ClearInRow { get; private set; }

    [Inject]
    public void Construct(SignalBus signalBus, Ball ball)
    {
        _signalBus = signalBus;
        _ball = ball;
    }

    private void Awake()
    {
        _ballActiveBasket = basketPull[ActiveBasket];
        _ballNotActiveBasket= basketPull[NotActiveBasket];
        _signalBus.Subscribe<GoalSignal>(OnGoal);
        _signalBus.Subscribe<ClearGoalSignal>(OnClearGoal);
        _signalBus.Subscribe<GameRestartSignal>(OnRestart);
        _signalBus.Subscribe<GameStartSignal>(OnStart);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<GoalSignal>(OnGoal);
        _signalBus.Unsubscribe<ClearGoalSignal>(OnClearGoal);
        _signalBus.Unsubscribe<GameRestartSignal>(OnRestart);
        _signalBus.Unsubscribe<GameStartSignal>(OnStart);
    }

    public void SpawnBall()
    {
        _ball.BallRb.transform.position = new Vector2(_ballActiveBasket.transform.position.x, _ballActiveBasket.transform.position.y + ballOffset.y);
        _ball.GetComponent<Ball>().BallRb.velocity = Vector2.zero;
        _ball.gameObject.SetActive(true);
       _ballActiveBasket.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void SpawnStar()
    {
        int rand = Random.Range(0, 5);
        if (rand == 0)
        {
            Instantiate(star, _ballNotActiveBasket.transform.position + starOffset, new Quaternion(0, 0, 0, 0));
        }
    }

    private async void OnGoal()
    {
        SwitchBasket();
        SetUpActiveBasket();
        await UniTask.Delay(200);
        SpawnNewBasket();
        SpawnStar();
    }
    
    private void OnStart()
    {
        Time.timeScale = 1;
    }
    
    private void OnRestart()
    {
        ActiveBasket=0;
        NotActiveBasket=1;
        var trans = _ballActiveBasket.gameObject.transform;
        var trans2 =  _ballNotActiveBasket.gameObject.transform;
        trans.position = basket1SpawnPlace;
        trans.rotation = new Quaternion(0, 0, 0, 0);
        trans2.position = basket2SpawnPlace;
        trans2.rotation = new Quaternion(0, 0, 0, 0);
        SpawnBall();
    }

    private async void OnClearGoal()
    {
        SwitchBasket();
        SetUpActiveBasket();
        await UniTask.Delay(200);
        SpawnNewBasket();
        SpawnStar();
    }

    private void SwitchBasket()
    {
        if (ActiveBasket == 0)
        {
            ActiveBasket++;
            NotActiveBasket--;
        }
        else
        {
            ActiveBasket--;
            NotActiveBasket++;
        }
    }

    private void SpawnNewBasket()
    {
        var notActiveBasket = _ballNotActiveBasket.gameObject.transform;
        _ballNotActiveBasket.gameObject.SetActive(false);
        
        var offsetY = Random.Range(4f, 5f);
        var offsetX = NotActiveBasket == 0 ? Random.Range(-1.6f, -0.3f): Random.Range(0.3f, 1.6f);
        var random = Random.Range(0, 2);
        
        float rotateZ;
        switch (random)
        {
            case 0:
                rotateZ = 0;
                break;
            case 1:
                rotateZ = NotActiveBasket == 0 ? basketAngleOffset : -basketAngleOffset;
                break;
            default:
                rotateZ = 0;
                break;
        }
        
        notActiveBasket.position = new Vector3(offsetX, notActiveBasket.position.y + offsetY, notActiveBasket.position.z);
        notActiveBasket.rotation = new Quaternion(0, 0, 0, 0);
        notActiveBasket.Rotate(Vector3.back, rotateZ);
        _ballNotActiveBasket.gameObject.SetActive(true);
    }

    private void SetUpActiveBasket()
    {
        var _activeBasket = _ballActiveBasket.gameObject;
        var sequence = DOTween.Sequence();
        sequence
            .Append(_activeBasket.transform.DORotate(Vector3.zero, 0f))
            .Append(_activeBasket.GetComponent<Basket>().BasketDown.transform.DOScaleY(1.3f, 0.2f))
            .Insert(0, _activeBasket.GetComponent<Basket>().BallPoint.transform.DOLocalMoveY(-0.3f, 0.2f))
            .Append(_activeBasket.GetComponent<Basket>().BasketDown.transform.DOScaleY(1f, 0.2f))
            .Insert(0.2f, _activeBasket.GetComponent<Basket>().BallPoint.transform.DOLocalMoveY(-0.2f, 0.2f));
    }

    public void ChangeRowValue(int value)
    {
        ClearInRow = value == 0 ? 0 : ClearInRow += value;
    }
}
