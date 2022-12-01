using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SoundToggler : MonoBehaviour
{
    private bool value = true;

    private Animator _animator;
    private Ball _ball;

    private static readonly int Value = Animator.StringToHash("Value");

    [Inject]
    public void Construct(Ball ball)
    {
        _ball = ball;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool(Value, value);

    }

    private void OnEnable()
    {
        _animator.SetBool(Value, value);
    }

    public void Toggle()
    {
        this.value = !this.value;

        _animator.SetBool(Value, value);
        _ball.gameObject.GetComponent<AudioListener>().enabled = value;
        
    }
}
