using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToggler : MonoBehaviour
{
    private bool value;

    private Animator _animator;

    private static readonly int Value = Animator.StringToHash("Value");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool(Value, value);
    }

    public void Toggle()
    {
        this.value = !this.value;

        _animator.SetBool(Value, value);
    }
}
