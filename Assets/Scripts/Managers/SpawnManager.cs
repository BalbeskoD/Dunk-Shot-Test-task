using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private Vector2 ballOffset;
    [SerializeField] private List<Basket> basketPull;
    [SerializeField] private float basketAngleOffset = 0.3f;
    [SerializeField] private Vector2 basket1SpawnPlace;
    [SerializeField] private Vector2 basket2SpawnPlace;
    private int activeBasket;
    private int notActiveBasket = 1;
    private int _clearInRow;
    private SignalBus _signalBus;
    

    public List<Basket> BasketPull => basketPull;
    public int ActiveBasket => activeBasket;

    public int ClearInRow => _clearInRow;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Awake()
    {
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
        ball.GetComponent<Ball>().BallRb.transform.position = new Vector2(basketPull[activeBasket].transform.position.x, basketPull[activeBasket].transform.position.y + ballOffset.y);
        ball.GetComponent<Ball>().BallRb.velocity = Vector2.zero;
        ball.gameObject.SetActive(true);
        basketPull[activeBasket].transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private async void OnGoal()
    {
        SwitchBasket();
        SetUpActiveBasket();
        await UniTask.Delay(200);
        SpawnNewBasket();
    }
    private void OnStart()
    {
        Time.timeScale = 1;
    }
    private void OnRestart()
    {
        activeBasket=0;
        notActiveBasket=1;
        basketPull[activeBasket].gameObject.transform.position = basket1SpawnPlace;
        basketPull[activeBasket].gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        basketPull[notActiveBasket].gameObject.transform.position = basket2SpawnPlace;
        basketPull[notActiveBasket].gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        SpawnBall();
    }


    private async void OnClearGoal()
    {
        SwitchBasket();
        SetUpActiveBasket();
        await UniTask.Delay(200);
        SpawnNewBasket();

    }

    private void SwitchBasket()
    {
        if (activeBasket == 0)
        {
            activeBasket++;
            notActiveBasket--;
        }
        else
        {
            activeBasket--;
            notActiveBasket++;
        }
    }

    private void SpawnNewBasket()
    {
        var _notActiveBasket = basketPull[notActiveBasket].gameObject;
        _notActiveBasket.SetActive(false);
        float offsetX;
        float rotateZ;
        float offsetY = Random.Range(4f, 5f);

        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                rotateZ = 0;
                break;
            case 1:
                rotateZ = notActiveBasket == 0 ? basketAngleOffset : -basketAngleOffset;
                break;
            default:
                rotateZ = 0;
                break;
        }


        if (notActiveBasket == 0)
        {
            offsetX = Random.Range(-1.6f, -0.3f);
        }
        else {
            offsetX = Random.Range(0.3f, 1.6f);
        }


        _notActiveBasket.transform.position = new Vector3(offsetX, _notActiveBasket.transform.position.y + offsetY, _notActiveBasket.transform.position.z);
        _notActiveBasket.transform.rotation = new Quaternion(0, 0, 0, 0);
        _notActiveBasket.transform.Rotate(Vector3.back, rotateZ);
        _notActiveBasket.SetActive(true);
    }

    private void SetUpActiveBasket()
    {
        var _activeBasket = basketPull[activeBasket].gameObject;
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(_activeBasket.transform.DORotate(Vector3.zero, 0f))
            .Append(_activeBasket.GetComponent<Basket>().BasketDown.transform.DOScaleY(1.3f, 0.2f))
            .Insert(0, _activeBasket.GetComponent<Basket>().BallPoint.transform.DOLocalMoveY(-0.3f, 0.2f))
            .Append(_activeBasket.GetComponent<Basket>().BasketDown.transform.DOScaleY(1f, 0.2f))
            .Insert(0.2f, _activeBasket.GetComponent<Basket>().BallPoint.transform.DOLocalMoveY(-0.2f, 0.2f));



    }


    public void ChangeRowValue(int value)
    {   
        if(value == 0)
        {
            _clearInRow = 0;
            return;
        }
        _clearInRow += value;
    }


    
}
