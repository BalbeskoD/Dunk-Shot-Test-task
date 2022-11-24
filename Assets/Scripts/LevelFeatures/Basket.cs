using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;

public class Basket : MonoBehaviour
{
    [SerializeField] private GameObject basketDown;
    [SerializeField] private GameObject ballPoint;
    private SignalBus _signalBus;
    private bool isClear;
    private SpawnManager _spawnManager;

    public GameObject BallPoint => ballPoint;
    public GameObject BasketDown => basketDown;

    [Inject]
    public void Construct(SignalBus signalBus, SpawnManager spawnManager)
    {
        _signalBus = signalBus;
        _spawnManager = spawnManager;
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

    private void OnGoal()
    {

    }

    private void OnClearGoal()
    {

    }

    public void SetBallPointPosY(float yPos)
    {
        ballPoint.transform.localPosition = new Vector3(ballPoint.transform.localPosition.x, yPos, ballPoint.transform.position.z);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
        {
            isClear = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ballColl = collision.gameObject.GetComponent<Ball>();
        if (_spawnManager.BasketPull[_spawnManager.ActiveBasket].gameObject != gameObject)
        {
            if (ballColl && isClear)
            {
                _signalBus.Fire<ClearGoalSignal>();
                ballColl.ToggleAttachBall(true);
            }
            else if (ballColl && !isClear)
            {
                _signalBus.Fire<GoalSignal>();
                ballColl.ToggleAttachBall(true);
            }
        }
        else if(_spawnManager.BasketPull[_spawnManager.ActiveBasket].gameObject == gameObject)
        {
            if (ballColl)
            {
                _signalBus.Fire<BallReturnSignal>();
                ballColl.ToggleAttachBall(true);
            }
            
        }
        
    }
}
