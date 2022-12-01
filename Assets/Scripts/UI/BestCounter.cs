using UnityEngine;
using Zenject;
using Zenject.Signals;
using TMPro;

public class BestCounter : MonoBehaviour
{
    private SignalBus _signalBus;
    private TextMeshProUGUI bestText;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    private void Awake()
    {
        _signalBus.Subscribe<BestResultSignal>(OnBestResult);
        bestText = GetComponent<TextMeshProUGUI>();
    }
    private void OnDestroy()
    {
        _signalBus.Unsubscribe<BestResultSignal>(OnBestResult);
    }

    private void OnBestResult(BestResultSignal signal)
    {
        bestText.text = signal._bestResult.ToString();
    }
}
