using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enums;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using Configs;

namespace Managers
{
    public class GameManager : IInitializable, IDisposable
    {

        private readonly DiContainer _diContainer;
        private readonly ProjectSettings _projectSettings;
        private readonly SignalBus _signalBus;

        private GameStates _gameStates;

        private GameStates GameState
        {
            get => _gameStates;
            set
            {
                if (_gameStates == value)
                    return;
                _gameStates = value;
                _signalBus.Fire(new GameStateChangeSignal() { GameStates = _gameStates });
                switch (_gameStates)
                {
                    case GameStates.Menu:
                        break;

                    case GameStates.Game:
                        break;

                    case GameStates.Lose:
                        break;

                    case GameStates.Pause:
                        break;


                }
            }
        }

        

        public GameManager(DiContainer diContainer, ProjectSettings projectSettings, SignalBus signalBus)
        {
            _diContainer = diContainer;
            _projectSettings = projectSettings;
            _signalBus = signalBus;
        }

        public async void Initialize()
        {
            SubscribeSignals();
            await UniTask.Yield();
            ChangeGameState(GameStates.Menu);
        }

        public void Dispose()
        {
            UnsubscribeSignals();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<GameStartSignal>(OnGameStart);
            _signalBus.Subscribe<FinishSignal>(OnFail);
            _signalBus.Subscribe<GameRestartSignal>(OnGameRestart);
            _signalBus.Subscribe<PauseSignal>(OnGamePause);
        }

        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<GameStartSignal>(OnGameStart);
            _signalBus.Unsubscribe<FinishSignal>(OnFail);
            _signalBus.Unsubscribe<GameRestartSignal>(OnGameRestart);
            _signalBus.Unsubscribe<PauseSignal>(OnGamePause);
        }

        private void OnGameRestart()
        {
            ChangeGameState(GameStates.Menu);
            Resources.UnloadUnusedAssets();
            GC.Collect();
            DOTween.Clear();
        }

        private void OnGamePause()
        {
            ChangeGameState(GameStates.Pause);
        }

        private void OnFail()
        {
            ChangeGameState(GameStates.Lose);
        }

        private void ChangeGameState(GameStates gameStates)
        {
            GameState = gameStates;
        }

        private void OnGameStart()
        {
            ChangeGameState(GameStates.Game);
        }
    }
}