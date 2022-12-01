using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Zenject.Signals;

public class MenuButton : MonoBehaviour
{
    private Button button;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenMenu);
    }

    private void OpenMenu()
    {
        _signalBus.Fire<GameRestartSignal>();
    }
}
