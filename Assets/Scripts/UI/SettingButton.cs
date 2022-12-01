using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Zenject.Signals;

public class SettingButton : MonoBehaviour
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
        button.onClick.AddListener(OpenSettings);
    }

    private void OpenSettings()
    {
        _signalBus.Fire<SettingsSignal>();
    }
}
