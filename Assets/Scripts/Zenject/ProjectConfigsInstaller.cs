using Configs;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "ProjectConfigsInstaller", menuName = "Installers/ProjectConfigsInstaller")]
public class ProjectConfigsInstaller : ScriptableObjectInstaller<ProjectConfigsInstaller>
{
    [FormerlySerializedAs("gameSettings")] [SerializeField] private ProjectSettings projectSettings;

    public override void InstallBindings()
    {
        Container.BindInstances(
            projectSettings
        );
    }
}