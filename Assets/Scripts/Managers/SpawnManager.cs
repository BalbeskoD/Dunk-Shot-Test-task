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
    private int activeBasket;
    private int notActiveBasket = 1;
    private SignalBus _signalBus;
    

    public List<Basket> BasketPull => basketPull;
    public int ActiveBasket => activeBasket;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Awake()
    {
        _signalBus.Subscribe<GoalSignal>(OnGoal);
        _signalBus.Subscribe<ClearGoalSignal>(OnClearGoal);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<GoalSignal>(OnGoal);
        _signalBus.Unsubscribe<ClearGoalSignal>(OnClearGoal);
    }

    public void SpawnBall()
    {
        ball.transform.position = new Vector2(basketPull[activeBasket].transform.position.x, basketPull[activeBasket].transform.position.y + ballOffset.y);
        ball.gameObject.SetActive(true);
    }

    private async void OnGoal()
    {
        SwitchBasket();
        SetUpActiveBasket();
        await UniTask.Delay(200);
        SpawnNewBasket();
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
        float offsetY = Random.Range(3, 6);

        int random = Random.Range(1, 2);
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
            offsetX = Random.Range(-2f, 0);
        }
        else {
            offsetX = Random.Range(0, 2f);
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



    
}
