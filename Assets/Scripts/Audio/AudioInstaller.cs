using UnityEngine;
using UnityEngine.Audio;
using Zenject;

[CreateAssetMenu(fileName = "AudioInstaller", menuName = "Installers/AudioInstaller")]
public class AudioInstaller : ScriptableObjectInstaller<AudioInstaller>
{
    public Sound[] sounds;
    
    public override void InstallBindings()
    {
        Container.Bind<AudioManager>().FromComponentInNewPrefabResource("Assets/Prefabs/GlobalAudioSource.prefab").AsSingle();
        Container.Bind<Sound[]>().FromInstance(sounds);
        Container.Bind<AudioMixer>().FromResource("Assets/Sounds/MainAudioMixer.mixer").AsSingle();
    }
}