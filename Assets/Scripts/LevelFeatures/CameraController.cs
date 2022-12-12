using UnityEngine;
using Zenject;
using Zenject.Signals;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject gameCamera;
    [SerializeField] private GameObject finishCamera;
    private CinemachineFramingTransposer _finishTrasposer;
    private CinemachineFramingTransposer _gameTrasposer;
    
    private SignalBus _signalBus;
    private SpawnManager _spawnManager;

    [Inject]
    public void Construct(SignalBus signalBus, SpawnManager spawnManager)
    {
        _signalBus = signalBus;
        _spawnManager = spawnManager;
    }

    private void Awake()
    {
        _finishTrasposer = finishCamera.GetComponentInChildren<CinemachineFramingTransposer>();
        _gameTrasposer = gameCamera.GetComponentInChildren<CinemachineFramingTransposer>();
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
        finishCamera.GetComponentInChildren<CinemachineVirtualCamera>().Follow = _spawnManager.BasketPull[_spawnManager.ActiveBasket].transform;
        _finishTrasposer.m_ScreenX = _spawnManager.ActiveBasket == 0 ? 0.25f : 0.67f;
        gameCamera.gameObject.SetActive(false);
        finishCamera.gameObject.SetActive(true);
    }

    private void OnRestart()
    {
        _gameTrasposer.m_ScreenX = _spawnManager.ActiveBasket == 0 ? 0.25f : 0.67f;
        gameCamera.gameObject.SetActive(true);
        finishCamera.gameObject.SetActive(false);
    }
}
