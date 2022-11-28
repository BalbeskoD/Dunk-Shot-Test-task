using Enums;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace UI
{
    public class UiController : MonoBehaviour
    {
        private MenuPanel _menuPanel;
        private IngamePanel _ingamePanel;
        private LosePanel _losePanel;
        private SettingsPanel _settingsPanel;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _menuPanel = GetComponentInChildren<MenuPanel>(true);
            _ingamePanel = GetComponentInChildren<IngamePanel>(true);
            _losePanel = GetComponentInChildren<LosePanel>(true);
            _settingsPanel = GetComponentInChildren<SettingsPanel>(true);
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
            _menuPanel.gameObject.SetActive(signal.GameStates == GameStates.Menu);
            _ingamePanel.gameObject.SetActive(signal.GameStates == GameStates.Game);
            _losePanel.gameObject.SetActive(signal.GameStates == GameStates.Lose);
            _settingsPanel.gameObject.SetActive(signal.GameStates == GameStates.Settings);
        }
    }
}