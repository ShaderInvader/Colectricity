using UnityEngine;
using Zenject;

namespace Modules.SceneModule
{
    [CreateAssetMenu(fileName = "SceneModuleInstaller", menuName = "Installers/SceneModuleInstaller")]
    public class SceneModuleInstaller : ScriptableObjectInstaller<SceneModuleInstaller>
    {
        public override void InstallBindings()
        {
        }
    }
}