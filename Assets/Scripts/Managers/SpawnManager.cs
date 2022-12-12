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
    private Ball _ball;
    public List<Basket> BasketPull => basketPull;
    public int ActiveBasket { get; private set; }
    public int NotActiveBasket { get; private set; } = 1;
    public bool IsColided { get; private set; } 

    public int ClearInRow { get; private set; }

    [Inject]
    public void Construct(SignalBus signalBus, Ball ball)
    {
        _signalBus = signalBus;
        _ball = ball;
    }

    private void Awake()
    {
        _signalBus.Subscribe<GoalSignal>(OnGoal);
        _signalBus.Subscribe<ClearGoalSignal>(OnClearGoal);
        _signalBus.Subscribe<GameRestartSignal>(OnRestart);
        _signalBus.Subscribe<GameStartSignal>(OnStart);
        _signalBus.Subscribe<ShotSignal>(OnShot);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<GoalSignal>(OnGoal);
        _signalBus.Unsubscribe<ClearGoalSignal>(OnClearGoal);
        _signalBus.Unsubscribe<GameRestartSignal>(OnRestart);
        _signalBus.Unsubscribe<GameStartSignal>(OnStart);
        _signalBus.Unsubscribe<ShotSignal>(OnShot);
    }

    public void SpawnBall()
    {
        _ball.BallRb.transform.position = new Vector2(basketPull[ActiveBasket].transform.position.x, basketPull[ActiveBasket].transform.position.y + ballOffset.y);
        _ball.GetComponent<Ball>().BallRb.velocity = Vector2.zero;
        _ball.gameObject.SetActive(true);
        basketPull[ActiveBasket].transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void SpawnStar()
    {
        int rand = Random.Range(0, 5);
        if (rand == 0)
        {
            Instantiate(star, basketPull[NotActiveBasket].transform.position + starOffset, new Quaternion(0, 0, 0, 0));
        }
    }

    private void OnShot()
    {
        IsColided = false;
    }
    private async void OnGoal()
    {
        //IsColided = true;
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
        var trans = basketPull[ActiveBasket].gameObject.transform;
        var trans2 =  basketPull[NotActiveBasket].gameObject.transform;
        trans.position = basket1SpawnPlace;
        trans.rotation = new Quaternion(0, 0, 0, 0);
        trans2.position = basket2SpawnPlace;
        trans2.rotation = new Quaternion(0, 0, 0, 0);
        SpawnBall();
    }

    private async void OnClearGoal()
    {
        //IsColided = true;
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
        var notActiveBasket = basketPull[NotActiveBasket].gameObject.transform;
        basketPull[NotActiveBasket].gameObject.SetActive(false);
        
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
        basketPull[NotActiveBasket].gameObject.SetActive(true);
    }

    private void SetUpActiveBasket()
    {
        var _activeBasket = basketPull[ActiveBasket].gameObject;
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
