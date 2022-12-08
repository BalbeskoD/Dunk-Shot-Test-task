using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using DG.Tweening;

public class Star : MonoBehaviour
{ 
    private SignalBus _signalBus;
    private StarsCounter _starsCounter;
    private Vector3 starUiPosition;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Awake()
    {
        _starsCounter = FindObjectOfType<StarsCounter>().GetComponent<StarsCounter>();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveY(transform.position.y - 0.3f, 1f))
            .Append(transform.DOLocalMoveY(transform.position.y + 0.3f, 1f));
        
        sequence.AppendCallback(RestartTween);
        
        void RestartTween()
        {
            
                sequence.Restart();
        }
        
        
       
    }

    private void FixedUpdate()
    {
       starUiPosition = _starsCounter.StarUiPos;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Ball>())
        {
            col.gameObject.GetComponent<Ball>().StarFire();
            Destroy(gameObject);
            /*Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(StarCount);
            sequence.Append(transform.DOJump(starUiPosition, 0.5f, 1, 1f));

            void StarCount()
            {
                
                col.gameObject.GetComponent<Ball>().StarFire();
            }*/
        }
    }
}
