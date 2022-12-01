using UnityEngine;
using Zenject;
using Zenject.Signals;

public class BasketCollisionDetector : MonoBehaviour
{
    [SerializeField] private GameObject basketDown;
    private SignalBus _signalBus;
    private SpawnManager _spawnManager;
    private Basket _basket;
    private static readonly string basketTopTag = "BasketTop";
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
        if (_spawnManager.BasketPull[_spawnManager.ActiveBasket].gameObject != _basket.gameObject)
        {
            if (ballColl && _basket.IsClear)
            {
                _spawnManager.ChangeRowValue(1);
                _signalBus.Fire(new ClearGoalSignal() { clearInRow = _spawnManager.ClearInRow});
                OnGoalAction();
            }
            else if (ballColl && !_basket.IsClear)
            {
                _spawnManager.ChangeRowValue(0);
                _signalBus.Fire<GoalSignal>();
                OnGoalAction();
                
            }
        }
        else if(_spawnManager.BasketPull[_spawnManager.ActiveBasket].gameObject == _basket.gameObject)
        {
            if (ballColl)
            {
                _spawnManager.ChangeRowValue(0);
                _signalBus.Fire<BallReturnSignal>();
                OnGoalAction();
            }
            
        }
        

        void OnGoalAction()
        {

            ballColl.ToggleAttachBall(true);
            basketDown.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var ballColl = collision.gameObject.GetComponent<PlayerController>();
        if (ballColl)
        {
           // ballColl.DisActiveControl();
        }
    }


}
