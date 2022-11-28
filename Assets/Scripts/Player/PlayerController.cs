using UnityEngine;
using DG.Tweening;
using Zenject;
using Zenject.Signals;

public class PlayerController : MonoBehaviour
{
    private Ball _ball;
    private SpawnManager _spawnManager;
    private Vector2 mousePos;
    private Vector2 startMousePos;
    private float screenHeight;
    private float totalScale;
    private PolygonCollider2D basketCollider;
    private float maxLength = 500;
    private Vector2 totalForce;
    private bool isControlable = true;
    private SignalBus _signalBus;

    public Vector2 MousePos =>mousePos;
    public Vector2 StartMousePos => startMousePos;
    public float TotalScale =>totalScale;
    public Vector2 TotalForce => totalForce;
    public bool IsControlable => isControlable;

    [Inject]
    public void Constructor(Ball ball, SpawnManager spawnManager, SignalBus signalBus)
    {
        _ball = ball;
        _spawnManager = spawnManager;
        _signalBus = signalBus;
    }

    private void Awake()
    {
        //basketCollider = _spawnManager.BasketPull[_spawnManager.ActiveBasket].GetComponent<PolygonCollider2D>();
        screenHeight = Screen.height/3;
        _signalBus.Subscribe<GoalSignal>(ActiveControl);
        _signalBus.Subscribe<ClearGoalSignal>(ActiveControl);
        _signalBus.Subscribe<BallReturnSignal>(ActiveControl);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<GoalSignal>(ActiveControl);
        _signalBus.Unsubscribe<ClearGoalSignal>(ActiveControl);
        _signalBus.Unsubscribe<BallReturnSignal>(ActiveControl);
    }

    private void Update()
    {
        if (isControlable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                UpdateMousePosition();
                startMousePos = mousePos;
                //_ball.BallRb.isKinematic = true;
            }
            else if (Input.GetMouseButton(0))
            {
                OnMouseAction();
            }

            else if (Input.GetMouseButtonUp(0))
            {
                OnMouseUpAction();
            }
            if (_ball.IsAttached)
            {
                _ball.BallRb.transform.position = _spawnManager.BasketPull[_spawnManager.ActiveBasket].BallPoint.transform.position;
                _ball.BallRb.rotation = 0;
            }
        }
        

    }


    private void UpdateMousePosition()
    {
        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    private void OnMouseAction()
    {
        

        UpdateMousePosition();

        var tempAngle = Mathf.Atan2(startMousePos.y - mousePos.y, startMousePos.x - mousePos.x) * Mathf.Rad2Deg;
        if (tempAngle != 0)
        {
            float angle = tempAngle - 90;
            _spawnManager.BasketPull[_spawnManager.ActiveBasket].gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        }


        var tempScreenVector = Mathf.Abs(Mathf.Max(startMousePos.y - mousePos.y, startMousePos.x - mousePos.x));
        if (tempScreenVector != 0)
        {

            totalScale = (1 + (tempScreenVector / screenHeight));
            Debug.Log(totalScale);
            if (totalScale >= 1.8)
            {
                _spawnManager.BasketPull[_spawnManager.ActiveBasket].BasketDown.transform.localScale = new Vector3(1, 1.8f, 1);
                _spawnManager.BasketPull[_spawnManager.ActiveBasket].SetBallPointPosY(-0.47f);
            }
            else
            {
                _spawnManager.BasketPull[_spawnManager.ActiveBasket].BasketDown.transform.localScale = new Vector3(1, totalScale, 1);
                _spawnManager.BasketPull[_spawnManager.ActiveBasket].SetBallPointPosY(totalScale/-4f);
            }
        }

        
    }
    private void OnMouseUpAction()
    {

        _spawnManager.BasketPull[_spawnManager.ActiveBasket].BasketDown.GetComponent<PolygonCollider2D>().enabled = false;


        Sequence mySequence = DOTween.Sequence();
        mySequence
            .Append(_spawnManager.BasketPull[_spawnManager.ActiveBasket].BasketDown.transform.DOScale(Vector3.one, 0.05f));

        _spawnManager.BasketPull[_spawnManager.ActiveBasket].SetBallPointPosY(-0.2f);


        var height = Vector2.ClampMagnitude(new Vector2((startMousePos.x - mousePos.x), (startMousePos.y - mousePos.y)), maxLength);
        //var startSpeed = Mathf.Sqrt(height * (-2f*9.8f));
        if(totalScale <= 1.4)
        {
            return;
        }
        else if (totalScale >= 1.8)
        {
            _ball.BallRb.isKinematic = false;
            _ball.ToggleAttachBall(false);

            totalForce = height * 1.8f * Time.deltaTime / 4.5f;
            _ball.BallRb.AddForce(totalForce, ForceMode2D.Impulse);
            Debug.Log(height);
        }
        else
        {
            _ball.BallRb.isKinematic = false;
            _ball.ToggleAttachBall(false);

            totalForce = height * totalScale * Time.deltaTime / 4.5f;
            _ball.BallRb.AddForce(totalForce, ForceMode2D.Impulse);
            //_ball.BallRb.AddForceAtPosition(totalForce, _ball.transform.position - new Vector3(0,0.25f,0));
            Debug.Log(height);
        }
        isControlable = false;
        _spawnManager.BasketPull[_spawnManager.ActiveBasket].BasketDown.GetComponent<PolygonCollider2D>().enabled = false;
        _spawnManager.BasketPull[_spawnManager.ActiveBasket].ActivateClearBasket();

    }

    private void ActiveControl()
    {
        isControlable = true;
    }
}
