using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enums;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using Configs;

namespace Managers
{
    public class GameManager : IInitializable, IDisposable, ITickable
    {

        private readonly DiContainer _diContainer;
        private readonly ProjectSettings _projectSettings;
        private readonly SignalBus _signalBus;

        private GameStates _gameStates;
        private List<string> _names;
        private float _timer;
        private bool _isGame;

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
                        OnGame();
                        break;

                    case GameStates.Lost:
                        OnGameEnd();
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
            _signalBus.Subscribe<PlayerFailSignal>(OnFail);
            _signalBus.Subscribe<GameRestartSignal>(OnGameRestart);
        }

        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<GameStartSignal>(OnGameStart);
            _signalBus.Unsubscribe<PlayerFailSignal>(OnFail);
            _signalBus.Unsubscribe<GameRestartSignal>(OnGameRestart);
        }

        private void OnGameRestart()
        {
            ChangeGameState(GameStates.Menu);
            Resources.UnloadUnusedAssets();
            GC.Collect();
            DOTween.Clear();
        }


        private void OnFail()
        {
            ChangeGameState(GameStates.Lost);
        }

        private void ChangeGameState(GameStates gameStates)
        {
            GameState = gameStates;
        }

        private void OnGameStart()
        {
            ChangeGameState(GameStates.Game);
        }

        private void OnGame()
        {
            _isGame = true;
        }

        private void OnGameEnd()
        {
            
            _isGame = false;
        }

        public void Tick()
        {
            if (!_isGame)
                return;
        }
    }
}