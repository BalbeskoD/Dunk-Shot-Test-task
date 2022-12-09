using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour
{ 
    private void Awake()
    {
        var y = transform.position.y;
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveY( y - 0.3f, 1f))
            .Append(transform.DOLocalMoveY(y + 0.3f, 1f));
        
        sequence.AppendCallback(RestartTween);
        
        void RestartTween()
        {
                sequence.Restart();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        var ball = col.gameObject.GetComponent<Ball>();
        if (!ball) return;
       
        col.gameObject.GetComponent<Ball>().StarFire();
        Destroy(gameObject);
    }
}
