using UnityEngine;
using Zenject;
using Zenject.Signals;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
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
        button.onClick.AddListener(Restart);
    }


    private void Restart()
    {
        _signalBus.Fire<GameRestartSignal>();
    }
}
