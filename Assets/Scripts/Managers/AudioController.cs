using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip clearGoalAudio;
    [SerializeField] private AudioClip shotAudio;
    [SerializeField] private AudioClip goalAudio;
    [SerializeField] private AudioClip knokAudio;
    private AudioSource _audioSource;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _signalBus.Subscribe<GoalSignal>(OnGoal);
        _signalBus.Subscribe<ClearGoalSignal>(OnClearGoal);
        _signalBus.Subscribe<ShotSignal>(OnShot);
    }

    private void OnGoal()
    {
        _audioSource.PlayOneShot(goalAudio, 1.0f);
    }
    private void OnClearGoal()
    {

        _audioSource.PlayOneShot(clearGoalAudio, 1.0f);
    }
    private void OnShot()
    {

        _audioSource.PlayOneShot(shotAudio, 1.0f);
    }

    public void PlayKnokAudio()
    {
        _audioSource.PlayOneShot(knokAudio, 1.0f);
    }
}
