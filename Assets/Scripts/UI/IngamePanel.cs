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

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _signalBus.Subscribe<FinishSignal>(OnFinish);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<FinishSignal>(OnFinish);
        }

        private void OnFinish()
        {
           
        }

        private void OnEnable()
        {
        }
    }
}
