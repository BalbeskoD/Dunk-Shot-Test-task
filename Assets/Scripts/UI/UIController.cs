using Enums;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        private MenuPanel _menuPanel;
        private IngamePanel _ingamePanel;
        private FinishPanel _finishPanel;
        private SettingsPanel _settingsPanel;
        private SignalBus _signalBus;
        private PausePanel _pausePanel;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _menuPanel = GetComponentInChildren<MenuPanel>(true);
            _ingamePanel = GetComponentInChildren<IngamePanel>(true);
            _finishPanel = GetComponentInChildren<FinishPanel>(true);
            _settingsPanel = GetComponentInChildren<SettingsPanel>(true);
            _pausePanel = GetComponentInChildren<PausePanel>(true);
            SubscribeSignals();
        }

        private void OnDestroy()
        {
            UnsubscribeSignals();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<GameStateChangeSignal>(OnGameStateChange);
        }

        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<GameStateChangeSignal>(OnGameStateChange);
        }

        private void OnGameStateChange(GameStateChangeSignal signal)
        {
            if(signal.GameStates == GameStates.Menu)
            {
                _menuPanel.gameObject.SetActive(true);
                _ingamePanel.gameObject.SetActive(false);
                _pausePanel.gameObject.SetActive(false);
                _finishPanel.gameObject.SetActive(false);

            } 
            else if(signal.GameStates == GameStates.Game)
            {
                _menuPanel.gameObject.SetActive(false);
                _ingamePanel.gameObject.SetActive(true);
                _pausePanel.gameObject.SetActive(false);
            } 
            else if (signal.GameStates == GameStates.Lose)
            {
                _menuPanel.gameObject.SetActive(false);
                _finishPanel.gameObject.SetActive(true);
            } 
            else if (signal.GameStates == GameStates.Pause)
            {
                _pausePanel.gameObject.SetActive(true);
            }
            
        }
    }
}