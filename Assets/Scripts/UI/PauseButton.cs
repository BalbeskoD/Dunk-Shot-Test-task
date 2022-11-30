using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private SignalBus _signalBus;
    private Button button;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PauseGame);
    }

    private void PauseGame()
    {
        _signalBus.Fire<PauseSignal>();
    }
}
