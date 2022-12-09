using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] private GameObject basketDown;
    [SerializeField] private GameObject ballPoint;
    public GameObject BallPoint => ballPoint;
    public GameObject BasketDown => basketDown;
    public bool IsClear { get; private set; } = true;

    public void SetBallPointPosY(float yPos)
    {
        ballPoint.transform.localPosition = new Vector3(ballPoint.transform.localPosition.x, yPos, ballPoint.transform.position.z);
    }
    
    public void OffClearBasket()
    {
            IsClear = false;
    }

    public void OnClearBasket()
    {
        IsClear = true;
    }
}
