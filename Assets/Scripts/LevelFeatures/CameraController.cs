using UnityEngine;
using Zenject;
using Zenject.Signals;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera gameCamera;
    [SerializeField] private CinemachineVirtualCamera finishCamera;

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }



    private void Awake()
    {
        _signalBus.Subscribe<FinishSignal>(OnFinish);
        _signalBus.Subscribe<GameRestartSignal>(OnRestart);
    }
    private void OnDestroy()
    {
        _signalBus.Unsubscribe<FinishSignal>(OnFinish);
        _signalBus.Unsubscribe<GameRestartSignal>(OnRestart);
    }

    private void OnFinish()
    {
        gameCamera.gameObject.SetActive(false);
        finishCamera.gameObject.SetActive(true);
    }

    private void OnRestart()
    {
        gameCamera.gameObject.SetActive(true);
        finishCamera.gameObject.SetActive(false);
    }
}
