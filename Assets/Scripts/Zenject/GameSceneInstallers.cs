using UnityEngine;
using Zenject;
using Zenject.Signals;
using Managers;
using UI;

public class GameSceneInstallers : MonoInstaller<GameSceneInstallers>
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Ball ball;
    [SerializeField] private SpawnManager spawnManager;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        Container.BindInstances(playerController, cameraController, ball, spawnManager);
        BinsSignals();
    }

    private void BinsSignals()
    {
        Container.DeclareSignal<ClearGoalSignal>();
        Container.DeclareSignal<GoalSignal>();
        Container.DeclareSignal<FinishSignal>();
        Container.DeclareSignal<PlayerFailSignal>();
        Container.DeclareSignal<GameStartSignal>();
        Container.DeclareSignal<GameStateChangeSignal>();
        Container.DeclareSignal<GameRestartSignal>();
        Container.DeclareSignal<BallReturnSignal>();
        Container.DeclareSignal<PauseSignal>();
        Container.DeclareSignal<BestResultSignal>();
        Container.DeclareSignal<ShotSignal>();
        Container.DeclareSignal<SettingsSignal>();
        Container.DeclareSignal<StarChangeSignal>();
    }
}