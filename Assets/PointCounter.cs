using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using TMPro;

public class PointCounter : MonoBehaviour
{
    private TextMeshProUGUI pointCounterText;
    private int pointCounter;
    private SignalBus _signalBus;

    public int PointCount => pointCounter;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    private void Awake()
    {
        pointCounterText = GetComponent<TextMeshProUGUI>();
        _signalBus.Subscribe<ClearGoalSignal>(OnClearGoal);
        _signalBus.Subscribe<GoalSignal>(OnGoal);
        _signalBus.Subscribe<GameRestartSignal>(OnRestart);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<ClearGoalSignal>(OnClearGoal);
        _signalBus.Unsubscribe<GoalSignal>(OnGoal);
        _signalBus.Unsubscribe<GameRestartSignal>(OnRestart);
    }

    private void OnClearGoal(ClearGoalSignal signal)
    {
        pointCounter += (signal.clearInRow + 1);
        PointCountUpdate();
    }

    private void OnGoal()
    {
        pointCounter++;
        PointCountUpdate();
    }

    private void OnRestart()
    {
        pointCounter = 0;
        PointCountUpdate();
    }

    private void PointCountUpdate()
    {
        pointCounterText.text = pointCounter.ToString();
    }
}

