using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using DG.Tweening;

public class Star : MonoBehaviour
{ 
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Awake()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveY(transform.position.y - 0.3f, 1f))
            .Append(transform.DOLocalMoveY(transform.position.y + 0.3f, 1f));
        
        sequence.AppendCallback(RestartTween);
        
        void RestartTween()
        {
            
                sequence.Restart();
        }
        
        
       
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Ball>())
        {
            col.gameObject.GetComponent<Ball>().StarFire();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOJump())
        }
    }
}
