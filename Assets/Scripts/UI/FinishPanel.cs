using System;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace UI
{
    public class FinishPanel : MonoBehaviour
     {
        private SignalBus _signalBus;
        private BestCounter _bestCounter;
        private RestartButton _restartButton;
        private SettingButton _settingsButton;


        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _bestCounter = GetComponentInChildren<BestCounter>();
            _restartButton = GetComponentInChildren<RestartButton>();
            _settingsButton = GetComponentInChildren<SettingButton>();
        }

        private void OnDestroy()
        {
        }


        private void OnEnable()
        {
            _bestCounter.gameObject.SetActive(true);
            _restartButton.gameObject.SetActive(true);
            _settingsButton.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            _bestCounter.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(false);
            _settingsButton.gameObject.SetActive(false);
        }
    }
}
