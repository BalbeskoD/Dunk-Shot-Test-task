using System;
using UnityEngine;
using Zenject;
using Zenject.Signals;


namespace UI
{
    public class IngamePanel : MonoBehaviour
    {
        private SignalBus _signalBus;
        private PointCounter _pointCounter;
        private PauseButton _pauseButton;
        private UIController _uiController;


        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _signalBus.Subscribe<FinishSignal>(OnFinish);
            _uiController = GetComponentInParent<UIController>();
            _pointCounter = GetComponentInChildren<PointCounter>();
            _pauseButton = GetComponentInChildren<PauseButton>();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<FinishSignal>(OnFinish);
        }

        private void OnFinish()
        {
            _pauseButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _pointCounter.gameObject.SetActive(true);
            _pauseButton.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            _pointCounter.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(false);

        }
       
    }
}
