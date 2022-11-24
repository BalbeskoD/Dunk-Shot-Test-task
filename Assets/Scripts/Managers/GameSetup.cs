using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameSetup : IInitializable
    {
        private readonly ProjectSettings _projectSettings;

        public GameSetup(ProjectSettings projectSettings)
        {
            _projectSettings = projectSettings;
        }

        public void Initialize()
        {
            Application.targetFrameRate = _projectSettings.TargetFps;
            Input.multiTouchEnabled = _projectSettings.MultiTouchEnable;
            QualitySettings.vSyncCount = 0;
            Physics.reuseCollisionCallbacks = true;
            SynchronizationContext.SetSynchronizationContext(new UniTaskSynchronizationContext());
        }
    }
}