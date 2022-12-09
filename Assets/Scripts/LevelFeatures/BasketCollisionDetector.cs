using System;
using UnityEngine;
using Zenject;
using Zenject.Signals;

public class BasketCollisionDetector : MonoBehaviour
{
    [SerializeField] private GameObject basketDown;
    private SignalBus _signalBus;
    private SpawnManager _spawnManager;
    private Basket _basket;
    private const string GoalSignal = "GoalSignal";
    private const string ClearGoalSignal = "ClearGoalSignal";
    private const string BallReturnSignal = "BallReturnSignal";
    public GameObject BasketDown => basketDown;

    [Inject]
    public void Construct(SignalBus signalBus, SpawnManager spawnManager)
    {
        _signalBus = signalBus;
        _spawnManager = spawnManager;
    }

    private void Awake()
    {
        _basket = GetComponentInParent<Basket>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ballColl = collision.gameObject.GetComponent<Ball>();
        
        if (!ballColl) return;
        
        if (_spawnManager.BasketPull[_spawnManager.ActiveBasket].gameObject != _basket.gameObject)
        {
            if (_basket.IsClear)
                OnGoalAction(1, ClearGoalSignal);
            
            else  
                OnGoalAction(0, GoalSignal);

            
        }
        
        else if (_spawnManager.BasketPull[_spawnManager.ActiveBasket].gameObject == _basket.gameObject)
            OnGoalAction(0, BallReturnSignal);
        
        
        void OnGoalAction(int row, string signalClass)
        {
            
            _spawnManager.ChangeRowValue(row);
            switch (signalClass)
            {
                case GoalSignal:
                    _signalBus.Fire<GoalSignal>();
                    break;
                
                case ClearGoalSignal:
                    _signalBus.Fire(new ClearGoalSignal(){clearInRow = _spawnManager.ClearInRow});
                    break;
                    
                case BallReturnSignal:
                    _signalBus.Fire<BallReturnSignal>();
                    break;
            }
            ballColl.ToggleAttachBall(true);
            basketDown.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }
}
