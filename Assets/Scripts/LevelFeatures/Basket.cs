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
    private bool isClear = true;
    private SpawnManager _spawnManager;
    public GameObject BallPoint => ballPoint;
    public GameObject BasketDown => basketDown;

    [Inject]
    public void Construct(SignalBus signalBus, SpawnManager spawnManager)
    {
        _signalBus = signalBus;
        _spawnManager = spawnManager;
    }

  

    public void SetBallPointPosY(float yPos)
    {
        ballPoint.transform.localPosition = new Vector3(ballPoint.transform.localPosition.x, yPos, ballPoint.transform.position.z);
    }
    public void DisactivateClearBasket()
    {
            isClear = false;
    }

    public void ActivateClearBasket()
    {
        isClear = true;
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ballColl = collision.gameObject.GetComponent<Ball>();
        if (_spawnManager.BasketPull[_spawnManager.ActiveBasket].gameObject != gameObject)
        {
            if (ballColl && isClear)
            {
                _spawnManager.ChangeRowValue(1);
                _signalBus.Fire(new ClearGoalSignal() { clearInRow = _spawnManager.ClearInRow});
                OnGoalAction();
            }
            else if (ballColl && !isClear)
            {
                _spawnManager.ChangeRowValue(0);
                _signalBus.Fire<GoalSignal>();
                OnGoalAction();
                
            }
        }
        else if(_spawnManager.BasketPull[_spawnManager.ActiveBasket].gameObject == gameObject)
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
