using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;

public class Basket : MonoBehaviour
{
    [SerializeField] private GameObject basketDown;
    [SerializeField] private GameObject ballPoint;
    private bool isClear = true;
    public GameObject BallPoint => ballPoint;
    public GameObject BasketDown => basketDown;
    public bool IsClear => isClear;


  

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
   

}
