using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "UiModuleInstaller", menuName = "Installers/UiModuleInstaller")]
public class UiModuleInstaller : ScriptableObjectInstaller<UiModuleInstaller>
{
    public override void InstallBindings()
    {
    }
}